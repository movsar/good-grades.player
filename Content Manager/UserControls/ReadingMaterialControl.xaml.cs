using Content_Manager.Stores;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Content_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for ReadingMaterialControl.xaml
    /// </summary>
    public partial class ReadingMaterialControl : UserControl
    {
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public ReadingMaterial Material { get; set; }
        private void Init()
        {
            InitializeComponent();
            DataContext = this;
        }
        public ReadingMaterialControl()
        {
            Init();
            Material = new ReadingMaterial()
            {
                Title = "Введите название материала"
            };
        }
        public ReadingMaterialControl(ReadingMaterial material)
        {
            Init();
            Material = material;
            txtTitle.IsEnabled = false;
        }

        private void txtTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Material.Title == "Введите название материала")
            {
                Material.Title = "";
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Material.Title == "")
            {
                Material.Title = "Введите название материала";
            }
        }
    }
}
