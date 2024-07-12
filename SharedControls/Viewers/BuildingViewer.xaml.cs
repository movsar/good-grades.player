using Data.Entities;
using Data.Interfaces;
using Shared.Controls;
using Shared.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Viewers
{
    public partial class BuildingViewer : Window, IAssignmentViewer
    {
        public event Action<IAssignment, bool> CompletionStateChanged;

        private readonly BuildingAssignment _assignment;

        public string TaskTitle { get; }
        public BuildingViewer(BuildingAssignment assignment)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = assignment;

            TaskTitle = _assignment.Title;

            foreach (var item in _assignment.Items)
            {
                var buildingItemViewControl = new BuildingItemViewControl(item) { Tag = item.Text };
                spItems.Children.Add(buildingItemViewControl);
            }
        }
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            foreach (BuildingItemViewControl buildingItemViewControl in spItems.Children)
            {
                var arrangedPhrase = GetUserArrangedPhrase(buildingItemViewControl);

                // Check if the user input matches one of the options
                if (arrangedPhrase != buildingItemViewControl.Tag.ToString())
                {
                    CompletionStateChanged?.Invoke(_assignment, false);
                    MessageBox.Show(Translations.GetValue("Incorrect"));
                    return;
                }
            }

            // Show a message if all inputs are correct
            CompletionStateChanged?.Invoke(_assignment, true);
            MessageBox.Show(Translations.GetValue("Correct"));
        }

        private string GetUserArrangedPhrase(BuildingItemViewControl buildingItemViewControl)
        {
            var words = new List<string>();
            foreach (Button btnWord in buildingItemViewControl.spItemDropZone.Children)
            {
                words.Add(btnWord.Content.ToString());
            }

            return string.Join(" ", words);
        }
    }
}
