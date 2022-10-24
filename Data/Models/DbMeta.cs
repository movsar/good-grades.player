using Data.Entities;
using Data.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class DbMeta : ModelBase, IDbMeta
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DbMeta(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public DbMeta()
        {
        }

        public DbMeta(DbMetaEntity optionEntity)
        {
            Id = optionEntity.Id;
            CreatedAt = optionEntity.CreatedAt;
            ModifiedAt = optionEntity.ModifiedAt;

            Title = optionEntity.Title;
            Description = optionEntity.Description;
        }
    }
}
