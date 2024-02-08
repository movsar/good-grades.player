using Content_Player.Models;
using Data.Entities;
using Data.Interfaces;
using Shared.Services;
using System.Windows;
using System.Windows.Input;

namespace Content_Player.Windows
{
    public partial class SegmentWindow : Window
    {
        public string SegmentTitle { get; }
        public List<IMaterial> Materials { get; } = new List<IMaterial>();
        public SegmentWindow(Segment segment)
        {
            InitializeComponent();
            DataContext = this;

            RtfService.LoadRtfFromText(rtbDescription, segment.Description);
            Title = segment.Title;

            Materials.AddRange(segment.ListeningMaterials.Cast<IMaterial>());
            Materials.AddRange(segment.ReadingMaterials.Cast<IMaterial>());
            Materials.Add(new FakeSegmentMaterial()
            {
                Id = "tasks",
                Title = "Хаарш зер"
            });

        }

        private void OnListViewItemSelected()
        {
            if (lvMaterials.SelectedItem == null)
            {
                return;
            }

            switch ((IMaterial)lvMaterials.SelectedItem)
            {
                case ReadingMaterial:
                    break;
                case ListeningMaterial:
                    break;
                case FakeSegmentMaterial:
                    break;
            }
        }

        private void lvMaterialsItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnListViewItemSelected();
        }

        private void lvMaterials_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                OnListViewItemSelected();
            }
        }
    }
}
