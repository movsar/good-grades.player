using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using System.Windows;
using System.Windows.Controls;

namespace Content_Player.Windows
{
    public partial class AssignmentSelector : Window
    {
        private List<IAssignment> Assignments { get; } = new List<IAssignment>();
        private StylingService _stylingService = App.AppHost.Services.GetRequiredService<StylingService>();
        public AssignmentSelector(List<IAssignment> assignments)
        {
            InitializeComponent();
            Assignments.AddRange(assignments);
            GenerateAssignmentButtons();
        }

        private void GenerateAssignmentButtons()
        {
            WrapPanel wrapPanel = new WrapPanel();

            int count = 1;
            foreach (var assignment in Assignments)
            {
                Button button = new Button
                {
                    Content = count.ToString(),
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5),
                    Style = _stylingService.CircularButtonStyle
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
