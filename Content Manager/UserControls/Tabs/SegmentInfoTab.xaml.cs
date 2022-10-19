using Content_Manager.Stores;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.Tracing;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls {
    /// <summary>
    /// Interaction logic for SegmentInfoTab.xaml
    /// </summary>
    public partial class SegmentInfoTab : UserControl {
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public SegmentInfoTab() {
            InitializeComponent();
            DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            ContentStore.UpdateSegment(ContentStore.SelectedSegment!);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
    }
}
