using Data.Entities;
using Data.Entities.TaskItems;
using Shared.Controls;
using Shared.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Viewers
{
    public partial class BuildingViewer : Window
    {
        IList<AssignmentItem> _items;
        public string TaskTitle { get; }
        public BuildingViewer(BuildingTaskAssignment taskAssignment)
        {
            InitializeComponent();
            DataContext = this;

            TaskTitle = taskAssignment.Title;
            _items = taskAssignment.Items; _items = taskAssignment.Items;

            foreach (var item in _items)
            {
                var buildingItemViewControl = new BuildingItemViewControl(item.Text);
                spItems.Children.Add(buildingItemViewControl);
            }
        }
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            foreach (BuildingItemViewControl buildingItemViewControl in spItems.Children)
            {
                // Check if the user input matches one of the options
                if (!buildingItemViewControl.IsCorrectlyArranged)
                {
                    MessageBox.Show(Ru.Incorrect);
                    return;
                }
            }

            // Show a message if all inputs are correct
            MessageBox.Show(Ru.Correct);
        }
    }
}
