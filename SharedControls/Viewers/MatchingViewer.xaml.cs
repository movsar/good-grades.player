using Data.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        private void Element_MouseMove(object sender, MouseEventArgs e){}

        public MatchingViewer(MatchingTaskAssignment assignment)
        {
            InitializeComponent();

            List<GridItem> items = new List<GridItem>
            {
                new GridItem { ImageSource = "/Shared;component/Viewers/Picture1.jpg", Row = 0, Column = 0 },
                new GridItem { ImageSource = "/Shared;component/Viewers/Picture2.jpg", Row = 1, Column = 0 },
                new GridItem { ImageSource = "/Shared;component/Viewers/Бабин ЧIирдиг.png", Row = 2, Column = 0 },
                new GridItem { ImageSource = "/Shared;component/Viewers/StoryPicture.png" , Row = 3, Column = 0 },


                new GridItem { Text = "Дерево", Row = 0, Column = 1 },
                new GridItem { Text = "Человек", Row = 1, Column = 1 },
                new GridItem { Text = "Бабушка", Row = 2, Column = 1 },
                new GridItem { Text = "Стамбул", Row = 3, Column = 1 },
                // Add other items here
            };

            AddElementsToGrid(gridMatchOptions, items);
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

                Grid.SetRow(border, item.Row);
                Grid.SetColumn(border, item.Column);

                if (!string.IsNullOrEmpty(item.ImageSource))
                {
                    Image image = new Image
                    {
                        Source = new BitmapImage(new Uri(item.ImageSource, UriKind.Relative))
                    };
                    border.Child = image;
                }
                else if (!string.IsNullOrEmpty(item.Text))
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Text = item.Text
                    };
                    border.Child = textBlock;
                }

                grid.Children.Add(border);
            }
        }

        public class GridItem
        {
            public string ImageSource { get; set; }
            public string Text { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
        }

    }
}