using Data.Entities.TaskItems;
using System.Collections.Generic;
using System.Linq;

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
        public static List<string> GetCorrectOptionIds(Question question)
        {
            return question.Options
                           .Where(option => option.IsChecked)
                           .Select(option => option.Id)
                           .ToList();
        }

        public static bool CheckAnswersForQuestion(Question question, List<string> userSelectedIds)
        {
            var correctOptionIds = GetCorrectOptionIds(question);
            return correctOptionIds.Count == userSelectedIds.Count &&
                   !correctOptionIds.Except(userSelectedIds).Any();
        }
    }
}
