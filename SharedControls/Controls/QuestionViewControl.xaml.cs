using Data.Entities.TaskItems;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Shared.Controls
{
    public partial class QuestionViewControl : UserControl
    {
        public Question Question { get; }
        public List<string> SelectedOptionIds => GetUserSelections();

        private List<string> selectedOptionIds = new List<string>();
        private bool wasPreviousAttemptCorrect = true;
        private List<string> GetUserSelections()
        {
            return selectedOptionIds;
        }

        public QuestionViewControl(Question question)
        {
            Question = question;
            InitializeComponent();
            DataContext = this;

            spOptions.Children.Clear();
            foreach (var option in Question.Options)
            {
                spOptions.Children.Add(GenerateOptionButton(option));
            }
        }

        private Button GenerateOptionButton(AssignmentItem option)
        {
            var button = new Button()
            {
                Tag = option.Id,
                Content = option.Text,
                Margin = new System.Windows.Thickness(10),
                Padding = new System.Windows.Thickness(20),
                FontSize = 24,
                Foreground = Brushes.Black,
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(3),
                FontWeight = FontWeights.Bold,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    ShadowDepth = 5,
                    Opacity = 0.3
                }
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
                selectedButton.Background = Brushes.Transparent;
            }
            else
            {
                selectedOptionIds.Add(selectedId!);
                selectedButton.Background = Brushes.LightBlue;
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
                        btn.Background = Brushes.LightCoral;
                        wasPreviousAttemptCorrect = false;
                    }
                    else if (correctOptionIds.Contains(optionId!) && allCorrectSelected)
                    {
                        btn.Background = Brushes.LightGreen;
                        wasPreviousAttemptCorrect = true;
                    }
                }
                else
                {
                    btn.ClearValue(Button.BackgroundProperty);
                }
            }
        }
        public void ResetSelections()
            {
                selectedOptionIds.Clear();
                foreach (Button btn in spOptions.Children.OfType<Button>())
                {
                    btn.Background = null;
                }
            }
        }
    } 
