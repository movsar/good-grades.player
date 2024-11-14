using Microsoft.Extensions.Logging;
using Serilog;
using Shared;
using Shared.Services;
using System;
using System.IO;
using Velopack;

namespace GGManager
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            CultureService.RunPowerShellScriptToRegisterCulture("Register-Culture.ps1");
            Translations.SetToCulture("ce");

            string logPath = Path.Combine("logs", "logs.txt");
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

            VelopackApp.Build().Run(LoggingInstance<Program>());

            var application = new App();
            application.InitializeComponent();
            application.Run();
        }


        public static Microsoft.Extensions.Logging.ILogger LoggingInstance<T>()
        {
            ILoggerFactory factory = new LoggerFactory().AddSerilog(Log.Logger);
            return factory.CreateLogger<T>();
        }
    }
}
