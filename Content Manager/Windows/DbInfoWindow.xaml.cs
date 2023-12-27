using System.Windows;

namespace Content_Manager.Windows
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
