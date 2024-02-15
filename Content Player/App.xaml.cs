using Data;
using Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Services;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Content_Player
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GoodGrades", "logs.txt");

            AppHost = Host.CreateDefaultBuilder()
                    .UseSerilog((host, loggerConfiguration) =>
                    {
                        loggerConfiguration.WriteTo
                            .File(logPath, rollingInterval: RollingInterval.Day).MinimumLevel.Debug();
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<Storage>();
                        services.AddSingleton<ShellWindow>();
                        services.AddSingleton<SettingsService>();
                        services.AddSingleton<StylingService>();
                    }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost.Start();
            var startUpForm = AppHost!.Services.GetRequiredService<ShellWindow>();
            startUpForm.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppHost!.StopAsync();
            base.OnExit(e);
        }
    }

}
