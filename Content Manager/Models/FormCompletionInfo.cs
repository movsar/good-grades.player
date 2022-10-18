using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Models {
    // A nice way to capture the progress of completion for simple forms
    public class FormCompletionInfo {
        public event Action<bool> StatusChanged;
        public bool IsReady => _stats.Where(s => s.Value == true).Count() == _stats.Count();

        private readonly Dictionary<string, bool> _stats = new Dictionary<string, bool>();
        string[] _propertiesToWatch;
        public FormCompletionInfo(string[] propertiesToWatch, bool existingElement) {
            _propertiesToWatch = propertiesToWatch;

            // Initialize the dictionary
            foreach (var v in _propertiesToWatch) {
                _stats.Add(v, existingElement);
            }
        }

        public void Update(string propertyTitle, bool isSet) {
            if (_stats[propertyTitle] == isSet) {
                return;
            }

            _stats[propertyTitle] = isSet;

            StatusChanged?.Invoke(IsReady);
        }
    }
}
