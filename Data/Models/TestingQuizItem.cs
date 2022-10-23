using Data.Entities;
using Data.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TestingQuizItem : ModelBase, ITestingQuizItem
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public IList<string> Options { get; set; }

        public TestingQuizItem(string question, string answer, IList<string> options)
        {
            Question = question;
            Answer = answer;
            Options = options;
        }

        public TestingQuizItem()
        {
        }

        public TestingQuizItem(TestingQuizItemEntity quizItemEntity)
        {
            Id = quizItemEntity.Id;
            CreatedAt = quizItemEntity.CreatedAt;
            ModifiedAt = quizItemEntity.ModifiedAt;

            Question = quizItemEntity.Question;
            Answer = quizItemEntity.Answer;
            Options = quizItemEntity.Options;
        }
    }
}
