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

namespace Content_Manager.UserControls.MaterialControls
{
    public partial class QuestionControl : UserControl
    {
        private ContentStore _contentStore { get; }
        public QuestionControl()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
        }

        public QuestionControl(TestingQuestion question)
        {
            
        }

        private void _contentStore_SegmentChanged(Segment selectedSegment)
        {
            if (selectedSegment == null) return;

            spItems.Children.Clear();

            foreach (var question in selectedSegment.TestingQuiz.Questions)
            {
                var questionControl = new QuestionControl(question);
                spItems.Children.Add(questionControl);
            }

            spItems.Children.Add(new QuestionControl());
        }
    }
}
