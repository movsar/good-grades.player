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
using System.IO;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Content_Manager.Stores;

namespace Content_Manager.UserControls {
    public partial class ReadingTab : UserControl {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ReadingTab() {
            InitializeComponent();
            DataContext = this;

            spReadingMaterialControls.Children.Clear();

            if (_contentStore.SelectedSegment!.ReadingMaterials == null) return;

            foreach (var material in _contentStore.SelectedSegment!.ReadingMaterials)
            {
                spReadingMaterialControls.Children.Add(new ReadingMaterialControl(material));
            }

            spReadingMaterialControls.Children.Add(new ReadingMaterialControl());
        }

    }
}
