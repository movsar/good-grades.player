using Data.Interfaces;

namespace Shared.Models
{
    public delegate void AssignmentItemCompletionCallback(IAssignment assignment, string itemId, bool success, bool isLast);
}
