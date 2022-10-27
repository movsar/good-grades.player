using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System;
using System.Windows;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Content_Manager
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .UseSerilog((_, loggerConfiguration) =>
                {
                    loggerConfiguration.WriteTo
                        .File("logs.txt", rollingInterval: RollingInterval.Day).MinimumLevel.Error();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<FileService>();
                    services.AddSingleton<Storage>();
                    services.AddSingleton<ContentModel>();
                    services.AddSingleton<ContentStore>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<StylingService>();
                }).Build();
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
            AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}