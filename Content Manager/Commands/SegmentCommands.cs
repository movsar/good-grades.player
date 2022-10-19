using Content_Manager.Stores;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Content_Manager.Commands
{
    internal class SegmentCommands
    {
        private static void DeleteAction(ContentStore contentStore, MainWindow view)
        {
            var result = MessageBox.Show($"Please approva removal of {view.lvSegments.SelectedItems.Count} dream(s)",
                                             "Confirmation",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var selectedSegments = view.lvSegments.SelectedItems.Cast<Segment>();

                var segmentsToRemove = contentStore.StoredSegments.Where(segment => selectedSegments.Select(s => s.Id).Contains(segment.Id));

                contentStore.DeleteSegments(segmentsToRemove);
            }
        }
        internal class DeleteSegment : CommandBase
        {
            private readonly ContentStore _contentStore;
            private readonly MainWindow _view;

            public DeleteSegment(ContentStore contentStore, MainWindow view)
            {
                _contentStore = contentStore;
                _view = view;
            }

            public override void Execute(object? parameter)
            {
                DeleteAction(_contentStore, _view);
            }
        }
    }
}
