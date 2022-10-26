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
    public partial class ListeningTab : UserControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ListeningTab()
        {
            InitializeComponent();
            DataContext = this;

            spListeningMaterialControls.Children.Clear();

            if (_contentStore.SelectedSegment!.ListeningMaterials == null) return;

            foreach (var material in _contentStore.SelectedSegment!.ListeningMaterials)
            {
                spListeningMaterialControls.Children.Add(new ListeningMaterialControl(material));
            }

            spListeningMaterialControls.Children.Add(new ListeningMaterialControl());
        }
    }
}
