using Data.Entities;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Shared.Viewers
{
    public partial class MatchingViewer : Window
    {
        private readonly Dictionary<string, BitmapImage> _matchingPairs = new Dictionary<string, BitmapImage>();
        #region Event Handlers
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < gridMatchOptions.Children.Count; i++)
            {
                var gridItem = gridMatchOptions.Children[i];
                var border = gridItem as Border;

                var item = border.Child;
                if (item.GetType() == typeof(Image))
                {
                    continue;
                }

                var textValue = ((TextBlock)item).Text;
                var correspondingImage = _matchingPairs[textValue];

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
        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                ResetBorderStyle(border);
            }
        }
        #endregion

        private void ResetBorderStyle(Border border)
        {
            border.BorderBrush = Brushes.Transparent;
            border.BorderThickness = new Thickness(0);
        }

        private void AddElementsToGrid(Grid grid, List<GridItem> items)
        {
            foreach (var item in items)
            {
                var border = CreateBorderWithChild(item.Element);
                Grid.SetRow(border, item.Row);
                Grid.SetColumn(border, item.Column);
                grid.Children.Add(border);
            }
        }

        private Border CreateBorderWithChild(UIElement child)
        {
            var border = new Border
            {
                Background = Brushes.Transparent,
                AllowDrop = true,
                Padding = new Thickness(10),
                Child = child
            };

            // AttachDragDropEvents
            border.MouseLeftButtonDown += Element_MouseLeftButtonDown;
            border.Drop += Element_Drop;
            border.DragEnter += Border_DragEnter;
            border.DragLeave += Border_DragLeave;

            return border;
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

        #region Properties, Fields and Constructors
        public MatchingViewer(MatchingTaskAssignment assignment)
        {
            InitializeComponent();

            foreach (var item in assignment.Items)
            {
                _matchingPairs.Add(item.Text, ConvertByteArrayToBitmapImage(item.Image));
            }

            var randomRowIndexesForTexts = Enumerable.Range(0, 4).OrderBy(i => Guid.NewGuid()).ToArray();
            var randomRowIndexesForImages = Enumerable.Range(0, 4).OrderBy(i => Guid.NewGuid()).ToArray();

            var gridItems = new List<GridItem>();

            for (int i = 0; i < _matchingPairs.Count(); i++)
            {
                var text = _matchingPairs.Keys.ToList()[i];
                var image = _matchingPairs.Values.ToList()[i];

                var imageUiElement = new Image { Source = image };
                var textBlockUiElement = new TextBlock { Text = text };

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
        #endregion

    }
}