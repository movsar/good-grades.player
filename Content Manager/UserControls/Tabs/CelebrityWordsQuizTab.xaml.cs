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
            //var cwq = _contentStore.UpdateItem
            //spQuizList.Children.Clear();

            if (selectedSegment == null) return;

            var quizId = selectedSegment.CelebrityWodsQuiz!.Id!;

            foreach (var kvpToId in selectedSegment.CelebrityWodsQuiz.Data) {
                spQuizList.Children.Add(new CwQmControl(quizId, kvpToId.Key, kvpToId.Value.Key, kvpToId.Value.Value));
            }

            spQuizList.Children.Add(new CwQmControl(quizId));
        }
    }
}
