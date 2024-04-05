using Content_Manager.Commands;
using Content_Manager.Stores;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Shared.Translations;
using Microsoft.EntityFrameworkCore;

namespace Content_Manager.UserControls
{
    public partial class SegmentListControl : UserControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ICommand DeleteSelectedSegment { get; }
        public SegmentListControl()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize commands
            DeleteSelectedSegment = new SegmentCommands.DeleteSegment(_contentStore, this);

            // Set events
            _contentStore.ItemDeleted += OnItemDeleted;
            _contentStore.ItemUpdated += OnItemUpdated;
            _contentStore.CurrentDatabaseChanged += _contentStore_CurrentDatabaseChanged;
        }

        private void _contentStore_CurrentDatabaseChanged()
        {
            RedrawSegmentList();
        }

        private void RedrawSegmentList(string? selectedSegmentId = null)
        {
            lvSegments.Items.Clear();
            foreach (var segment in _contentStore.DbContext.Segments)
            {
                lvSegments.Items.Add(segment);
            }

            if (selectedSegmentId == null)
            {
                _contentStore.SelectedSegment = null;
                return;
            }

            var currentSegment = _contentStore.DbContext.Segments.Where(item => item.Id == selectedSegmentId).First();
            lvSegments.SelectedItem = currentSegment;
        }

        private void BtnNewSection_Click(object sender, RoutedEventArgs e)
        {
            Segment segment = new Segment() { Title = Ru.NewChapter };
            _contentStore.DbContext.Add(segment);
            _contentStore.DbContext.SaveChanges();

            RedrawSegmentList();
            _contentStore.SelectedSegment = segment;
        }

        private void lvSegments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var segment = ((Segment)lvSegments.SelectedItem);
            if (segment == null) return;

            if (_contentStore.SelectedSegment?.Id == segment.Id)
            {
                return;
            }

            _contentStore.SelectedSegment = segment;
        }

        #region Segment Event Handlers

        private void OnItemUpdated(IEntityBase entity)
        {
            if (entity is not Segment)
            {
                return;
            }

            RedrawSegmentList(entity.Id);
        }

        private void OnItemDeleted(IEntityBase entity)
        {
            if (entity is not Segment)
            {
                return;
            }

            RedrawSegmentList();
        }
        #endregion

    }
}
