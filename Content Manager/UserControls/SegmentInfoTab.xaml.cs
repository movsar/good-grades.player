using Data.Models;
using System;
using System.Diagnostics.Tracing;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for SegmentInfoTab.xaml
    /// </summary>
    public partial class SegmentInfoTab : UserControl
    {
        public SegmentInfoTab()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty SelectedSegmentProperty = DependencyProperty.Register("SelectedSegment",
            typeof(Segment),
            typeof(SegmentInfoTab),
            new PropertyMetadata(null));
        
        public Segment SelectedSegment {
            get => (Segment)GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }
    }
}
