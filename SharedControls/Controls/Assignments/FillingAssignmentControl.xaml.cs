using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
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
                    HorizontalAlignment = HorizontalAlignment.Center
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
                            FontSize = 14,
                            HorizontalAlignment = HorizontalAlignment.Center
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
                            FontSize = 14,
                            BorderBrush = Brushes.Gray,
                            BorderThickness = new Thickness(0, 0, 0, 1), // Подчеркивание
                            Background = Brushes.Transparent, // Прозрачный фон
                            HorizontalAlignment = HorizontalAlignment.Center
                        };
                        textBox.Style = (Style)FindResource("FillInTextBoxStyle");

                        panel.Children.Add(textBox);
                    }
                }
                spItems.Children.Add(panel);
            }
        }

        public bool Check()
        {
            throw new NotImplementedException();
        }

        void IAssignmentViewer.OnCheckClicked()
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

        public void OnRetryClicked()
        {
            throw new NotImplementedException();
        }

        public void OnNextClicked()
        {
            throw new NotImplementedException();
        }
    }
}
