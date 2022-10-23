using Data.Interfaces;
using Data.Models;
using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TestingQuizItemEntity : RealmObject, ITestingQuizItem, IEntityBase
    {
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Question { get; set; }
        public string Answer { get; set; }
        public IList<string> Options { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;

        public IModelBase ToModel()
        {
            return new TestingQuizItem(this);
        }

        public void SetFromModel(IModelBase model)
        {
            var quizItem = model as TestingQuizItem;
            Question = quizItem.Question;
            Answer = quizItem.Answer;
            Options = quizItem.Options;
        }
    }
}
