using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Player;
using Content_Player.Pages;
using Data;
using Data.Entities;
using Data.Services;
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

namespace Content_Manager.Windows
{
    public partial class StartWindow : Window
    {
        private readonly Segment _selectedSegment;
        private readonly NavigationService _navigationService;

        public StartWindow(Segment selectedSegment, NavigationService navigationService)
        {
            InitializeComponent();
            _selectedSegment = selectedSegment;
            _navigationService = navigationService;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Навигация к SegmentPage с выбранным сегментом
            var segmentPage = new SegmentPage(_selectedSegment);
            _navigationService.Navigate(segmentPage);

            // Закрытие стартового окна
            this.Close();
        }
    }
}


