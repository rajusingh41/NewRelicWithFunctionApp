using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewRelic.LogEnrichers.Serilog;
using Serilog;
using Serilog.Events;
using System;

namespace NewRelicWithFunctionApp
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
               
                .ConfigureServices(services =>
                {
                    services.AddLogging(loggingBuilder =>
                    {
                        Log.Logger = new LoggerConfiguration()
                           .WriteTo.Console() // Logs to console (for local debugging)
                           .MinimumLevel.Information()
                           .Enrich.FromLogContext()
                           .Enrich.WithNewRelicLogsInContext()
                           .WriteTo.NewRelicLogs(applicationName: "Testapp",
                                licenseKey: "3486610d8f75a0a59d60eb0193b47b07FFFFNRAL")
                           .CreateLogger();

                        loggingBuilder.AddSerilog();
                    });
                })
                .Build();

            host.Run();
        }
    }
}