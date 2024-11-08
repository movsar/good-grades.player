using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class BuildingAssignmentControl : UserControl, IAssignmentViewer
    {

        private readonly BuildingAssignment _assignment;
        private int _currentItemIndex = -1;

        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;

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
            var item = _assignment.Items[++_currentItemIndex];
            var buildingItemViewControl = new BuildingItemViewControl(item) { Tag = item.Text };

            spItems.Children.Clear();
            spItems.Children.Add(buildingItemViewControl);
        }
        public void OnCheckClicked()
        {
            if (spItems.Children.Count == 0)
            {
                return;
            }

            // Get submission information
            var buildingItemViewControl = (BuildingItemViewControl)spItems.Children[0];
            var arrangedPhrase = GetUserArrangedPhrase(buildingItemViewControl);

            // Check whether the submission was correct
            var isItemSubmissionCorrect = (arrangedPhrase == buildingItemViewControl.Tag.ToString());
            AssignmentItemSubmitted?.Invoke(_assignment, _assignment.Items[_currentItemIndex].Id, isItemSubmissionCorrect);

            if (isItemSubmissionCorrect && _currentItemIndex == _assignment.Items.Count - 1)
            {
                AssignmentCompleted?.Invoke(_assignment, true);
            }

            // Block UI until the Next is clicked
            IsEnabled = false;
        }

        private string GetUserArrangedPhrase(BuildingItemViewControl buildingItemViewControl)
        {
            var words = new List<string>();
            foreach (var child in buildingItemViewControl.spItemDropZone.Children)
            {
                // Проверяем, является ли элемент кнопкой
                if (child is Button btnWord)
                {
                    words.Add(btnWord.Content.ToString());
                }
                // Если это Grid, например, то извлекаем текст из TextBlock внутри Grid
                else if (child is Grid gridWord)
                {
                    var textBlock = gridWord.Children.OfType<TextBlock>().FirstOrDefault();
                    if (textBlock != null)
                    {
                        words.Add(textBlock.Text);
                    }
                }
            }
            return string.Join(" ", words);
        }

        public void OnRetryClicked()
        {
            IsEnabled = true;
        }

        public void OnNextClicked()
        {
            IsEnabled = true;

            LoadNextItem();
        }

    }
}