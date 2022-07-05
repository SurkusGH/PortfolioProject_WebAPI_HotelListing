using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog; // <- this line enables Serilog package usage
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioProject_WebAPI_HotelListing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Serilog
            Log.Logger = new LoggerConfiguration() // <- initiate logger
               .WriteTo.File(
                    path: "d:\\hotelListing\\logs\\log-.txt", // <- set where the logger creates log file
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{level:u3}] {Message:lj}{NewLine}{Exception}",// <- Formating
                    rollingInterval: RollingInterval.Day, // <- indicates how often "log-[index] file is created"
                    restrictedToMinimumLevel: LogEventLevel.Information // <- using Serilog.Events; 
                    ).CreateLogger(); // <- creates logger w/ aforementioned parameters
            try
            {
                Log.Information("Application is starting..."); // <- with this we log the fact that application launches in a log file
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "(!) Application Failed to start..."); // <- with this we log the fact that application launch failed in a log file
            }
            finally
            {
                Log.CloseAndFlush();
            }
            #endregion
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // <- this line enables Serilog
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
