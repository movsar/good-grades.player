using Microsoft.Extensions.Logging;
using Serilog;
using Shared.Services;
using System;
using System.Diagnostics;
using System.IO;
using Velopack;

namespace GGManager
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (IsWindows7())
            {
                RunFontInstallScript();
            }
            string logPath = Path.Combine("logs", "logs.txt");
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

            VelopackApp.Build().Run(LoggingInstance<Program>());

            var application = new App();
            application.InitializeComponent();
            application.Run();
        }


        private static bool IsWindows7()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT &&
                   Environment.OSVersion.Version.Major == 6 &&
                   Environment.OSVersion.Version.Minor == 1;
        }

        private static void RunFontInstallScript()
        {
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Fonts", "InstallFont.bat");

            if (!File.Exists(scriptPath))
            {
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = scriptPath,
                    Verb = "runas",
                    UseShellExecute = true
                })?.WaitForExit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while running the script: {ex.Message}");
            }
        }

        public static Microsoft.Extensions.Logging.ILogger LoggingInstance<T>()
        {
            ILoggerFactory factory = new LoggerFactory().AddSerilog(Log.Logger);
            return factory.CreateLogger<T>();
        }
    }
}
