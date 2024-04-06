using Data.Services;
using System;
using Velopack;

namespace GGManager
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            VelopackApp.Build().Run();

            var application = new App();
            application.InitializeComponent();
            application.Run();
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
