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
        private ContentStore _contentStore { get; }
        public ListeningTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
        }

        private void _contentStore_SegmentChanged(Segment selectedSegment)
        {
            spListeningMaterialControls.Children.Clear();

            if (selectedSegment == null) return;

            foreach (var material in selectedSegment.ListeningMaterials)
            {
                spListeningMaterialControls.Children.Add(new ListeningMaterialControl(material));
            }

            spListeningMaterialControls.Children.Add(new ListeningMaterialControl());
        }
    }
}
