using Data.Interfaces;
using Data.Models;
using MongoDB.Bson;
using Realms;

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
            
            SetCwqOptions(celebrityWordsQuiz);
        }

        private void SetCwqOptions(CelebrityWordsQuiz cwq) {
            if (cwq.Options == null) {
                return;
            }

            // Commit removed options
            var currentOptionIds = cwq.Options.Select(x => x.Id).ToList();
            var optionsToRemove = Options.Where(rm => !currentOptionIds.Contains(rm.Id));
            foreach (var option in optionsToRemove) {
                Options.Remove(option);
            }

            // Add or update options
            foreach (var option in cwq.Options) {
                var existingOption = Options?.FirstOrDefault((rm => rm.Id == option.Id));
                if (existingOption != null) {
                    existingOption.SetFromModel(option);
                } else {
                    var newOption = new CwqOptionEntity();
                    newOption.SetFromModel(option);
                    Options?.Add(newOption);

                    option.Id = newOption.Id;
                }
            }
        }
        #endregion
    }
}
