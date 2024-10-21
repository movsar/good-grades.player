using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shared.Controls
{
    public partial class OkDialog : UserControl
    {
        public OkDialog()
        {
            InitializeComponent();
        }

        public void SetMessage(string message)
        {
            DialogMessage.Text = message;
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }
    }
}