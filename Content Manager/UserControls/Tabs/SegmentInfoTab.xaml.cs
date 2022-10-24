using Content_Manager.Stores;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.Tracing;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls
{
    public partial class SegmentInfoTab : UserControl
    {
        #region Properties
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SegmentInfoTab), new PropertyMetadata(string.Empty));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(SegmentInfoTab), new PropertyMetadata(string.Empty));

        #endregion

        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();

        public SegmentInfoTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore.SelectedSegmentChanged += SelectedSegmentChanged;
        }

        private void SelectedSegmentChanged(Segment segment)
        {
            Title = segment.Title;
            Description = segment.Description;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _contentStore.SelectedSegment!.Title = Title;
            _contentStore.SelectedSegment!.Description = Description;

            _contentStore.UpdateSegment(_contentStore.SelectedSegment!);
        }
    }
}
