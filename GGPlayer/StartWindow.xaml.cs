
using Data;
using Data.Entities;
using Data.Services;
using GGManager;
using GGManager.Stores;
using GGPlayer.Pages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GGPlayer
{
    public partial class StartWindow : Window
    {

        public StartWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var logger = new Logger<Storage>(new LoggerFactory());
            var storage = new Storage(logger);
            var settingsService = new SettingsService();
            var contentStore = new ContentStore(storage, settingsService);

            // Создание и показ основного окна
            var shellWindow = new ShellWindow();
            shellWindow.Show();

            // Закрытие стартового окна
            this.Close();
        }
    }
}
