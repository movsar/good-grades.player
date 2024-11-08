using Data.Entities.TaskItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Shared.Controls
{
    public partial class BuildingItemViewControl : UserControl
    {
        private const double AverageWordWidth = 100;
        private const double Padding = 40; 

        public BuildingItemViewControl(AssignmentItem item)
        {
            InitializeComponent();
            DataContext = this;

            // Перемешать слова и добавить их как кнопки
            var shuffledWords = item.Text.Split(" ").OrderBy(w => Guid.NewGuid()).ToList();
            foreach (string word in shuffledWords)
            {
                var wordContainer = new Grid
                {
                    Width = 100,
                    Height = 30,
                    Margin = new Thickness(1, 0, 1, 0)
                };

                var imgBackground = new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Shared;component/Images/FillingImages/khas.png")),
                    Stretch = Stretch.Fill,
                    Width = 130,
                    Height = 60,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var txtWord = new TextBlock
                {
                    Text = word,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                };

                wordContainer.Children.Add(imgBackground);
                wordContainer.Children.Add(txtWord);

                // Подписка на событие нажатия
                wordContainer.MouseLeftButtonUp += (s, e) =>
                {
                    if (spItemSource.Children.Contains(wordContainer))
                    {
                        spItemSource.Children.Remove(wordContainer);
                        spItemDropZone.Children.Add(wordContainer);
                    }
                    else
                    {
                        spItemDropZone.Children.Remove(wordContainer);
                        spItemSource.Children.Add(wordContainer);
                    }
                };

                spItemSource.Children.Add(wordContainer);
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
            // Фиксируем ширину линии в зависимости от количества слов и их размеров
            double totalWidth = shuffledWords.Sum(word => Math.Max(AverageWordWidth, word.Length * 15)) + Padding;
            dropZoneBorder.Width = totalWidth > 0 ? totalWidth : 100;
            dropZoneBorder.HorizontalAlignment = HorizontalAlignment.Left; 
        }
    }
}