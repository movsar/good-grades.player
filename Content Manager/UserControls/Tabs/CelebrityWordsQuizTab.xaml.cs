using Content_Manager.Stores;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
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

namespace Content_Manager.UserControls.Tabs {
    /// <summary>
    /// Interaction logic for CelebrityWordsQuizTab.xaml
    /// </summary>
    public partial class CelebrityWordsQuizTab : UserControl {

        private ContentStore _contentStore { get; }
        public CelebrityWordsQuizTab() {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            //_contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;

            spQuizList.Children.Add(new CwQmControl());
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e) {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {

        }

        //private void _contentStore_SegmentChanged(Segment selectedSegment) {
        //    spQuizList.Children.Clear();

        //    if (selectedSegment == null) return;

        //    foreach (var material in ) {
        //        spQuizList.Children.Add(new CwQmControl(material));
        //    }

        //    spQuizList.Children.Add(new CwQmControl());
        //}
    }
}
