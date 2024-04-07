using System.Reflection;
using System.Windows;

namespace GGManager.Windows
{
    public partial class AboutWindow : Window
    {
        public string AppVersion { get; }
        public AboutWindow()
        {
            InitializeComponent();
            string? appVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            Title += " " + appVersion;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
