using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data.Entities;
using System;
using System.Linq;
using System.Windows;

namespace Content_Manager.Commands
{
    internal class SegmentCommands
    {
        private static void DeleteAction(ContentStore contentStore, SegmentListControl view)
        {
            Segment? selectedSegment = view.lvSegments.SelectedItems.Cast<Segment>().FirstOrDefault();
            if (selectedSegment == null)
            {
                return;
            }

            var result = MessageBox.Show($"Подтвердите удаление раздела {selectedSegment.Title}\"",
                                             "Good Grades",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                contentStore.Database.Write(() => contentStore.Database.Remove(selectedSegment));
                contentStore.RaiseItemDeletedEvent(selectedSegment);
            }
        }
        internal class DeleteSegment : CommandBase
        {
            private readonly ContentStore _contentStore;
            private readonly SegmentListControl _view;

            public DeleteSegment(ContentStore contentStore, SegmentListControl view)
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
