using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Data.Interfaces;
using GGPlayer.Pages.Assignments;

namespace GGPlayer.Pages
{
    public partial class AssignmentsPage : Page
    {
        private List<int> completedAssignments = new List<int>();
        private List<IAssignment> Assignments { get; } = new List<IAssignment>();
        const int MaxAssignments = 30;

        private readonly ShellWindow _shell;

        public AssignmentsPage(ShellWindow shell, List<IAssignment> assignments)
        {
            InitializeComponent();
            // Ограничиваем список заданий до 30
            Assignments.AddRange(assignments.Take(MaxAssignments));
            GenerateAssignmentButtons();

            _shell = shell;
        }

        private void GenerateAssignmentButtons()
        {
            // Очистка предыдущего содержимого
            wrapPanel.Children.Clear();

            int count = 1;
            foreach (var assignment in Assignments)
            {
                var label = new Label()
                {
                    Content = count,
                    Style = (Style)FindResource("AssignmentButtonStyle"),
                    Tag = assignment.Id
                };

                // Устанавливаем событие клика на изображение
                label.MouseDown += AssignmentButton_Click;

                // Помещаем Grid в WrapPanel
                wrapPanel.Children.Add(label);
                count++;
            }
        }

        // Метод для изменения цвета границы
        private void ChangeBorderColor(IAssignment assignment, bool isCompleted)
        {
            var button = FindAssignmentButton(assignment);
            if (button == null)
            {
                return;
            }

            var effect = new DropShadowEffect();
            if (isCompleted)
            {
                effect.Color = Colors.LimeGreen;
                effect.BlurRadius = 30;
                effect.ShadowDepth = 5;
            }
            else
            {
                effect.Color = Colors.Transparent;
                effect.BlurRadius = 0;
                effect.ShadowDepth = 0;
            }

            button.Effect = effect;
        }

        // Метод для поиска изображения задания
        private Label? FindAssignmentButton(IAssignment assignment)
        {
            foreach (var child in wrapPanel.Children)
            {
                var button = child as Label;
                if (button != null && button.Tag != null && button.Tag.Equals(assignment.Id))
                {
                    return button;
                }
            }
            return null;
        }

        private void AssignmentButton_Click(object sender, MouseButtonEventArgs e)
        {
            var clickedButton = (Label)sender;
            var taskIndex = int.Parse(clickedButton.Content.ToString()!) - 1;
            var assignment = Assignments[taskIndex];

            NavigateToAssignment(assignment);
        }

        private void NavigateToAssignment(IAssignment assignment)
        {
            var viewerPage = new AssignmentViewerPage(_shell, assignment);
            viewerPage.AssignmentCompleted += OnAssignmentCompleted;
            _shell.CurrentFrame.Navigate(viewerPage);
        }

        private void OnAssignmentCompleted(IAssignment assignment, bool success)
        {
            // Находим индекс задания в списке
            var assignmentIndex = Assignments.IndexOf(assignment);
            if (assignmentIndex == -1)
            {
                return;
            }

            // Добавляем задание в список выполненных, если оно еще не добавлено
            if (!completedAssignments.Contains(assignmentIndex))
            {
                completedAssignments.Add(assignmentIndex);
            }

            // Находим изображение задания и перекрашиваем его границу в зеленый
            SetAssignmentButtonState(assignment, true);

            if (completedAssignments.Count == Assignments.Count)
            {
                return;
            }

            var nextTaskIndex = assignmentIndex + 1;
            if (nextTaskIndex < Assignments.Count)
            {
                // Загрузка следующего задания
                var nextAssignment = Assignments[nextTaskIndex];
                NavigateToAssignment(nextAssignment);
            }
            else
            {
                // После последнего возвращаемся на экран выбора заданий
                _shell.CurrentFrame.Navigate(this);
            }
        }

        private void SetAssignmentButtonState(IAssignment assignment, bool successfullyCompleted)
        {
            var button = FindAssignmentButton(assignment);
            if (button == null)
            {
                return;
            }

            // Изменяем цвет границы
            ChangeBorderColor(assignment, successfullyCompleted);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Подстраивание под изменение окна
            wrapPanel.Children.Clear();
            GenerateAssignmentButtons();
        }
    }
}