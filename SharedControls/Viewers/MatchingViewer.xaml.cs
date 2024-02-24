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

namespace Shared.Viewers
{
    // Defines a MatchingViewer class as a specialized window for displaying matching tasks.
    public partial class MatchingViewer : Window, IAssignmentViewer
    {
        public string TaskTitle { get; }

        // A dictionary to hold matching pairs with string identifiers and corresponding images.
        private readonly Dictionary<string, BitmapImage> _matchingPairs = new Dictionary<string, BitmapImage>();
        // The matching task assignment to be completed in this viewer.
        private readonly MatchingTaskAssignment _assignment;

        // An event that signals when the completion state of the assignment changes.
        public event Action<IAssignment, bool> CompletionStateChanged;

        #region Properties, Fields and Constructors
        // Constructor initializes the MatchingViewer with a specific assignment.
        public MatchingViewer(MatchingTaskAssignment assignment)
        {
            _assignment = assignment;
            TaskTitle = _assignment.Title;

            InitializeComponent();
            DataContext = this;

            // Load matching pairs from the assignment into the dictionary.
            foreach (var item in _assignment.Items)
            {
                _matchingPairs.Add(item.Text, ConvertByteArrayToBitmapImage(item.Image));
            }

            // Set up the grid rows based on the number of items to match.
            int numberOfRows = _assignment.Items.Count;
            for (int i = 0; i < numberOfRows; i++)
            {
                gridMatchOptions.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Shuffle the items to randomize their positions in the grid.
            var randomRowIndexesForTexts = Enumerable.Range(0, numberOfRows).OrderBy(i => Guid.NewGuid()).ToArray();
            var randomRowIndexesForImages = Enumerable.Range(0, numberOfRows).OrderBy(i => Guid.NewGuid()).ToArray();

            var gridItems = new List<GridItem>();

            // Create UI elements for each pair and assign them to random rows.
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

            // Add the elements to the grid.
            AddElementsToGrid(gridMatchOptions, gridItems);
        }
        #endregion

        #region Event Handlers
        // Handles button click events to check for correct matches.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Iterate through grid rows, assuming each row contains an image and text block pair.
            for (int row = 0; row < gridMatchOptions.RowDefinitions.Count; row++)
            {
                var imageBorder = FindChildInGrid<Border>(gridMatchOptions, row, 0);
                var textBlockBorder = FindChildInGrid<Border>(gridMatchOptions, row, 1);

                // Skip the iteration if either image or text block is missing.
                if (imageBorder == null || textBlockBorder == null) continue;

                var image = imageBorder.Child as Image;
                var textBlock = textBlockBorder.Child as TextBlock;

                // Show a message and signal incomplete assignment if any pair does not match.
                if (image != null && textBlock != null && image.Name != textBlock.Name)
                {
                    MessageBox.Show(Ru.ElementsDoNotMatch);
                    CompletionStateChanged?.Invoke(_assignment, false);
                    return;
                }
            }

            // If all pairs match, show a success message and signal assignment completion.
            MessageBox.Show(Ru.AllElementsMatch);
            CompletionStateChanged?.Invoke(_assignment, true);
        }

        // Generic method to find a child element of a specific type in a grid cell.
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
            border.Background = Brushes.Transparent;
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
            var border = new Border
            {
                AllowDrop = true,
                Padding = new Thickness(10),
                Child = child
            };

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
                b.Background = new SolidColorBrush(Color.FromArgb(255, 135, 206, 250)); // Light Sky Blue
            };
            border.MouseLeave += (s, e) =>
            {
                var b = s as Border;
                b.Background = Brushes.Transparent;
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


    }
}
