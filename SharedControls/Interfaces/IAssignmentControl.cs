using Data.Entities;
using Data.Interfaces;
using System;

namespace Shared.Interfaces
{
    public interface IAssignmentControl
    {
        // Assignment, Success (true/false)
        event Action<IAssignment, bool> AssignmentCompleted;
        // Assignment, Assignment Item's Id, Success (true/false)
        event Action<IAssignment, string, bool> AssignmentItemSubmitted;

        // Steps before checking for the result
        int StepsCount { get; }
      
        // Event callbacks
        void OnCheckClicked();
        void OnRetryClicked();
        void OnNextClicked();
        void OnPreviousClicked();
        void Initialize(IAssignment assignment);
    }
}
