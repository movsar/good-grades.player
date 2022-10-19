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
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e) {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {

        }

        private void _contentStore_SegmentChanged(Segment selectedSegment) {
            if (selectedSegment == null) return;

            spQuizList.Children.Clear();

            foreach (var option in selectedSegment.CelebrityWodsQuiz.Options) {
                spQuizList.Children.Add(new CwqOptionControl(option.Id, option.Image, option.WordsCollection));
            }

            spQuizList.Children.Add(new CwqOptionControl());
        }
    }
}
