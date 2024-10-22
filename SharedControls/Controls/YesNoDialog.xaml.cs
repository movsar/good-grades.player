using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shared.Controls
{
    public partial class YesNoDialog : UserControl
    {
        public MessageBoxResult DialogResult { get; private set; }

        public YesNoDialog()
        {
            InitializeComponent();
        }
        
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = MessageBoxResult.Yes;
            CloseDialog();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = MessageBoxResult.No;
            CloseDialog();
        }

        private void CloseDialog()
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.DialogResult = true;
                parentWindow.Close();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.DragMove();
                }
            }
        }

        public static MessageBoxResult Show(string message, string title = "Good Grades")
        {
            // Create a new window to host the dialog
            var dialogWindow = new Window
            {
                Title = title,
                Content = new YesNoDialog
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                },
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                Background = System.Windows.Media.Brushes.Transparent
            };

            var dialog = (YesNoDialog)dialogWindow.Content;

            // Customize the dialog
            dialog.DialogMessage.Text = message;
            dialog.DialogHeader.Text = title;

            // Show the dialog and wait for user interaction
            bool? dialogResult = dialogWindow.ShowDialog();
            return dialog.DialogResult;
        }
    }
}