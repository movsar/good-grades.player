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
    }
}
