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
            foreach (var option in celebrityWordsQuiz.Options)
            {
                spChallenges.Children.Add(new CwqOptionChallenge(option));
            }

            foreach (var option in celebrityWordsQuiz.Options)
            {
                spOptions.Children.Add(new CwqOptionBox(option));
            }
        }
    }
}