using Data.Entities;
using Data.Entities.TaskItems;
using Shared.Controls;
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
        public BuildingViewer(BuildingTaskAssignment taskAssignment)
        {
            InitializeComponent();
            DataContext = this;

            _items = taskAssignment.Items; _items = taskAssignment.Items;

            foreach (var item in _items)
            {
                var buildingItemViewControl = new BuildingItemViewControl(item.Text);
                spItems.Children.Add(buildingItemViewControl);
            }
        }
    }
}
