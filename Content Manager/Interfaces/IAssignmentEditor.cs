using GGManager.UserControls;
using Data.Entities.TaskItems;
using Data.Interfaces;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GGManager.Interfaces
{
    public interface IAssignmentEditor
    {
        bool? ShowDialog();
        IAssignment Assignment { get; }

        internal static void SetAssignmentItems(IList<AssignmentItem> assignmentItems, StackPanel spItems)
        {
            // Extract Assignment Items from UI
            assignmentItems.Clear();
            foreach (var item in spItems.Children)
            {
                var aiEditControl = item as AssignmentItemEditControl;
                if (aiEditControl == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(aiEditControl.Item.Text))
                {
                    continue;
                }

                if (!aiEditControl.IsValid)
                {
                    continue;
                }

                assignmentItems.Add(aiEditControl.Item);
            }
        }
    }
}
