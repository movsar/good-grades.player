﻿using Content_Manager.Stores;
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

namespace Content_Manager.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for ProverbsBuilderQuizTab.xaml
    /// </summary>
    public partial class ProverbBuilderQuizTab : UserControl
    {
        private ContentStore _contentStore { get; }
        public ProverbBuilderQuizTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.ProverbBuilderQuiz == null)
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

            foreach (var quizItem in selectedSegment.ProverbBuilderQuiz.QuizItems)
            {
                spItems.Children.Add(new QuizItemControl(Data.Enums.QuizTypes.ProverbBuilder, quizItem.Id!, quizItem!.Image, quizItem.Text, selectedSegment.ProverbSelectionQuiz.CorrectProverbId == quizItem.Id));
            }

            spItems.Children.Add(new QuizItemControl(Data.Enums.QuizTypes.ProverbBuilder));
        }
    }
}
