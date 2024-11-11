using Data.Entities;
using Shared.Controls;
using System.Windows.Controls;

namespace GGPlayer.Pages
{
    public partial class MaterialViewerPage : Page
    {
        public MaterialViewerPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void LoadMaterial(Material material)
        {
            Title = material.Title;

            ucRoot.Content = new MaterialViewerControl(material.Title, material.PdfData, material.Audio);
            InitializeComponent();
        }
    }
}
