using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Shared.Controls.Assignments
{
    public partial class FillingAssignmentControl : UserControl, IAssignmentViewer
    {
        private readonly FillingAssignment _assignment;

        public int StepsCount { get; } = 1;

        public FillingAssignmentControl(FillingAssignment fillingTask)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = fillingTask;

            tbTitle.Text = _assignment.Title;

            GenerateItemsUI();
        }

        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;

        private void GenerateItemsUI()
        {
            spItems.Children.Clear();

            foreach (var item in _assignment.Items)
            {
                var panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    MaxWidth = 500,
                };

                // Разделяем текст на части для добавления меток и полей для ввода
                string[] parts = item.Text.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < parts.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        // Статический текст как Label
                        var label = new Label
                        {
                            Content = parts[i],
                            Style = (Style)FindResource("FillInLabelStyle"),
                        };
                        panel.Children.Add(label);
                    }
                    else
                    {
                        // Поле для ввода без фона, с подчеркиванием
                        var options = parts[i].Split('|').Select(o => o.ToLower().Trim()).ToArray();
                        var textBox = new TextBox
                        {
                            Tag = options,
                            Width = Math.Min(150, options[0].Length * 8),
                        };
                        textBox.Style = (Style)FindResource("FillInTextBoxStyle");

                        panel.Children.Add(textBox);
                    }
                }
                spItems.Children.Add(panel);
            }
        }

        public void OnCheckClicked()
        {
            foreach (StackPanel panel in spItems.Children)
            {
                foreach (var child in panel.Children)
                {
                    if (child is TextBox textBox)
                    {
                        // Retrieve the options stored in the Tag property
                        var options = (string[])textBox.Tag;

                        // Check if the user input matches one of the options
                        var userInput = TextService.GetChechenString(textBox.Text).ToLower();

                        if (!options.Contains(userInput))
                        {
                            AssignmentCompleted?.Invoke(_assignment, false);
                            return;
                        }
                    }
                }
            }

            // Show a message if all inputs are correct
            AssignmentCompleted?.Invoke(_assignment, true);
        }

        public void OnRetryClicked()
        {
            IsEnabled = true;
        }

        public void OnNextClicked()
        {
            IsEnabled = true;
        }

        public void OnPreviousClicked() { 
            IsEnabled = true;
        }
    }
}
