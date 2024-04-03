using Data.Interfaces;

namespace Content_Manager.Interfaces
{
    public interface ITaskEditor
    {
        IAssignment Assignment { get; }
        bool? ShowDialog();
    }
}
