using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class BuildingAssignmentControl : UserControl, IAssignmentViewer
    {
        public event Action<IAssignment, bool> CompletionStateChanged;

        private readonly BuildingAssignment _assignment;
        private int _currentItemIndex = 0;

        public string TaskTitle { get; }

        public BuildingAssignmentControl(BuildingAssignment assignment)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = assignment;
            TaskTitle = _assignment.Title;

            // Загрузка первого выражения
            LoadCurrentItem();
        }

        // Метод для загрузки текущего элемента
        private void LoadCurrentItem()
        {
            spItems.Children.Clear();

            var item = _assignment.Items[_currentItemIndex];
            var buildingItemViewControl = new BuildingItemViewControl(item) { Tag = item.Text };
            spItems.Children.Add(buildingItemViewControl);

            _currentItemIndex++;
        }
        public bool Check()
        {
            if (spItems.Children.Count == 0)
            {
                CompletionStateChanged?.Invoke(_assignment, true);
                return true;
            }

            var buildingItemViewControl = (BuildingItemViewControl)spItems.Children[0];
            var arrangedPhrase = GetUserArrangedPhrase(buildingItemViewControl);

            // Проверка правильности
            if (arrangedPhrase != buildingItemViewControl.Tag.ToString())
            {
                CompletionStateChanged?.Invoke(_assignment, false);
                return false;
            }

            // Проверка на завершение всех элементов
            if (_currentItemIndex != _assignment.Items.Count)
            {
                CompletionStateChanged?.Invoke(_assignment, false);
                return false;
            }

            LoadCurrentItem();
            CompletionStateChanged?.Invoke(_assignment, true);
            return true;
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
