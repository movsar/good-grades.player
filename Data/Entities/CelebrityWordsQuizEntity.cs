﻿using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;

namespace Data.Entities {
    public class CelebrityWordsQuizEntity : RealmObject, IEntityBase, ICelebrityWordsQuiz {
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [Required]
        public string SegmentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public IList<CwqOptionEntity> Options { get; }
        #endregion

        #region HelperMethods
        public IModelBase AsModel() {
            return new CelebrityWordsQuiz(this);
        }
        public void SetFromModel(IModelBase model) {
            var celebrityWordsQuiz = model as CelebrityWordsQuiz;
            SegmentId = celebrityWordsQuiz!.SegmentId;

            Utils.SyncLists(Options, celebrityWordsQuiz.Options);
        }
        #endregion
    }
}