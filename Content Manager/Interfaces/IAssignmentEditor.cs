using Data.Interfaces;

namespace Content_Manager.Interfaces
{
    public interface IAssignmentEditor
    {
        bool? ShowDialog();
        IAssignment Assignment { get; }
    }
}
