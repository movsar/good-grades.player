using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using Shared.Models;
using Shared.Translations;
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
    public partial class MatchingViewer : Window, IAssignmentViewer
    {
        private readonly Dictionary<string, BitmapImage> _matchingPairs = new Dictionary<string, BitmapImage>();
        private readonly MatchingTaskAssignment _assignment;

        public event Action<IAssignment, bool> CompletionStateChanged;
        #region Event Handlers
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Assuming grid rows and columns are filled in pairs (Image in column 0 and TextBlock in column 1)
            for (int row = 0; row < gridMatchOptions.RowDefinitions.Count; row++)
            {
                var imageBorder = FindChildInGrid<Border>(gridMatchOptions, row, 0);
                var textBlockBorder = FindChildInGrid<Border>(gridMatchOptions, row, 1);

                if (imageBorder == null || textBlockBorder == null) continue;

                var image = imageBorder.Child as Image;
                var textBlock = textBlockBorder.Child as TextBlock;

                if (image != null && textBlock != null && image.Name != textBlock.Name)
                {
                    MessageBox.Show(Ru.ElementsDoNotMatch);
                    CompletionStateChanged?.Invoke(_assignment, false);
                    return;
                }
            }

            MessageBox.Show(Ru.AllElementsMatch);
            CompletionStateChanged?.Invoke(_assignment, true);
        }
        private T? FindChildInGrid<T>(Grid grid, int row, int column) where T : FrameworkElement
        {
            foreach (FrameworkElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column && child is T)
                {
                    return (T)child;
                }
            }
            return null;
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

            _assignment = assignment;
            foreach (var item in _assignment.Items)
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

                int pairIndex = i;
                var imageUiElement = new Image { Source = image, Name = $"Pair_{pairIndex}" };
                var textBlockUiElement = new TextBlock { Text = text, Name = $"Pair_{pairIndex}" };

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