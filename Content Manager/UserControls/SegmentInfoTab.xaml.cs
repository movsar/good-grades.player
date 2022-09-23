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

        public static readonly DependencyProperty SelectedSegmentProperty = DependencyProperty.Register("SelectedSegmentId",
            typeof(string),
            typeof(SegmentInfoTab),
            new PropertyMetadata(string.Empty, OnSelectedSegmentIdChanged));
        
        public string SelectedSegmentId {
            get => (string)GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }

        private static void OnSelectedSegmentIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue= e.NewValue as string;
        }
    }
}
