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
                    Style = (Style)FindResource("AssignmentButtonLabel"),
                    Tag = assignment
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
            var label = FindAssignmentButton(assignment);
            if (label == null)
            {
                return;
            }

            var effect = label.Effect as DropShadowEffect;
            if (isCompleted)
            {
                effect.Color = Colors.LimeGreen;
                effect.BlurRadius = 30;
            }
            else
            {
                effect.Color = Colors.Transparent;
                effect.BlurRadius = 0;
            }
        }

        // Метод для поиска изображения задания
        private Label FindAssignmentButton(IAssignment assignment)
        {
            foreach (var child in wrapPanel.Children)
            {
                var label = child as Label;
                if (label != null && label.Tag != null && label.Tag.Equals(assignment))
                {
                    return label;
                }
            }
            return null;
        }

        private void AssignmentButton_Click(object sender, MouseButtonEventArgs e)
        {
            var clickedButton = (Label)sender;
            var taskIndex = int.Parse(clickedButton.Content.ToString());
            var assignment = Assignments[taskIndex];

            // Когда окно задания закрывается, проверяем состояние выполнения
            //viewer.Closed += (s, args) =>
            //{
            //    if (completedAssignments.Count == Assignments.Count)
            //    {
            //        MessageBox.Show("ХӀокху декъера дерриг тӀедахкарш кхочушдинаахь!");
            //    }
            //};

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

            // Загрузка следующего задания
            if (assignmentIndex + 1 < Assignments.Count)
            {
                var nextAssignment = Assignments[assignmentIndex + 1];
                NavigateToAssignment(nextAssignment);
            }
            else
            {
                _shell.CurrentFrame.Navigate(this);
            }
        }

        private void SetAssignmentButtonState(IAssignment assignment, bool successfullyCompleted)
        {
            var image = FindAssignmentButton(assignment);
            if (image != null)
            {
                // Изменяем цвет границы
                ChangeBorderColor(assignment, successfullyCompleted);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Подстраивание под изменение окна
            wrapPanel.Children.Clear();
            GenerateAssignmentButtons();
        }
    }
}