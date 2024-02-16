using Data.Interfaces;
using System;

namespace Shared.Interfaces
{
    public interface IAssignmentViewer
    {
        // This will be triggered whenever the user checks their  answers,
        // it will return true if it's correct and false if not
        public event Action<IAssignment, bool> CompletionStateChanged;
    }
}
