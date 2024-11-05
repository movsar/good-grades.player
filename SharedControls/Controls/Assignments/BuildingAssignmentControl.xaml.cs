using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class BuildingAssignmentControl : UserControl, IAssignmentViewer
    {
        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemCompleted;

        private readonly BuildingAssignment _assignment;
        private int _currentItemIndex = -1;

        public string TaskTitle { get; }

        public BuildingAssignmentControl(BuildingAssignment assignment)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = assignment;
            TaskTitle = _assignment.Title;

            LoadNextItem();
        }

        // Метод для загрузки текущего элемента
        private void LoadNextItem()
        {
            this.IsEnabled = true;
          
            var item = _assignment.Items[++_currentItemIndex];
            var buildingItemViewControl = new BuildingItemViewControl(item) { Tag = item.Text };
            
            spItems.Children.Clear();
            spItems.Children.Add(buildingItemViewControl);
        }
        public void OnCheckClicked()
        {
            this.IsEnabled = false;

            if (spItems.Children.Count == 0)
            {
                return;
            }

            var buildingItemViewControl = (BuildingItemViewControl)spItems.Children[0];
            var arrangedPhrase = GetUserArrangedPhrase(buildingItemViewControl);
            if (arrangedPhrase != buildingItemViewControl.Tag.ToString())
            {
                AssignmentItemCompleted?.Invoke(_assignment, _assignment.Items[_currentItemIndex].Id, false);
                return;
            }

            // Проверка на завершение всех элементов
            if (_currentItemIndex == _assignment.Items.Count)
            {
                AssignmentCompleted?.Invoke(_assignment, true);
            }
            else
            {
                AssignmentItemCompleted?.Invoke(_assignment, _assignment.Items[_currentItemIndex].Id, true);
                LoadNextItem();
            }
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

        public void OnRetryClicked()
        {
            this.IsEnabled = true;
        }
    }
}
