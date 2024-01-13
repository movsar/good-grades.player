using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shared.Controls
{
    public partial class BuildingItemViewControl : UserControl
    {
        // Property to indicate if the arrangement is correct
        public bool IsCorrectlyArranged { get; set; } = false;

        // The original phrase for comparison
        private string _originalPhrase;
        private List<string> _shuffledWords = new List<string>();

        public BuildingItemViewControl(string phrase)
        {
            InitializeComponent();
            DataContext = this;

            _originalPhrase = phrase;

            // Shuffle words and add them as buttons
            _shuffledWords = phrase.Split(" ").OrderBy(w => Guid.NewGuid()).ToList();
            foreach (string word in _shuffledWords)
            {
                var btnWord = new Button()
                {
                    Content = word,
                    Style = (Style)FindResource("BuilderItemButtonStyle"),
                    AllowDrop = true
                };

                // Event handlers for drag and drop
                btnWord.PreviewMouseLeftButtonDown += BtnWord_MouseLeftButtonDown;
                btnWord.Drop += BtnWord_Drop;

                spItems.Children.Add(btnWord);
            }
        }

        private void BtnWord_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Begin drag operation
            Button btnWord = sender as Button;
            DragDrop.DoDragDrop(btnWord, btnWord.Content, DragDropEffects.Move);
        }

        private void BtnWord_Drop(object sender, DragEventArgs e)
        {
            // Handle drop operation
            Button target = sender as Button;
            string sourceWord = e.Data.GetData(DataFormats.StringFormat) as string;
            int targetIndex = _shuffledWords.IndexOf(target.Content as string);
            int sourceIndex = _shuffledWords.IndexOf(sourceWord);

            // Swap words
            _shuffledWords[targetIndex] = sourceWord;
            _shuffledWords[sourceIndex] = target.Content as string;

            // Update UI
            spItems.Children.Clear();
            foreach (string word in _shuffledWords)
            {
                var btnWord = new Button()
                {
                    Content = word,
                    AllowDrop = true,
                    Style = (Style)FindResource("BuilderItemButtonStyle"),
                };

                btnWord.PreviewMouseLeftButtonDown += BtnWord_MouseLeftButtonDown;
                btnWord.Drop += BtnWord_Drop;

                spItems.Children.Add(btnWord);
            }

            // Check if correctly arranged
            CheckArrangement();
        }

        private void CheckArrangement()
        {
            // Check if the current arrangement matches the original phrase
            IsCorrectlyArranged = string.Join(" ", _shuffledWords) == _originalPhrase;

            // You can add more logic here to notify about the correct arrangement
        }
    }
}