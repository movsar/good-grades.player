using Data.Entities.TaskItems;
using System.Collections.Generic;

namespace Shared.Services
{
    internal static class QuestionService
    {
        internal static bool CheckUserAnswers(Question question, List<string> selections)
        {
            foreach (var option in question.Options)
            {
                if (option.IsChecked && !selections.Contains(option.Id))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
