using Content_Manager.Commands;
using Content_Manager.Stores;
using Data.Interfaces;
using Data.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Content_Manager {
    public partial class MainWindow : Window {
        public ICommand DeleteSelectedSegment { get; }

        private readonly ContentStore _contentStore;
        public ObservableCollection<ISegment> Segments { get; }

        public Segment CurrentSegment {
            get { return (Segment)GetValue(CurrentSegmentProperty); }
            set { SetValue(CurrentSegmentProperty, value); _contentStore.SelectedSegment = value; }
        }

        public static readonly DependencyProperty CurrentSegmentProperty =
            DependencyProperty.Register("CurrentSegment", typeof(Segment), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow(ContentStore contentStore) {
            InitializeComponent();

            // Initialize content store
            _contentStore = contentStore;

            // Set existing segments
            Segments = new ObservableCollection<ISegment>();
            foreach (var segment in _contentStore.StoredSegments) {
                Segments.Add(segment);
            }

            // Set events
            _contentStore.ItemAdded += OnItemAdded;
            _contentStore.ItemDeleted += OnItemDeleted;
            _contentStore.ItemUpdated += OnItemUpdated;

            // Initialize commands
            DeleteSelectedSegment = new SegmentCommands.DeleteSegment(_contentStore, this);

            // Bind data
            DataContext = this;

            tbcMain.Visibility = Visibility.Hidden;
        }
    
        private void OnSegmentAdded(Segment? segment) {
            Segments.Add(segment!);
            CurrentSegment = segment;
            tbcMain.Visibility = Visibility.Visible;
        }
        private void OnSegmentUpdated(Segment? segment) {
            var index = Segments.ToList().FindIndex(s => s.Id == segment!.Id);

            // Refresh UI
            lvSegments.Items.Refresh();

            // Force Segment's refresh
            if (_contentStore.SelectedSegment!.Id == segment!.Id) {
                _contentStore.SelectedSegment = segment;
            }
        }

        private void OnSegmentDeleted(Segment? segment) {
            var index = Segments.ToList().FindIndex(s => s.Id == segment!.Id);
            Segments.RemoveAt(index);

            if (CurrentSegment == segment) {
                CurrentSegment = null;
                tbcMain.Visibility = Visibility.Hidden;
            }
        }

        private void BtnNewSection_Click(object sender, RoutedEventArgs e) {
            ISegment segment = new Segment() { Title = "Керла дакъа" };

            _contentStore.AddSegment(segment);
        }

        private void lvSegments_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var segment = ((Segment)lvSegments.SelectedItem);
            if (segment == null) return;

            CurrentSegment = segment;
            tbcMain.Visibility = Visibility.Visible;
        }
        private void OnItemAdded(string modelType, IModelBase model) {
            if (modelType.Equals(nameof(ISegment))) {
                OnSegmentAdded(model as Segment);
            }
        }

        private void OnItemUpdated(string modelType, IModelBase model) {
            if (modelType.Equals(nameof(ISegment))) {
                OnSegmentUpdated(model as Segment);
            }
        }

        private void OnItemDeleted(string modelType, IModelBase model) {
            if (modelType.Equals(nameof(ISegment))) {
                OnSegmentDeleted(model as Segment);
            }
        }
    }
}
