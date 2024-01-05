using Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shared.Viewers
{
    public partial class MatchingViewer : Window
    {
        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            var draggedBorder = e.Data.GetData(typeof(Border)) as Border;

            if (border != null && draggedBorder != null)
            {
                // Check if the types of the children of the source and target borders are the same
                bool isSameType = (draggedBorder.Child.GetType() == border.Child.GetType());

                border.BorderBrush = isSameType ? Brushes.Green : Brushes.Red;
                border.BorderThickness = new Thickness(3);
            }
        }

        private void ResetBorderStyle(Border border)
        {
            border.BorderBrush = Brushes.Transparent;
            border.BorderThickness = new Thickness(0);
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                ResetBorderStyle(border);
            }
        }

        private void Element_Drop(object sender, DragEventArgs e)
        {
            var targetBorder = sender as Border;
            var sourceBorder = e.Data.GetData(typeof(Border)) as Border;

            if (sourceBorder == null || targetBorder == null)
            {
                return;
            }

            ResetBorderStyle(targetBorder);
            ResetBorderStyle(sourceBorder);

            if (sourceBorder == targetBorder)
            {
                return;
            }


            // Get the contents of the Borders
            UIElement sourceContent = sourceBorder.Child;
            UIElement targetContent = targetBorder.Child;

            // Only allow elements of the same type to be swapped
            if (sourceContent.GetType() != targetContent.GetType())
            {
                return;
            }

            // Disconnect the children from their current parent Borders
            sourceBorder.Child = null;
            targetBorder.Child = null;

            // Swap the contents of the Borders
            sourceBorder.Child = targetContent;
            targetBorder.Child = sourceContent;
        }
        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // This marks the start of the drag
            var border = sender as Border;
            if (border != null)
            {
                DragDrop.DoDragDrop(border, new DataObject(typeof(Border), border), DragDropEffects.Move);
            }
        }
        private void Element_MouseMove(object sender, MouseEventArgs e) { }

        public MatchingViewer(MatchingTaskAssignment assignment)
        {
            InitializeComponent();

            var matchingPairs = assignment.Items.Select(i => new MatchingPair
            {
                Image = ConvertByteArrayToBitmapImage(i.Image),
                Text = i.Text
            }).ToList();

            var randomizer = new Random();
            var randomRowIndexesForTexts = Enumerable.Range(0, 4).OrderBy(i => randomizer.Next(0, 4)).ToArray();
            var randomRowIndexesForImages = Enumerable.Range(0, 4).OrderBy(i => randomizer.Next(0, 4)).ToArray();

            var gridItems = new List<GridItem>();

            for (int i = 0; i < matchingPairs.Count(); i++)
            {
                var item = matchingPairs[i];

                var imageUiElement = new Image { Source = item.Image };
                var textBlockUiElement = new TextBlock { Text = item.Text };

                gridItems.Add(new GridItem
                {
                    Element = imageUiElement,
                    Row = randomRowIndexesForImages[i],
                    Column = 0
                });

                gridItems.Add(new GridItem
                {
                    Element = textBlockUiElement,
                    Row = randomRowIndexesForTexts[i],
                    Column = 1
                });
            }

            AddElementsToGrid(gridMatchOptions, gridItems);
        }

        private void AddElementsToGrid(Grid grid, List<GridItem> items)
        {
            foreach (var item in items)
            {
                Border border = new Border
                {
                    Background = Brushes.Transparent,
                    AllowDrop = true
                };

                border.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                border.MouseMove += Element_MouseMove;
                border.Drop += Element_Drop;
                border.DragEnter += Border_DragEnter;
                border.DragLeave += Border_DragLeave;
                border.Padding = new Thickness(10);

                border.Child = item.Element;

                Grid.SetRow(border, item.Row);
                Grid.SetColumn(border, item.Column);

                grid.Children.Add(border);
            }
        }
        public BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
        public class MatchingPair
        {
            public BitmapImage Image { get; set; }
            public string Text { get; set; }
        }

        public class GridItem
        {
            public FrameworkElement Element { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
        }

    }
}