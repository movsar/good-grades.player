using Data;
using Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Services;
using System.IO;
using System.Windows;


namespace GGPlayer
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            //создание лога и его запись в файл
            string logPath = Path.Combine("logs", "logs.txt");
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

            // Handle unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            AppHost = Host.CreateDefaultBuilder()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<Storage>();
                        services.AddSingleton<ShellWindow>();
                        services.AddSingleton<SettingsService>();
                        services.AddSingleton<StartWindow>();
                        services.AddSingleton<StylingService>();
                    }).Build();
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Log the exception
            Log.Error(e.Exception, "An unhandled exception occurred.");
            e.Handled = true;
            MessageBox.Show($"Произошла непредвиденная ошибка {e.Exception.Message}");
            Application.Current.Shutdown();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                // Log the exception
                Log.Error(ex, "An unhandled domain exception occurred.");
                MessageBox.Show($"Произошла непредвиденная ошибка {ex.Message}");
                Application.Current.Shutdown();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost.Start();
            var startUpForm = AppHost!.Services.GetRequiredService<StartWindow>();
            startUpForm.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.CloseAndFlush();
            AppHost!.StopAsync();
            base.OnExit(e);
        }
        
    }

}
