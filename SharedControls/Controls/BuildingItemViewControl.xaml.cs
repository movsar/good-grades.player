using Data.Entities.TaskItems;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class BuildingItemViewControl : UserControl
    {
        public BuildingItemViewControl(AssignmentItem item)
        {
            InitializeComponent();
            DataContext = this;

            // Shuffle words and add them as buttons
            var shuffledWords = item.Text.Split(" ").OrderBy(w => Guid.NewGuid()).ToList();
            foreach (string word in shuffledWords)
            {
                var btnWord = new Button()
                {
                    Content = word,
                    Style = (Style)FindResource("BuilderItemButtonStyle"),
                    FontSize = 24, 
                    Width = 180, 
                    Height = 60,
                };

                // Event handlers for drag and drop
                btnWord.Click += BtnWord_Click;

                spItemSource.Children.Add(btnWord);
            }
        }

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            Button btnWord = sender as Button;
            if (spItemSource.Children.Contains(btnWord))
            {
                spItemSource.Children.Remove(btnWord);
                spItemDropZone.Children.Add(btnWord);
            }
            else
            {
                spItemDropZone.Children.Remove(btnWord);
                spItemSource.Children.Add(btnWord);
            }
        }
    }
}