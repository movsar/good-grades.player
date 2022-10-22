﻿using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using static System.Net.Mime.MediaTypeNames;

namespace Data.Entities {
    public class SegmentEntity : RealmObject, ISegment, IEntityBase {
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<ReadingMaterialEntity> ReadingMaterials { get; }
        public IList<ListeningMaterialEntity> ListeningMaterials { get; }
        public CelebrityWordsQuizEntity CelebrityWordsQuiz { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public IModelBase ToModel() {
            return new Segment(this);
        }
        public void SetFromModel(IModelBase model) {
            // Set segment info, stuff that happens prior committing data from the Segment model
            var segment = model as Segment;
            Title = segment!.Title;
            Description = segment.Description;
            ModifiedAt = DateTime.Now;

            // These two method calls include all the changes made in reading and listening materials
            // while saving a Segment. Instead, it would be better to save them thorugh their own
            // repositories.
            Utils.SyncLists(ListeningMaterials, segment.ListeningMaterials);
            Utils.SyncLists(ReadingMaterials, segment.ReadingMaterials);

            if (segment.Id == null) {
                // Initialize a new segment
                CelebrityWordsQuiz = new();
                //    segment.Id = Id;
                //    segment.CreatedAt = CreatedAt;
                //    segment.ModifiedAt = ModifiedAt;
                //    segment.CelebrityWodsQuiz = new CelebrityWordsQuiz(CelebrityWordsQuiz);
            }
        }
    }
}
