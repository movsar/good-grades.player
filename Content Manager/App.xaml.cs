using Content_Manager.Models;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Content_Manager
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            // Initialize DB
            Storage storage;
            try
            {
                storage = new Storage(false);
            }
            catch
            {
                storage = new Storage(true);
            }

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(provider => new ContentModel(storage));
                    services.AddSingleton<ContentStore>(); 
                    services.AddSingleton<SegmentInfoTab>();
                    services.AddSingleton<MainWindow>();
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