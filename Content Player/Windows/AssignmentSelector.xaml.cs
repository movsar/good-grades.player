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
            // Size of the buttons
            const int buttonSize = 100;
            // Space between buttons
            const int spacing = 50;

            // Track the current row and column
            int row = 0, column = 0;
            // Used to create the checkmate pattern
            bool isOffset = false;

            WrapPanel wrapPanel = new WrapPanel()
            {
                Width = this.Width
            };

            // Determine the number of buttons that can fit in a row based on the container's width
            int buttonsPerRow = (int)wrapPanel.Width / (buttonSize + spacing) - 1;

            int count = 1;
            foreach (var assignment in Assignments)
            {
                // Create a new button for the assignment
                Button button = new Button
                {
                    Content = count.ToString(),
                    Width = buttonSize,
                    Height = buttonSize,
                    Margin = new Thickness(spacing),
                    Style = _stylingService.CircularButtonStyle
                };

                // Add the button to the wrap panel at the correct position
                if (isOffset && column == 0)
                {
                    // Add an offset at the beginning of an offset row
                    wrapPanel.Children.Add(new TextBlock { Width = buttonSize });
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
        }


        private void AssignmentButton_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = (Button)e.Source;
            var assignment = Assignments[int.Parse(clickedButton.Content.ToString()!) - 1];
        }
    }
}
