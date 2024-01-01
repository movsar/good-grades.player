using Data.Entities;
using Shared.Controls;
using System.Windows;

namespace Shared.Viewers
{
    public partial class CelebrityQuizPresenter : Window
    {
        public CelebrityQuizPresenter(MatchingTaskAssignment celebrityWordsQuiz)
        {
            InitializeComponent();
            foreach (var option in celebrityWordsQuiz.Items)
            {
                //spChallenges.Children.Add(option);
            }

            foreach (var option in celebrityWordsQuiz.Items)
            {
                //spOptions.Children.Add(option);
            }
        }
    }
}