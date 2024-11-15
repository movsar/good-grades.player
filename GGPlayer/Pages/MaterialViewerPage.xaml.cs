using Data.Entities;
using Microsoft.Extensions.DependencyInjection;
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

        public void Initialize(Material material)
        {
            Title = material.Title;

            var materialControl = App.AppHost!.Services.GetRequiredService<MaterialViewerControl>();
            materialControl.Initialize(material.Title, material.PdfData, material.Audio);

            ucRoot.Content = materialControl;
            InitializeComponent();
        }
    }
}
