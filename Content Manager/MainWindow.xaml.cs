using Content_Manager.Commands;
using Content_Manager.Models;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Content_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>0
    public partial class MainWindow : Window
    {
        public ICommand DeleteSelectedSegment { get; }

        public Segment? CurrentSegment
        {
            get { return (Segment)GetValue(CurrentSegmentProperty); }
            set { SetValue(CurrentSegmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SegmentId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSegmentProperty =
            DependencyProperty.Register("CurrentSegment", typeof(Segment), typeof(MainWindow), new PropertyMetadata(null));




        private readonly ContentStore _contentStore;
        public ObservableCollection<ISegment> Segments { get; }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize DB
            Storage storage;
            try
            {
                storage = new Storage(false);
            }
            catch
            {
                storage = new Storage(true);
            }

            // Initialize content store
            _contentStore = new ContentStore(new ContentModel(storage));

            // Set existing segments
            Segments = new ObservableCollection<ISegment>();
            foreach (var segment in _contentStore.StoredSegments)
            {
                Segments.Add(segment);
            }

            // Set events
            _contentStore.ItemAdded += OnSegmentAdded;
            _contentStore.ItemDeleted += OnSegmentDeleted;
            _contentStore.ItemUpdated += OnSegmentUpdated;

            // Initialize commands
            DeleteSelectedSegment = new SegmentCommands.DeleteSegment(_contentStore, this);

            // Bind data
            DataContext = this;

            tbcMain.Visibility = Visibility.Hidden;
        }

        private void OnSegmentAdded(IModelBase model)
        {
            if (model.GetType().IsAssignableTo(typeof(ISegment)) == false) return;

            var segment = model as ISegment;
            Segments.Add(segment);
            CurrentSegment = (Segment)segment;
            tbcMain.Visibility = Visibility.Visible;

        }

        private void OnSegmentUpdated(IModelBase model)
        {
            if (model.GetType().IsAssignableTo(typeof(ISegment)) == false) return;

            var segment = model as ISegment;
            var index = Segments.ToList().FindIndex(s => s.Id == segment!.Id);

            lvSegments.Items.Refresh();
        }

        private void OnSegmentDeleted(IModelBase model)
        {
            if (model.GetType().IsAssignableTo(typeof(ISegment)) == false) return;

            var segment = model as ISegment;
            var index = Segments.ToList().FindIndex(s => s.Id == segment!.Id);
            Segments.RemoveAt(index);
            
            if (CurrentSegment == segment)
            {
                CurrentSegment = null;
                tbcMain.Visibility = Visibility.Hidden;
            }
        }

        private void BtnNewSection_Click(object sender, RoutedEventArgs e)
        {
            ISegment segment = new Segment() { Title = "Керла дакъа" };

            _contentStore.AddItem<ISegment>(segment);
        }

        private void lvSegments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var segment = ((Segment)lvSegments.SelectedItem);
            if (segment == null) return;

            CurrentSegment = segment;
            tbcMain.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _contentStore.UpdateItem<ISegment>(CurrentSegment);
        }
    }
}
