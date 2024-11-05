using Data.Interfaces;
using System;

namespace Shared.Interfaces
{
    public interface IAssignmentViewer
    {
        // Assignment, Success (true/false)
        event Action<IAssignment, bool> AssignmentCompleted;

        // gnment, Assignment Item's Id, Success (true/false)
        event Action<IAssignment, string, bool> AssignmentItemCompleted;
        void OnCheckClicked();
        void OnRetryClicked();
    }
}
