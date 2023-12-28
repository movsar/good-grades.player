using Content_Manager.Stores;
using Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls.SegmentTabs
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

            Title = _contentStore.SelectedSegment!.Title;
            Description = _contentStore.SelectedSegment!.Description;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _contentStore.Database.Write(() =>
            {
                _contentStore.SelectedSegment!.Title = Title;
                _contentStore.SelectedSegment!.Description = Description;
            });
        }
    }
}
