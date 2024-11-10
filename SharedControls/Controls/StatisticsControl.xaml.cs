using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class StatisticsControl : UserControl
    {
        public StatisticsControl(int correctAnswers, int incorrectAnswers)
        {
            InitializeComponent();

            txtCorrectAnswers.Text = Translations.GetValue("CorrectAnswersCount") + ": " + correctAnswers;
            txtIncorrectAnswers.Text = Translations.GetValue("IncorrectAnswersCount") + ": " + incorrectAnswers;
        }
    }
}
