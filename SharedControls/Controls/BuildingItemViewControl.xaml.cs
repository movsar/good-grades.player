using Data.Entities.TaskItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class BuildingItemViewControl : UserControl
    {
        private const double AverageWordWidth = 180;
        private const double Padding = 40; 

        public BuildingItemViewControl(AssignmentItem item)
        {
            InitializeComponent();
            DataContext = this;

            // Перемешать слова и добавить их как кнопки
            var shuffledWords = item.Text.Split(" ").OrderBy(w => Guid.NewGuid()).ToList();
            foreach (string word in shuffledWords)
            {
                var btnWord = new Button()
                {
                    Content = word,
                    Style = (Style)FindResource("BuilderItemButtonStyle"),
                    FontSize = 24,
                    Width = Math.Max(AverageWordWidth, word.Length * 14 ),
                    Height = 60,
                };

                btnWord.Click += BtnWord_Click;

                spItemSource.Children.Add(btnWord);
            }

            SetStaticDropZoneWidth(shuffledWords);
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

        private void SetStaticDropZoneWidth(List<string> shuffledWords)
        {
            // Фиксируем ширину линии в зависимости от количества слов и отступов
            double totalWidth = shuffledWords.Sum(word => Math.Max(AverageWordWidth, word.Length * 14 )) + Padding;
            dropZoneBorder.Width = totalWidth > 0 ? totalWidth : 100; 
        }
    }
}