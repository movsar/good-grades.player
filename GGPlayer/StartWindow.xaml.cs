
using Data;
using Data.Entities;
using Data.Services;
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
            
            var segmentPage = new SegmentPage(_selectedSegment);
            _navigationService.Navigate(segmentPage);

            this.Close();
        }
    }
}
