﻿using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.Materials
{
    public class ReadingMaterial : RealmObject, IEntityBase
    {
        [Required][PrimaryKey] public string Id { get; } = ObjectId.GenerateNewId().ToString();
        [Required] public string Title { get; set; }
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Text { get; set; }
        public byte[] Image { get; set; }
    }
}