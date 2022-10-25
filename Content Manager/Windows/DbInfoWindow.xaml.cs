using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
