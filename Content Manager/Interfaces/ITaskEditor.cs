using Data.Interfaces;

namespace Content_Manager.Interfaces
{
    public interface ITaskEditor
    {
        ITaskAssignment TaskAssignment { get; }
        bool? ShowDialog();
    }
}
