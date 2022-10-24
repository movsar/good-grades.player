using Content_Manager.Commands;
using Content_Manager.Stores;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Content_Manager.UserControls
{
    public partial class SegmentList : UserControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ICommand DeleteSelectedSegment { get; }
        public SegmentList()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize commands
            DeleteSelectedSegment = new SegmentCommands.DeleteSegment(_contentStore, this);

            // Set events
            _contentStore.ItemAdded += OnItemAdded;
            _contentStore.ItemDeleted += OnItemDeleted;
            _contentStore.ItemUpdated += OnItemUpdated;
            _contentStore.ContentStoreInitialized += ContentStoreInitialized;
        }
        private void RedrawSegmentList(string? selectedSegmentId = null)
        {
            lvSegments.Items.Clear();
            foreach (var segment in _contentStore.StoredSegments)
            {
                lvSegments.Items.Add(segment);
            }

            if (selectedSegmentId == null)
            {
                return;
            }

            var currentSegment = _contentStore.StoredSegments.Where(item => item.Id == selectedSegmentId).First();
            lvSegments.SelectedItem = currentSegment;
        }
        private void ContentStoreInitialized()
        {
            RedrawSegmentList();
        }
        private void BtnNewSection_Click(object sender, RoutedEventArgs e)
        {
            ISegment segment = new Segment() { Title = "Керла дакъа" };

            _contentStore.AddSegment(segment);
        }

        private void lvSegments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var segment = ((Segment)lvSegments.SelectedItem);
            if (segment == null) return;

            _contentStore.SelectedSegment = segment;
        }

        #region Segment Event Handlers
        private void OnItemAdded(string modelType, IModelBase model)
        {
            if (!modelType.Equals(nameof(ISegment)))
            {
                return;
            }

            RedrawSegmentList();
        }

        private void OnItemUpdated(string modelType, IModelBase model)
        {
            if (!modelType.Equals(nameof(ISegment)))
            {
                return;
            }

            RedrawSegmentList(model.Id);
        }

        private void OnItemDeleted(string modelType, IModelBase model)
        {
            if (!modelType.Equals(nameof(ISegment)))
            {
                return;
            }

            RedrawSegmentList();
        }
        #endregion

    }
}
