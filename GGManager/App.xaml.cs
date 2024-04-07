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

            ExceptionService.HandleError(e.Exception, e.Exception.Message);
            Application.Current.Shutdown();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                // Log the exception
                ExceptionService.HandleError(ex, "Произошла непредвиденная ошибка ");
                Application.Current.Shutdown();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost.Start();
            var startUpForm = AppHost!.Services.GetRequiredService<MainWindow>();
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