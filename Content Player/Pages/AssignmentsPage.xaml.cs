using Data.Entities;
using Data;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using Shared.Viewers;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Shared.Interfaces;
using System.Text.RegularExpressions;

namespace Content_Player.Pages
{
    public partial class AssignmentsPage : Page
    {

        private List<IAssignment> Assignments { get; } = new List<IAssignment>();
        private Dictionary<string, bool> _assignmentCompletionStatus = new Dictionary<string, bool>();
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
                    Style = (Style)FindResource("CircularButtonStyle"),
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

            ((IAssignmentViewer)viewer).CompletionStateChanged += AssignmentsPage_CompletionStateChanged;
            viewer.ShowDialog();

            // TODO: Check whether the user had solved anything, if so - set green
        }

        private void AssignmentsPage_CompletionStateChanged(IAssignment assignment, bool completionState)
        {
            if (!completionState)
            {
                return;
            }

            // Find the index of the received assignment in the Assignments list
            var assignmentIndex = Assignments.IndexOf(assignment);
            if (assignmentIndex == -1)
            {
                return;
            }

            // Assuming the WrapPanel is the only child of ScrollViewerContainer.Content
            var wrapPanel = ScrollViewerContainer.Content as WrapPanel;
            // Find the button with the content that matches the index + 1 (since you started counting from 1)
            var buttonContentToFind = (assignmentIndex + 1).ToString();
            var button = wrapPanel.Children
                          .OfType<Button>()
                          .FirstOrDefault(b => b.Content.ToString() == buttonContentToFind);

            if (button != null)
            {
                // Change the background color of the button to Green
                button.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            }
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
