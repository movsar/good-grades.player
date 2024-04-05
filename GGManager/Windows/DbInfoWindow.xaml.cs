using System.Windows;

namespace GGManager.Windows
{
    public partial class DbInfoWindow : Window
    {
        public DbInfoWindow()
        {
            InitializeComponent();
            ucDbInformation.Saved += DbInformation_Saved;
        }

        private void DbInformation_Saved()
        {
            Close();
        }
    }
}
