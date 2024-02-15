using Data.Entities;
using Data;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using Shared.Viewers;
using System.Windows;
using System.Windows.Controls;

namespace Content_Player.Pages
{
    public partial class AssignmentsPage : Page
    {

        private List<IAssignment> Assignments { get; } = new List<IAssignment>();
        private StylingService _stylingService = App.AppHost.Services.GetRequiredService<StylingService>();

        const int ButtonSize = 100;
        const int ButtonSpacing = 50;
        public AssignmentsPage(List<IAssignment> assignments)
        {
            InitializeComponent();
            Assignments.AddRange(assignments);
        }
        private void GenerateAssignmentButtons()
        {

            // Track the current row and column
            int row = 0, column = 0;
            // Used to create the checkmate pattern
            bool isOffset = false;

            WrapPanel wrapPanel = new WrapPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = this.ActualWidth
            };

            // Determine the number of buttons that can fit in a row based on the container's width
            int buttonsPerRow = (int)wrapPanel.Width / (ButtonSize + ButtonSpacing) - 1;

            int count = 1;
            foreach (var assignment in Assignments)
            {
                // Create a new button for the assignment
                Button button = new Button
                {
                    Content = count.ToString(),
                    Width = ButtonSize,
                    Height = ButtonSize,
                    Margin = new Thickness(ButtonSpacing),
                    Style = _stylingService.CircularButtonStyle,
                };

                button.Click += AssignmentButton_Click;

                // Add the button to the wrap panel at the correct position
                if (isOffset && column == 0)
                {
                    // Add an offset at the beginning of an offset row
                    wrapPanel.Children.Add(new TextBlock { Width = ButtonSize });
                }

                wrapPanel.Children.Add(button);

                // Update the row and column for the next button
                column++;
                if (column >= buttonsPerRow)
                {
                    isOffset = !isOffset;
                    column = 0;
                    row++;
                }

                count++;
            }

            ScrollViewerContainer.Content = wrapPanel;
            ScrollViewerContainer.UpdateLayout();
        }


        private void AssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button)e.Source;
            var assignment = Assignments[int.Parse(clickedButton.Content.ToString()!) - 1];

            Window viewer = null!;
            switch (assignment)
            {
                case MatchingTaskAssignment:
                    viewer = new MatchingViewer((MatchingTaskAssignment)assignment);
                    break;

                case TestingTaskAssignment:
                    viewer = new TestingViewer((TestingTaskAssignment)assignment);
                    break;

                case FillingTaskAssignment:
                    viewer = new FillingViewer((FillingTaskAssignment)assignment);
                    break;

                case SelectingTaskAssignment:
                    viewer = new SelectingViewer((SelectingTaskAssignment)assignment);
                    break;
                case BuildingTaskAssignment:
                    viewer = new BuildingViewer((BuildingTaskAssignment)assignment);
                    break;
            }

            viewer.Show();

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var widthDifference = this.ActualWidth - e.PreviousSize.Width;
            var heightDifference = this.ActualHeight - e.PreviousSize.Height;

            if (widthDifference < ButtonSize && heightDifference < ButtonSize)
            {
                return;
            }

            ScrollViewerContainer.Content = null;
            GenerateAssignmentButtons();
        }
    }
}
