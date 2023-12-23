using Data.Entities;
using Shared.Controls;
using System.Windows;

namespace Shared.Viewers
{
    public partial class CelebrityQuizPresenter : Window
    {
        public CelebrityQuizPresenter(TextToImageQuizEntity celebrityWordsQuiz)
        {
            InitializeComponent();
            foreach (var option in celebrityWordsQuiz.QuizItems)
            {
                //spChallenges.Children.Add(option);
            }

            foreach (var option in celebrityWordsQuiz.QuizItems)
            {
                //spOptions.Children.Add(option);
            }
        }
    }
}