using Application.StockAndPurchaseService;
using Application.StockHoldingService;
using Application.StockPurchaseService;
using Domain;
using Domain.DTO;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.TransactionHistory.Fidelity.FidelityTransactionTypes;

namespace Application.TransactionHistory.Fidelity
{
    public class FidelityTransactionHistoryService : ITransactionHistoryService
    {
        public delegate ITransactionHistoryService ServiceResolver(string key);
        private readonly IStockAndPurchaseService _stockPurchaseService;
        private readonly IStockPurchaseService _purchaseService;
        private readonly IStockHoldingService _stockHoldingService;

        public FidelityTransactionHistoryService(IStockAndPurchaseService stockPurchaseService,
            IStockPurchaseService purchaseService,
            IStockHoldingService stockHoldingService)
        {
            _stockPurchaseService = stockPurchaseService;
            _purchaseService = purchaseService;
            _stockHoldingService = stockHoldingService;
        }

        public async Task ImportTransactionHistory(ulong userId, string fileName)
        {
            var transactionCollection = await Task.Run(() => ReadPurchasesFromTransactions(fileName));
            if (transactionCollection.Count > 0)
            {
                var allowedTypesForFiltering = new List<string>()
                {
                    FidelityTransactionTypes.Purchase,
                    DividendReinvestment
                };
                var lastPurchaseDate = await GetLastPurchaseDateForUser(userId);
                var columnMapping = GetColumnMapping(transactionCollection[0]);
                var filteredTransactions = await Task.Run(() => GetTransactionsOfSpecificTypesAfterSpecificDate
                (
                    transactionCollection[0],
                    lastPurchaseDate,
                    allowedTypesForFiltering,
                    columnMapping)
                );
                if (filteredTransactions.Count > 0)
                {
                    var purchases = await Task.Run(() => MapTransactionsToPurchases
                    (
                        userId,
                        filteredTransactions,
                        columnMapping)
                    );
                    foreach (var purchase in purchases)
                    {
                        var stockPurchase = await _stockPurchaseService.AddStockAndPurchaseInfo(purchase);
                        await _stockHoldingService.AddPurchaseToHoldings(userId, stockPurchase.Purchase);
                    }
                }
            }          
        }

        private DataTableCollection ReadPurchasesFromTransactions(string fileName)
        {
            var filePath = $"{fileName}";
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    return result.Tables;
                }
            }
        }

        private IEnumerable<StockPurchase> MapTransactionsToPurchases(ulong userId, 
            List<object[]> transactions,          
            FidelityTransactionColumns columns)
        {
            var purchases = new List<StockPurchase>();
            for (var i = 0; i < transactions.Count; i++)
            {
                try
                {
                    var purchase = new StockPurchase()
                    {
                        Comment = string.Empty,
                        PurchaseDate = Convert.ToDateTime(transactions[i][columns.RunDate]),
                        Price = Convert.ToDecimal(transactions[i][columns.Price]),
                        Quantity = Convert.ToDecimal(transactions[i][columns.Quantity]),
                        Ticker = Convert.ToString(transactions[i][columns.Symbol]),
                        UserId = userId
                    };
                    purchases.Add(purchase);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return purchases;
        }

        private List<object[]> GetTransactionsOfSpecificTypesAfterSpecificDate(DataTable transactions,
            DateTime lastTransactionDateOnRecord,
            List<string> allowedTypes,
            FidelityTransactionColumns columns)
        {
            var filteredData = new List<object[]>();
            for (var i = 0; i < transactions.Rows.Count; i++)
            {
                var assumedPurchaseDate = transactions.Rows[i][columns.RunDate];
                var purchaseDate = DateTime.MinValue;
                var isPurchaseDate = DateTime.TryParse(assumedPurchaseDate.ToString(), out purchaseDate);
                if (isPurchaseDate)
                {
                    if (purchaseDate.Date > lastTransactionDateOnRecord.Date)
                    {
                        var transactionType = transactions.Rows[i][columns.Action];
                        if (transactionType != null &&
                            transactionType.GetType() == typeof(string) &&
                            DoesTransactionTypeMatch(allowedTypes, transactionType.ToString()) &&
                            !IsSecurityDescriptionCash(transactions.Rows[i][columns.SecurityDescription].ToString())
                            )
                        {
                            filteredData.Add(transactions.Rows[i].ItemArray);
                        }
                    }
                }               
            }

            return filteredData;
        }

        private FidelityTransactionColumns GetColumnMapping(DataTable transactions)
        {
            var columnMapping = new FidelityTransactionColumns();
            for (var i = 0; i < transactions.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(transactions.Rows[i][0].ToString()))
                {
                    for (var j = 0; j < transactions.Rows[i].ItemArray.Length; j++)
                    {
                        var columnHeader = transactions.Rows[i][j].ToString();
                        MapCurrentColumnNameToIndex(columnHeader, columnMapping, j);
                    }
                    if (CheckIfColumnIndexesAreSet(columnMapping))
                    {
                        break;
                    }
                }
            }
            return columnMapping;
        }

        private void MapCurrentColumnNameToIndex(string columnHeader, FidelityTransactionColumns columns, int index)
        {
            if (columnHeader.Contains(FidelityColumnNames.ACTION))
            {
                columns.Action = index;
            }
            else if (columnHeader.Contains(FidelityColumnNames.PRICE))
            {
                columns.Price = index;
            }
            else if (columnHeader.Contains(FidelityColumnNames.QUANTITY))
            {
                columns.Quantity = index;
            }
            else if (columnHeader.Contains(FidelityColumnNames.SYMBOL))
            {
                columns.Symbol = index;
            }
            else if (columnHeader.Contains(FidelityColumnNames.TRANSACTION_DATE))
            {
                columns.RunDate = index;
            }
            else if (columnHeader.Contains(FidelityColumnNames.SECURITY_DESCRIPTION))
            {
                columns.SecurityDescription = index;
            }
        }

        private bool CheckIfColumnIndexesAreSet(FidelityTransactionColumns columnMapping)
        {
            return columnMapping.GetType().GetProperties().Any(x => Convert.ToInt32(x.GetValue(columnMapping)) > 0);
        }

        private bool DoesTransactionTypeMatch(List<string> allowedTypes,
            string currentRowType)
        {
            foreach (var type in allowedTypes)
            {
                if (currentRowType.Contains(type))
                    return true;
            }

            return false;
        }

        private bool IsSecurityDescriptionCash(string securityDescription)
        {
            if (securityDescription.ToLower() == "cash")
            {
                return true;
            }
            return false;
        }

        private async Task<DateTime> GetLastPurchaseDateForUser(ulong userId)
        {
            var lastPurchase = await _purchaseService.GetLastPurchaseForUser(userId);

            if (lastPurchase != null)
            {
                return lastPurchase.Date;
            }

            return DateTime.MinValue;
        }
    }

}
