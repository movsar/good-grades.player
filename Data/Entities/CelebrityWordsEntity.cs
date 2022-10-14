using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities {
    internal class CelebrityWordsQuizEntity : RealmObject, IEntityBase, ICelebrityWordsQuiz {

        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        [Required]
        public string SegmentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;

        [Ignored]
        public Dictionary<string, KeyValuePair<byte[], string>> Data {
            get {
                var outputData = new Dictionary<string, KeyValuePair<byte[], string>>();

                var ids = _optionsImageToId.Keys;
                foreach (var id in ids) {
                    outputData.Add(id, GetOption(id));
                }
                return outputData;
            }

            set {
                _optionsImageToId.Clear();
                _optionsTextToId.Clear();
                foreach (var kvp in value) {
                    AddOption(kvp.Value);
                }
            }
        }
        #endregion

        #region Fields
        private Dictionary<string, byte[]> _optionsImageToId = new();
        private Dictionary<string, string> _optionsTextToId = new();
        #endregion

        #region HelperMethods
        public void AddOption(KeyValuePair<byte[], string> kvp) {
            var id = ObjectId.GenerateNewId().ToString();
            _optionsImageToId.Add(id, kvp.Key);
            _optionsTextToId.Add(id, kvp.Value);
        }

        public void UpdateOption(string id, KeyValuePair<byte[], string> kvp) {
            _optionsImageToId[id] = kvp.Key;
            _optionsTextToId[id] = kvp.Value;
        }

        public KeyValuePair<byte[], string> GetOption(string id) {
            return new KeyValuePair<byte[], string>(_optionsImageToId[id], _optionsTextToId[id]);
        }
        #endregion

        public void SetFromModel(IModelBase model) {
            var celebrityWordsQuiz = model as ICelebrityWordsQuiz;
            Data = celebrityWordsQuiz.Data;
        }

    }
}
