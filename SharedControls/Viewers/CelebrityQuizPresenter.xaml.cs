using Data.Entities.Materials;
using Shared.Controls;
using System.Windows;

namespace Shared.Viewers
{
    public partial class CelebrityQuizPresenter : Window
    {
        public CelebrityQuizPresenter(MatchingTaskMaterial celebrityWordsQuiz)
        {
            InitializeComponent();
            foreach (var option in celebrityWordsQuiz.Matches)
            {
                //spChallenges.Children.Add(option);
            }

            foreach (var option in celebrityWordsQuiz.Matches)
            {
                //spOptions.Children.Add(option);
            }
        }
    }
}