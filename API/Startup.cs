using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ConfigService;
using Application.Connection;
using Application.StockHoldingService;
using Application.StockAnalyzerService;
using Application.StockAndPurchaseService;
using Application.StockPurchaseService;
using Application.StockQuoteService;
using Application.StockService;
using Application.UserService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.HoldingsData;
using Persistence.StockData;
using Persistence.StockPurchaseData;
using Persistence.UserData;
using PortfolioAnalyzer.AlphavantageAnalyzerService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService;
using Application.StockIndexTickerService;
using Application.StockIndexValueService;
using Persistence.StockIndexTickerData;
using Persistence.StockIndexValueData;
using Application.MarketDataService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.DataOrchestrationService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService.LastStockQuoteService;
using Application.StockReturnsService;
using Persistence.StockSaleData;
using Application.StockSaleService;
using Application.StockIndexComparisonService;
using Application.StockTotalValueService;
using Application.TotalValueComparisonService;
using Application.PortfolioCompositionService;
using Application.IndexAnalysisService;
using Application.WebScraperService;
using Application.WebScraperService.CNNScraper;
using Application.CompositionAndRecommendationService;
using Application.StockRecommendationService;
using Application.StockSplitService;
using Application.IndividualStockComparisonService;
using Application.LifeTimeReturnsComparisonService;
using Application.TransactionHistory.Fidelity;
using static Application.TransactionHistory.Fidelity.FidelityTransactionHistoryService;
using ExternalServices.OrchestrationService;
using static Application.MarketDataService.MarketDataService;
using PortfolioAnalyzer.Alphavantage.DataOrchestrationService;
using static PortfolioAnalyzer.Alphavantage.DataOrchestrationService.AlphavantageDataOrchestrationService;
using static Application.ConfigService.AlphaVantageServiceConfig;
using Domain.DTO.ExternalData;
using Application.TransactionHistory;

namespace API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_allowedOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://192.168.101.1:3000/");
                                  });
            });
            services.AddControllers();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            services.AddTransient<FidelityTransactionHistoryService>();          
            services.AddTransient<FinancialModellingPrepDataOrchestrationService>();
            services.AddTransient<AlphaVantageServiceConfig>();
            services.AddTransient<FinancialModelPrepServiceConfig>();
            services.AddTransient<AlphavantageDataOrchestrationService>();

            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case TransactionTypes.FIDELITY:
                        return serviceProvider.GetService<FidelityTransactionHistoryService>();                
                    default:
                        throw new KeyNotFoundException();
                }
            });
            services.AddTransient<ExternalServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case ExternalServiceTypes.ALPHAVANTAGE:
                        return serviceProvider.GetService<AlphavantageDataOrchestrationService>();
                    case ExternalServiceTypes.FINANCIALMODELINGPREP:
                        return serviceProvider.GetService<FinancialModellingPrepDataOrchestrationService>();
                    default:
                        throw new KeyNotFoundException();
                }
            });
            services.AddTransient<ExternalServiceConfigResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case ExternalServiceTypes.ALPHAVANTAGE:
                        return serviceProvider.GetService<AlphaVantageServiceConfig>();
                    case ExternalServiceTypes.FINANCIALMODELINGPREP:
                        return serviceProvider.GetService<FinancialModelPrepServiceConfig>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddSingleton<IConfigService, ConfigService>();

            services.AddSingleton<IAlphaVantageAnalyzerService, AlphaVantageAnalyzerService>();
            services.AddSingleton<ICompanyProfileService, CompanyProfileService>();

            services.AddSingleton<IStockQuoteService, StockQuoteService>();
            services.AddSingleton<IStockPurchaseService, StockPurchaseService>();
            services.AddSingleton<IStockSaleService, StockSaleService>();
            services.AddSingleton<IStockService, StockService>();
            services.AddSingleton<IStockAndPurchaseService, StockAndPurchaseService>();
            services.AddSingleton<IStockIndexComparisonService, StockIndexComparisonService>();
            services.AddSingleton<IStockReturnsService, StockReturnsService>();
            services.AddSingleton<IStockAnalyzerService, StockAnalyzerService>();
            services.AddSingleton<IStockHoldingService, StockHoldingService>();
            services.AddSingleton<IStockIndexTickerService, StockIndexTickerService>();
            services.AddSingleton<IStockIndexValueService, StockIndexValueService>();
            services.AddSingleton<IStockTotalValueService, StockTotalValueService>();
            services.AddSingleton<ITotalValueComparisonService, TotalValueComparisonService>();
            services.AddSingleton<IPortfolioCompositionService, PortfolioCompositionService>();
            services.AddSingleton<IMarketDataService, MarketDataService>();        
            services.AddSingleton<IDailyStockPriceService, DailyStockPriceService>();
            services.AddSingleton<ILastStockQuoteService, LastStockQuoteService>();
            services.AddSingleton<IIndexAnalysisService, IndexAnalysisService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IWebScraperService, CNNScraperService>();
            services.AddSingleton<ICompositionAndRecommendationService, CompositionAndRecommendationService>();
            services.AddSingleton<IStockRecommendationService, StockRecommendationService>();
            services.AddSingleton<IStockSplitService, StockSplitService>();
            services.AddSingleton<IIndividualStockComparisonService, IndividualStockComparisonService>();
            services.AddSingleton<ILifeTimeReturnsComparisonService, LifeTimeReturnsComparisonService>();
          
            services.AddSingleton<IStockPurchaseData, StockPurchaseData>();
            services.AddSingleton<IStockSaleData, StockSaleData>();
            services.AddSingleton<IStockData, StockData>();
            services.AddSingleton<IStockIndexTickerData, StockIndexTickerData>();
            services.AddSingleton<IStockIndexValueData, StockIndexValueData>();
            services.AddSingleton<IUserData, UserData>();
            services.AddSingleton<IHoldingsData, HoldingsData>();

            services.AddSingleton<PortfolioAnalyzer.Alphavantage.CompanyProfileService.ICompanyProfileService, 
                PortfolioAnalyzer.Alphavantage.CompanyProfileService.CompanyProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
