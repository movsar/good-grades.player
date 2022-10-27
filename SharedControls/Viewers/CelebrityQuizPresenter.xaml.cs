using Data.Interfaces;
using Data.Models;
using Shared.Controls;
using System.Windows;

namespace Shared.Viewers
{
    public partial class CelebrityQuizPresenter : Window
    {
        public CelebrityQuizPresenter(CelebrityWordsQuiz celebrityWordsQuiz)
        {
            InitializeComponent();
            foreach (var option in celebrityWordsQuiz.QuizItems)
            {
                spChallenges.Children.Add(new CwqChallenge(option));
            }

            foreach (var option in celebrityWordsQuiz.QuizItems)
            {
                spOptions.Children.Add(new CwqOptionBox(option));
            }
        }
    }
}