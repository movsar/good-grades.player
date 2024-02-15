using Content_Player.Pages;
using System.Windows;

namespace Content_Player
{
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage());
        }
    }
}