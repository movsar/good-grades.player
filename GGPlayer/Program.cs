using Data.Services;
using Velopack;

namespace GGPlayer
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            VelopackApp.Build().Run();

            var osVersion = Environment.OSVersion.Version.Major;
            if (osVersion < 10)
            {
                AssetService.CopyFonts();
            }

            var application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }
}
