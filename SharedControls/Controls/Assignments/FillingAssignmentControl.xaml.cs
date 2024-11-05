using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class FillingAssignmentControl : UserControl, IAssignmentViewer
    {
        private readonly FillingAssignment _assignment;

        public string TaskTitle { get; }
        public FillingAssignmentControl(FillingAssignment fillingTask)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = fillingTask;
            TaskTitle = _assignment.Title;
            GenerateItemsUI();
        }

        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemCompleted;

        private void GenerateItemsUI()
        {
            foreach (var item in _assignment.Items)
            {
                // Create a horizontal StackPanel for each item
                var panel = new StackPanel { Orientation = Orientation.Horizontal };

                // Split the text into parts, separating static text from placeholders
                string[] parts = item.Text.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < parts.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        // Add static text as TextBlock
                        panel.Children.Add(new Label
                        {
                            Content = parts[i],
                            Style = (Style)FindResource("FillInLabelStyle")
                        });
                    }
                    else
                    {
                        // Add editable text as TextBox
                        var options = parts[i].Split('|').Select(o => o.ToLower().Trim()).ToArray();

                        var textBox = new TextBox
                        {
                            Tag = options,
                            Width = options[0].Length * 14,
                            FontSize = 24
                        };
                        textBox.Style = (Style)FindResource("FillInTextBoxStyle");
                        panel.Children.Add(textBox);
                    }
                }

                // Add the constructed panel to the StackPanel in the window
                spItems.Children.Add(panel);
            }
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
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
                        if (!options.Contains(textBox.Text.ToLower().Trim()))
                        {
                            MessageBox.Show(Translations.GetValue("Incorrect"));
                            AssignmentCompleted?.Invoke(_assignment, false);
                            return;
                        }
                    }
                }
            }

            // Show a message if all inputs are correct
            MessageBox.Show(Translations.GetValue("Correct"));
            AssignmentCompleted?.Invoke(_assignment, true);
        }

        public bool Check()
        {
            throw new NotImplementedException();
        }

        void IAssignmentViewer.OnCheckClicked()
        {
            throw new NotImplementedException();
        }

        public void OnRetryClicked()
        {
            throw new NotImplementedException();
        }
    }
}
