using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Shared.Controls.Assignments
{
    public partial class FillingAssignmentControl : UserControl, IAssignmentControl
    {
        public int StepsCount { get; } = 1;
        private FillingAssignment _assignment;
        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;
        public FillingAssignmentControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void GenerateItemsUI()
        {
            spItems.Children.Clear();
            foreach (var item in _assignment.Items)
            {
                // Split the text into lines to preserve formatting
                string[] lines = item.Text.Split(new[] { '\n' }, StringSplitOptions.None);

                foreach (var line in lines)
                {
                    // Create a TextBlock to handle inline elements
                    var textBlock = new TextBlock
                    {
                        TextWrapping = TextWrapping.Wrap,
                        MaxWidth = 500
                    };

                    // Split line into parts (static text and placeholders)
                    string[] parts = line.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            // Add static text using Run
                            textBlock.Inlines.Add(new Run(parts[i]));
                        }
                        else
                        {
                            // Add a placeholder as a TextBox using InlineUIContainer
                            var options = parts[i].Split('|').Select(o => o.ToLower().Trim()).ToArray();

                            var textBox = new TextBox
                            {
                                Tag = options, // Store options for later use
                                Width = options[0].Length * 8, // Adjust width dynamically
                                Margin = new Thickness(2, 0, 2, 0),
                                Style = (Style)FindResource("FillInTextBoxStyle")
                            };

                            // Wrap the TextBox in InlineUIContainer
                            var container = new InlineUIContainer(textBox)
                            {
                                BaselineAlignment = BaselineAlignment.Center
                            };

                            textBlock.Inlines.Add(container);
                        }
                    }

                    // Add the formatted TextBlock to the main StackPanel
                    spItems.Children.Add(textBlock);
                }
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

        public void Initialize(IAssignment assignment)
        {
            IsEnabled = true;
            if (_assignment == assignment)
            {
                return;
            }
            _assignment = (FillingAssignment)assignment;

            GenerateItemsUI();
        }
    }
}
