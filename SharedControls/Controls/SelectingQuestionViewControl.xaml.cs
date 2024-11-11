using Data.Entities.TaskItems;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Shared.Controls
{
    public partial class SelectingQuestionViewControl : UserControl
    {
        public Question Question { get; }
        public List<string> SelectedOptionIds => GetUserSelections();

        private List<string> selectedOptionIds = new List<string>();
        private bool wasPreviousAttemptCorrect = true;

        private readonly SolidColorBrush NEUTRAL_BRUSH = new SolidColorBrush(Color.FromArgb(255, 60, 127, 177));
        private readonly Brush SELECTION_BRUSH = Brushes.DarkBlue;
        private List<string> GetUserSelections()
        {
            return selectedOptionIds;
        }

        public SelectingQuestionViewControl(Question question)
        {
            InitializeComponent();
            DataContext = this;

            Question = question;
            spOptions.Children.Clear();
            for (int i = 0; i < Question.Options.Count; i++)
            {
                AssignmentItem option = Question.Options[i];
                var button = GenerateOptionButton(option);
                if (i % 2 != 0)
                {
                    button.Margin = new Thickness(50, 5, 0, 5);
                }

                spOptions.Children.Add(button);
            }
        }

        private Button GenerateOptionButton(AssignmentItem option)
        {
            var button = new Button()
            {
                Tag = option.Id,
                Background = Brushes.Transparent,
                Content = option.Text,
                Style = (Style)FindResource("SelectingQuestionOptionStyle"),
            };

            button.Click += (s, e) => OnOptionSelected(button);

            return button;
        }

        private void OnOptionSelected(Button selectedButton)
        {
            if (!wasPreviousAttemptCorrect)
            {
                ResetSelections();
                wasPreviousAttemptCorrect = true;
            }

            var selectedId = selectedButton.Tag.ToString();

            if (selectedOptionIds.Contains(selectedId!))
            {
                selectedOptionIds.Remove(selectedId!);
                selectedButton.BorderBrush = NEUTRAL_BRUSH;
            }
            else
            {
                selectedOptionIds.Add(selectedId!);
                selectedButton.BorderBrush = SELECTION_BRUSH;
            }
        }

        public void HighlightCorrectOptions(List<string> correctOptionIds)
        {
            bool allCorrectSelected = correctOptionIds.All(id => SelectedOptionIds.Contains(id));
            bool anyWrongSelected = SelectedOptionIds.Any(id => !correctOptionIds.Contains(id));

            foreach (Button btn in spOptions.Children.OfType<Button>())
            {
                var optionId = btn.Tag.ToString();

                if (SelectedOptionIds.Contains(optionId!))
                {
                    if (anyWrongSelected || !allCorrectSelected)
                    {
                        btn.BorderBrush = Brushes.LightCoral;
                        wasPreviousAttemptCorrect = false;
                    }
                    else if (correctOptionIds.Contains(optionId!) && allCorrectSelected)
                    {
                        btn.BorderBrush = Brushes.LightGreen;
                        wasPreviousAttemptCorrect = true;
                    }
                }
                else
                {
                    btn.ClearValue(Button.BorderBrushProperty);
                }
            }
        }
        public void ResetSelections()
        {
            selectedOptionIds.Clear();
            foreach (Button btn in spOptions.Children.OfType<Button>())
            {
                btn.BorderBrush = NEUTRAL_BRUSH;
            }
        }
    }
}
