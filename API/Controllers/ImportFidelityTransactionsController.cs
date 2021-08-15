using Application.ConfigService;
using Application.TransactionHistory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Application.TransactionHistory.Fidelity.FidelityTransactionHistoryService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportFidelityTransactionsController : ControllerBase
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IConfigService _configService;
        public ImportFidelityTransactionsController(ServiceResolver serviceAccessor,
            IConfigService configService)
        {
            _transactionHistoryService = serviceAccessor(TransactionTypes.FIDELITY);
            _configService = configService;
        }

        [HttpPost("ImportTransactions/{userId}/{fileName}")]
        public async Task<IActionResult> ImportTransactions(ulong userId, string fileName)
        {
            try
            {
                var path = $"{_configService.GetFidelityFilePath()}{fileName}";
                await _transactionHistoryService.ImportTransactionHistory(userId, path);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
