using Data.Entities;
using Data.Interfaces;
using System.Windows;

namespace Content_Player.Windows
{
    public partial class SegmentWindow : Window
    {
        public string SegmentTitle { get; }
        public string Description { get; }
        public List<IMaterial> Materials { get; } = new List<IMaterial>();
        public SegmentWindow(Segment segment)
        {
            InitializeComponent();
            DataContext = this;

            Description = segment.Description;
            Title = segment.Title;

            Materials.AddRange(segment.ListeningMaterials.Cast<IMaterial>());
            Materials.AddRange(segment.ReadingMaterials.Cast<IMaterial>());
        }
    }
}
