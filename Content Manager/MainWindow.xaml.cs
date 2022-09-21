﻿using Content_Manager.Commands;
using Content_Manager.Models;
using Content_Manager.Stores;
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

namespace Content_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>0
    public partial class MainWindow : Window
    {
        public ICommand DeleteSelectedSegment { get; }

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
            _contentStore.ItemUpdated+= OnSegmentUpdated;

            // Initialize commands
            DeleteSelectedSegment = new SegmentCommands.DeleteSegment(_contentStore, this);

            // Bind data
            DataContext = this;
        }

        private void OnSegmentAdded(IModelBase model)
        {
            if (model.GetType().IsAssignableTo(typeof(ISegment)) == false) return;

            var segment = model as ISegment;
            Segments.Add(segment);
        }

        private void OnSegmentUpdated(IModelBase model)
        {
            if (model.GetType().IsAssignableTo(typeof(ISegment)) == false) return;

            var segment = model as ISegment;
            var index = Segments.ToList().FindIndex(s => s.Id == segment!.Id);
            Segments[index] = segment;
        }

        private void OnSegmentDeleted(IModelBase model)
        {
            if (model.GetType().IsAssignableTo(typeof(ISegment)) == false) return;

            var segment = model as ISegment;
            var index = Segments.ToList().FindIndex(s => s.Id == segment!.Id);
            Segments.RemoveAt(index);
        }

        private void BtnNewSection_Click(object sender, RoutedEventArgs e)
        {
            ISegment segment = new Segment() { Title = "Керла дакъа" };

            _contentStore.AddItem<ISegment>(segment);
        }

    }
}
