using Data.Interfaces;

namespace Content_Manager.Interfaces
{
    public interface ITaskEditor
    {
        IAssignment TaskAssignment { get; }
        bool? ShowDialog();
    }
}
