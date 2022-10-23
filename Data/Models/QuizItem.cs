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
    public class QuizItem : ModelBase, IQuizItem
    {
        public string Text { get; set; }
        public byte[] Image { get; set; }

        public QuizItem(string wordsCollection, byte[] image)
        {
            Text = wordsCollection;
            Image = image;
        }

        public QuizItem()
        {
        }

        public QuizItem(QuizItemEntity optionEntity)
        {
            Id = optionEntity.Id;
            CreatedAt = optionEntity.CreatedAt;
            ModifiedAt = optionEntity.ModifiedAt;

            Text = optionEntity.Text;
            Image = optionEntity.Image;
        }
    }
}
