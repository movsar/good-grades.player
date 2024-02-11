using Data.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace Content_Player.Windows
{
    public partial class AssignmentSelector : Window
    {
        private List<IAssignment> Assignments { get; } = new List<IAssignment>();

        public AssignmentSelector(List<IAssignment> assignments)
        {
            InitializeComponent();
            Assignments.AddRange(assignments);
            GenerateAssignmentButtons();
        }

        private void GenerateAssignmentButtons()
        {
            WrapPanel wrapPanel = new WrapPanel
            {
                // Set properties for the WrapPanel if needed, such as padding, item alignment, etc.
            };

            int count = 1;
            foreach (var assignment in Assignments)
            {
                Button button = new Button
                {
                    Content = count.ToString(),
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5),

                };

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
        }
    }
}
