using Data.Entities;
using Data.Interfaces;
using Shared.Viewers;
using System.Windows;
using System.Windows.Controls;
using Shared.Interfaces;
using System.Windows.Input;
using System.Windows.Media;

namespace GGPlayer.Pages
{
    public partial class AssignmentsPage : Page
    {
        private List<int> completedAssignments = new List<int>();
        private List<IAssignment> Assignments { get; } = new List<IAssignment>();
        const int ButtonSize = 150;
        const int ButtonSpacing = 50;
        public AssignmentsPage(List<IAssignment> assignments)
        {
            InitializeComponent();
            Assignments.AddRange(assignments);
        }

        
        private void GenerateAssignmentButtons()
        {
            // Clear previous content
            ScrollViewerContainer.Content = null;

            int buttonsPerRow = (int)(ScrollViewerContainer.ActualWidth / (ButtonSize + ButtonSpacing));

            WrapPanel wrapPanel = new WrapPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = ScrollViewerContainer.ActualWidth
            };

            int count = 1;
            foreach (var assignment in Assignments)
            {
                Button button = new Button
                {
                    Content = count.ToString(),
                    Width = ButtonSize,
                    Height = ButtonSize,
                    Margin = new Thickness(ButtonSpacing),
                    Style = (Style)FindResource("CircularButtonStyle"),
                    Cursor = Cursors.Hand
                };

                // Окрашиваем кнопку в зеленый, если задание уже выполнено
                if (completedAssignments.Contains(count - 1))
                {
                    button.Background = new SolidColorBrush(Colors.Green);
                }

                button.Click += AssignmentButton_Click;
                wrapPanel.Children.Add(button);
                count++;
            }

            ScrollViewerContainer.Content = wrapPanel;
        }

        private void AssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button)e.Source;
            var assignment = Assignments[int.Parse(clickedButton.Content.ToString()!) - 1];

            Window viewer = null!;
            //выбор типа задания
            switch (assignment)
            {
                case MatchingAssignment:
                    viewer = new MatchingViewer((MatchingAssignment)assignment);
                    break;

                case TestingAssignment:
                    viewer = new TestingViewer((TestingAssignment)assignment);
                    break;

                case FillingAssignment:
                    viewer = new FillingViewer((FillingAssignment)assignment);
                    break;

                case SelectingAssignment:
                    viewer = new SelectingViewer((SelectingAssignment)assignment);
                    break;
                case BuildingAssignment:
                    viewer = new BuildingViewer((BuildingAssignment)assignment);
                    break;
            }

            // Подписываемся на событие закрытия окна
            viewer.Closed += (s, args) =>
            {
                // Проверяем, выполнены ли все задания после закрытия окна
                if (completedAssignments.Count == Assignments.Count)
                {
                    MessageBox.Show("ХӀокху декъера дерриг тӀедахкарш кхочушдина ахь!");
                }
            };

            // Подписываемся на изменение состояния выполнения задания
            ((IAssignmentViewer)viewer).CompletionStateChanged += AssignmentsPage_CompletionStateChanged;
            viewer.ShowDialog();
        }

        private void AssignmentsPage_CompletionStateChanged(IAssignment assignment, bool completionState)
        {
            if (!completionState)
            {
                return;
            }

            var assignmentIndex = Assignments.IndexOf(assignment);
            if (assignmentIndex == -1)
            {
                return;
            }

            // Добавляем задание в список выполненных
            if (!completedAssignments.Contains(assignmentIndex))
            {
                completedAssignments.Add(assignmentIndex);
            }

            var wrapPanel = ScrollViewerContainer.Content as WrapPanel;
            var buttonContentToFind = (assignmentIndex + 1).ToString();
            var button = wrapPanel.Children.OfType<Button>()
                                    .FirstOrDefault(b => b.Content.ToString() == buttonContentToFind);

            //перекраска кнопки выполненного задания в зеленый
            if (button != null)
            {
                button.Background = new SolidColorBrush(Colors.Green);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //подстраивание под изменение окна
            var widthDifference = ActualWidth - e.PreviousSize.Width;
            var heightDifference = ActualHeight - e.PreviousSize.Height;

            if (widthDifference < ButtonSize && heightDifference < ButtonSize)
            {
                return;
            }

            ScrollViewerContainer.Content = null;
            GenerateAssignmentButtons();
        }
    }
}
