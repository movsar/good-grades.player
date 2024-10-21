using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shared.Controls
{
    public partial class YesNoDialog : UserControl
    {
        public enum YesNoResult
        {
            Yes,
            No
        }

        public YesNoResult DialogResult { get; private set; }

        public YesNoDialog()
        {
            InitializeComponent();
        }

        public void SetMessage(string message)
        {
            DialogMessage.Text = message;
        }

        // Yes button click event handler
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = YesNoResult.Yes;
            CloseDialog();
        }

        // No button click event handler
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = YesNoResult.No;
            CloseDialog();
        }

        // Close the dialog by closing its parent window
        private void CloseDialog()
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.DialogResult = true;
                parentWindow.Close();
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
    }
}