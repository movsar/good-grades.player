using GGManager.Services;
using GGManager.Stores;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using Serilog;
using System.IO;
using Shared.Services;
using Data.Services;
using Velopack;
using Shared;

namespace GGManager
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            // Handle unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            AppHost = Host.CreateDefaultBuilder()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddTransient<FileService>();
                        services.AddSingleton<Storage>();
                        services.AddSingleton<MainWindow>();
                        services.AddSingleton<SettingsService>();
                        services.AddSingleton<StylingService>();
                        services.AddSingleton<ContentStore>();
                    }).Build();
        }
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            MessageBox.Show("Произошла непредвиденная ошибка", "Good Grades", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Error(e.Exception, e.Exception.Message);
            Application.Current.Shutdown();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                // Log the exception
                MessageBox.Show("Произошла непредвиденная ошибка", "Good Grades", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Error(ex, ex.Message);
                Application.Current.Shutdown();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost.Start();
            base.OnStartup(e);

            var uiLanguageCode = AppHost.Services.GetRequiredService<SettingsService>().GetValue("uiLanguageCode");
            Translations.SetToCulture(uiLanguageCode ?? "uk");

            var startUpForm = AppHost!.Services.GetRequiredService<MainWindow>();
            startUpForm.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.CloseAndFlush();
            AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}