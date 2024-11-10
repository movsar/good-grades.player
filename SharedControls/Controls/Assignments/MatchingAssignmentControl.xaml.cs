using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
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

namespace Shared.Controls.Assignments
{
    public partial class MatchingAssignmentControl : UserControl, IAssignmentViewer
    {
        public string TaskTitle { get; }

        public int StepsCount { get; } = 1;

        // A dictionary to hold matching pairs with string identifiers and corresponding images.
        private readonly Dictionary<string, BitmapImage> _matchingPairs = new Dictionary<string, BitmapImage>();
        // The matching task assignment to be completed in this viewer.
        private readonly MatchingAssignment _assignment;

        // An event that signals when the completion state of the assignment changes.
        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;

        #region Properties, Fields and Constructors
        // Constructor initializes the MatchingViewer with a specific assignment.
        public MatchingAssignmentControl(MatchingAssignment assignment)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = assignment;
            TaskTitle = _assignment.Title;

            LoadContent();
        }

        private void LoadContent()
        {
            _matchingPairs.Clear();
            gridMatchOptions.ColumnDefinitions.Clear();
            gridMatchOptions.Children.Clear();

            // Load matching pairs from the assignment into the dictionary.
            foreach (var item in _assignment.Items)
            {
                _matchingPairs.Add(item.Text, ConvertByteArrayToBitmapImage(item.Image));
            }

            // Set up the grid columns based on the number of items to match.
            int numberOfColumns = _assignment.Items.Count;
            for (int i = 0; i < numberOfColumns; i++)
            {
                gridMatchOptions.ColumnDefinitions.Add(
                    new ColumnDefinition { Width = GridLength.Auto }
                );
            }

            // Shuffle the items to randomize their positions in the grid.
            var randomColumnIndexesForTexts = Enumerable.Range(0, numberOfColumns).OrderBy(i => Guid.NewGuid()).ToArray();
            var randomColumnIndexesForImages = Enumerable.Range(0, numberOfColumns).OrderBy(i => Guid.NewGuid()).ToArray();

            var gridItems = new List<GridItem>();

            // Create UI elements for each pair and assign them to random columns.
            for (int i = 0; i < _matchingPairs.Count; i++)
            {
                var text = _matchingPairs.Keys.ToList()[i];
                var image = _matchingPairs.Values.ToList()[i];

                int pairIndex = i;

                var imageUiElement = new Image
                {
                    Source = image,
                    Name = $"Pair_{pairIndex}",
                    Style = (Style)FindResource("MatchingImageStyle")
                };

                var textBlockUiElement = new TextBlock
                {
                    Text = text,
                    Name = $"Pair_{pairIndex}",
                };

                gridItems.Add(new GridItem
                {
                    Element = imageUiElement,
                    Row = 0,
                    Column = randomColumnIndexesForImages[i]
                });

                gridItems.Add(new GridItem
                {
                    Element = textBlockUiElement,
                    Row = 1,
                    Column = randomColumnIndexesForTexts[i]
                });
            }

            // Add the elements to the grid.
            AddElementsToGrid(gridMatchOptions, gridItems);
        }

        #endregion

        //Generic method to find a child element of a specific type in a grid cell.
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

        #region Drag & Drop

        // Handles the drop event during a drag-and-drop operation to swap elements.
        private void Element_Drop(object sender, DragEventArgs e)
        {
            var targetBorder = sender as Border;
            var sourceBorder = e.Data.GetData(typeof(Border)) as Border;

            // Do nothing if the source or target elements are invalid.
            if (sourceBorder == null || targetBorder == null)
            {
                return;
            }

            // Reset styles for both the source and target borders.
            ResetBorderStyle(targetBorder);
            ResetBorderStyle(sourceBorder);

            // Do nothing if attempting to drop an element onto itself.
            if (sourceBorder == targetBorder)
            {
                return;
            }

            // Swap contents only if both elements are of the same type.
            UIElement sourceContent = sourceBorder.Child;
            UIElement targetContent = targetBorder.Child;
            if (sourceContent.GetType() != targetContent.GetType())
            {
                return;
            }

            // Perform the swap of the children between the source and target borders.
            sourceBorder.Child = null;
            targetBorder.Child = null;
            sourceBorder.Child = targetContent;
            targetBorder.Child = sourceContent;
        }

        // Initiates a drag operation when a mouse button is pressed down on an element.
        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                DragDrop.DoDragDrop(border, new DataObject(typeof(Border), border), DragDropEffects.Move);
            }
        }

        // Handles the drag enter event to provide visual feedback for a potential drop.
        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            var draggedBorder = e.Data.GetData(typeof(Border)) as Border;

            // Change border style based on whether the dragged element can be dropped here.
            if (border != null && draggedBorder != null)
            {
                bool isSameType = (draggedBorder.Child.GetType() == border.Child.GetType());
                border.BorderBrush = isSameType ? Brushes.Green : Brushes.Red;
                border.BorderThickness = new Thickness(4);
            }
        }

        // Resets the border style when the dragged element leaves the area.
        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                ResetBorderStyle(border);
            }
        }

        #endregion

        // Resets the border style to default settings.
        private void ResetBorderStyle(Border border)
        {
            // Set to a default or neutral color.
            border.BorderBrush = Brushes.LightGray;
            border.BorderThickness = new Thickness(2);
            border.Background = Brushes.White;
        }

        // Adds UI elements to the grid based on the list of GridItem objects.
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

        // Creates a styled Border element containing a child UIElement.
        private Border CreateBorderWithChild(UIElement child)
        {
            var border = new Border { Child = child };

            // Reference a predefined style for rounded borders.
            border.SetResourceReference(Border.StyleProperty, "RoundedBorderStyle");

            // Attach event handlers for drag-and-drop and hover effects.
            border.MouseLeftButtonDown += Element_MouseLeftButtonDown;
            border.Drop += Element_Drop;
            border.DragEnter += Border_DragEnter;
            border.DragLeave += Border_DragLeave;
            border.MouseEnter += (s, e) =>
            {
                var b = s as Border;
                // Light Sky Blue
                b.Background = new SolidColorBrush(Color.FromArgb(255, 135, 206, 250));
            };
            border.MouseLeave += (s, e) =>
            {
                var b = s as Border;
                b.Background = Brushes.White;
            };

            return border;
        }

        // Converts a byte array to a BitmapImage, useful for displaying images from binary data.
        public BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                // Optimizes memory usage.
                bitmap.Freeze();
                return bitmap;
            }
        }

        public void OnCheckClicked()
        {
            IsEnabled = false;

            // Iterate through grid rows, assuming each row contains an image and text block pair.
            for (int column = 0; column < gridMatchOptions.RowDefinitions.Count; column++)
            {
                var imageBorder = FindChildInGrid<Border>(gridMatchOptions, 0, column);
                var textBlockBorder = FindChildInGrid<Border>(gridMatchOptions, 1, column);

                // Skip the iteration if either image or text block is missing.
                if (imageBorder == null || textBlockBorder == null) continue;

                var image = imageBorder.Child as Image;
                var textBlock = textBlockBorder.Child as TextBlock;

                // Show a message and signal incomplete assignment if any pair does not match.
                if (image != null && textBlock != null && image.Name != textBlock.Name)
                {
                    AssignmentCompleted?.Invoke(_assignment, false);
                    return;
                }
            }

            // If all pairs match, show a success message and signal assignment completion.
            AssignmentCompleted?.Invoke(_assignment, true);
        }

        public void OnRetryClicked()
        {
            IsEnabled = true;
        }

        public void OnNextClicked()
        {
            IsEnabled = true;
        }
    }
}
