using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Velopack;

namespace GGManager
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string logPath = Path.Combine("logs", "logs.txt");
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

            VelopackApp.Build().Run(LoggingInstance<Program>());
            // SetToChechenCulture();
            
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }
        private static void SetToChechenCulture()
        {
            // Set the culture to Chechen ("ce")
            CultureInfo cultureInfo = new CultureInfo("ce");

            // Set both the current culture and the current UI culture
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        public static Microsoft.Extensions.Logging.ILogger LoggingInstance<T>()
        {
            ILoggerFactory factory = new LoggerFactory().AddSerilog(Log.Logger);
            return factory.CreateLogger<T>();
        }

        //private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        //{
        //    tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        //}

        //private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
        //{
        //    tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        //}
    }
}
