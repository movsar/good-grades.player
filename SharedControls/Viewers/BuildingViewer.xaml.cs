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
        private int _currentItemIndex = 0;

        public string TaskTitle { get; }

        public BuildingViewer(BuildingAssignment assignment)
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

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (spItems.Children.Count == 0)
            {
                return;
            }

            var buildingItemViewControl = (BuildingItemViewControl)spItems.Children[0];
            var arrangedPhrase = GetUserArrangedPhrase(buildingItemViewControl);

            // Проверка правильности
            if (arrangedPhrase != buildingItemViewControl.Tag.ToString())
            {
                CompletionStateChanged?.Invoke(_assignment, false);
                MessageBox.Show(Translations.GetValue("Incorrect"));
                return;
            }

            // Проверка на завершение всех элементов
            if (_currentItemIndex == _assignment.Items.Count)
            {
                MessageBox.Show(Translations.GetValue("AllAnswersAreCorrect"));
                CompletionStateChanged?.Invoke(_assignment, true);
                this.Close();
                return;
            }

            // Если все верно, показать уведомление и переход к следующему выражению
            MessageBox.Show(Translations.GetValue("Correct"));
            LoadCurrentItem();
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
