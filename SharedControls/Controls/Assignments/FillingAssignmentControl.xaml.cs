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
                // Create a TextBlock to represent the current item
                var textBlock = new TextBlock
                {
                    Margin = new Thickness(0, 0, 0, 10),
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 500
                };

                // Split line into parts (static text and placeholders)
                string[] parts = item.Text.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                for (int i = 0; i < parts.Length; i++)
                {
                    /*
                        * Text outside of {} becomes even-indexed parts (e.g., index 0, 2, 4, ...)
                        * Placeholder content inside {} becomes odd-indexed parts (e.g., index 1, 3, 5, ...)
                     */
                    if (i % 2 == 0)
                    {
                        // Add static text using Run
                        textBlock.Inlines.Add(new Run(parts[i]));
                    }
                    else
                    {
                        // Add a placeholder as a TextBox using InlineUIContainer
                        var options = parts[i].Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(o => o.ToLower().Trim()).ToArray();

                        var textBox = new TextBox
                        {
                            // Store options for later use
                            Tag = options,
                            // Adjust width dynamically
                            Width = options[0].Length * 8,
                            MaxLength = options[0].Length * 2,
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

        public void OnCheckClicked()
        {
            foreach (var child in spItems.Children)
            {
                if (child is TextBlock textBlock)
                {
                    // Iterate through the inlines of the TextBlock
                    foreach (var inline in textBlock.Inlines)
                    {
                        if (inline is InlineUIContainer container && container.Child is TextBox textBox)
                        {
                            // Retrieve the options stored in the Tag property
                            var options = (string[])textBox.Tag;

                            // Check if the user input matches one of the options
                            var userInput = TextService.GetChechenString(textBox.Text).ToLower();

                            if (!options.Contains(userInput))
                            {
                                // Notify that the assignment is incomplete
                                AssignmentCompleted?.Invoke(_assignment, false);
                                return;
                            }
                        }
                    }
                }
            }

            // If all TextBoxes are correct
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

        public void OnPreviousClicked()
        {
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
