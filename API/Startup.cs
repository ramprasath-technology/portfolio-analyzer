using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ConfigService;
using Application.Connection;
using Application.StockAndPurchaseService;
using Application.StockPurchaseService;
using Application.StockQuoteService;
using Application.StockService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.StockData;
using Persistence.StockPurchaseData;
using PortfolioAnalyzer.AlphavantageAnalyzerService;
using PortfolioAnalyzer.FinanancialModelingPrep.FinancialModelingPrepService;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddSingleton<IConfigService, ConfigService>();

            services.AddSingleton<IAlphaVantageAnalyzerService, AlphaVantageAnalyzerService>();
            services.AddSingleton<ICompanyProfileService, CompanyProfileService>();

            services.AddSingleton<IStockQuoteService, StockQuoteService>();
            services.AddSingleton<IStockPurchaseService, StockPurchaseService>();
            services.AddSingleton<IStockService, StockService>();
            services.AddSingleton<IStockAndPurchaseService, StockAndPurchaseService>();

            services.AddSingleton<IStockPurchaseData, StockPurchaseData>();
            services.AddSingleton<IStockData, StockData>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
