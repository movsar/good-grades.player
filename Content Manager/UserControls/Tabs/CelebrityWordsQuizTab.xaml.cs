using Content_Manager.Stores;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Controls;
using Shared.Viewers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Content_Manager.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for CelebrityWordsQuizTab.xaml
    /// </summary>
    public partial class CelebrityWordsQuizTab : UserControl
    {
        private ContentStore _contentStore { get; }
        public CelebrityWordsQuizTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.CelebrityWodsQuiz == null)
            {
                return;
            }
            var previewWindow = new CelebrityQuizPresenter(_contentStore.SelectedSegment.CelebrityWodsQuiz);
            previewWindow.ShowDialog();
        }

        private void _contentStore_SegmentChanged(Segment selectedSegment)
        {
            if (selectedSegment == null) return;

            spItems.Children.Clear();

            foreach (var option in selectedSegment.CelebrityWodsQuiz.QuizItems)
            {
                spItems.Children.Add(new QuizItemControl(Data.Enums.QuizTypes.CelebrityWords, option.Id, option.Image, option.Text));
            }

            spItems.Children.Add(new QuizItemControl(Data.Enums.QuizTypes.CelebrityWords));
        }
    }
}
