﻿using Data.Entities;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using Shared.Translations;

namespace Shared.Viewers
{
    public partial class FillingViewer : Window
    {
        public string TaskTitle { get; }
        public FillingViewer(FillingTaskAssignment fillingTask)
        {
            InitializeComponent();
            DataContext = this;

            TaskTitle = fillingTask.Title;
            GenerateItemsUI(fillingTask);
        }
        private void GenerateItemsUI(FillingTaskAssignment fillingTask)
        {
            foreach (var item in fillingTask.Items)
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
                        panel.Children.Add(new TextBlock { Text = parts[i] });
                    }
                    else
                    {
                        // Add editable text as TextBox
                        var options = parts[i].Split('|').Select(o => o.ToLower().Trim()).ToArray();
                        var textBox = new TextBox { Tag = options, Width = 100 };
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
                            MessageBox.Show(Ru.Incorrect);
                            return;
                        }
                    }
                }
            }

            // Show a message if all inputs are correct
            MessageBox.Show(Ru.Correct);
        }
    }
}