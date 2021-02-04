using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Compact;
using System.IO;

namespace Hahn.ApplicatonProcess.December2020.Web
{
    public class Program
    {
        //public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //    .AddEnvironmentVariables()
        //    .Build();
           
        public static void Main(string[] args)
        {
            var configSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(new RenderedCompactJsonFormatter()).WriteTo.Debug(outputTemplate: DateTime.Now.ToString()).WriteTo.File(configSettings["Log:LogPath"], rollingInterval: RollingInterval.Day)
                .CreateLogger();
            //Log.Logger = new LoggerConfiguration()
            ////.MinimumLevel.Debug()
            ////.WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "C:\\logs\\log-{Date}.txt"),
            ////                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}")
            //.ReadFrom.Configuration(Configuration)
            //.CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureLogging(logging =>
            {
                logging.AddFilter("Microsoft", LogLevel.Information);
                logging.AddFilter("System", LogLevel.Error);
            })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        //Host.CreateDefaultBuilder(args)
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<Startup>()
        //        .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
        //        .Enrich.FromLogContext()
        //        .WriteTo.File(@"C:\Temp\log.txt", outputTemplate: "{Timestamp:HH:mm:ss} {Level:u} {Method} {Path} {CorrelationRequestId} {SourceContext} {Message:lj}{NewLine} {Exception}")
        //        ).Build();
        //    });
    }
}
