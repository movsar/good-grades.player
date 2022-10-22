using Content_Manager.Stores;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Viewers;
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

namespace Content_Manager.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for ProverbSelectionQuizTab.xaml
    /// </summary>
    public partial class ProverbSelectionQuizTab : UserControl
    {
        private ContentStore _contentStore { get; }
        public ProverbSelectionQuizTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.ProverbSelectionQuiz == null)
            {
                return;
            }
            //var previewWindow = new ProverbSelectionQuizViewer(_contentStore.SelectedSegment.ProverbSelectionQuiz);
            //previewWindow.ShowDialog();
        }

        private void _contentStore_SegmentChanged(Segment selectedSegment)
        {
            if (selectedSegment == null) return;

            spItems.Children.Clear();

            foreach (var option in selectedSegment.CelebrityWodsQuiz.Options)
            {
                spItems.Children.Add(new QuizItemControl(option.Id, option.Image, option.Text));
            }

            spItems.Children.Add(new QuizItemControl());
        }
    }
}
