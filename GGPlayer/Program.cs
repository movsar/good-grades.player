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

            var application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }
}
