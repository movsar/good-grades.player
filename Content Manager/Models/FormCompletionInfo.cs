using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Models
{
    // A nice way to capture the progress of completion for simple forms
    public class FormCompletionInfo
    {
        public event Action<bool> StatusChanged;
        public bool IsReady => _states.Where(s => s.Value == true).Count() == _states.Count();

        private readonly Dictionary<string, bool> _states = new Dictionary<string, bool>();
        private readonly List<string> _propertiesToWatch;
        public FormCompletionInfo(List<string> propertiesToWatch, bool existingElement)
        {
            _propertiesToWatch = propertiesToWatch;

            // Initialize the dictionary
            foreach (var prop in _propertiesToWatch)
            {
                _states.Add(prop, existingElement);
            }
        }

        public void Update(string propertyTitle, bool isSet)
        {
            if (!_states.ContainsKey(propertyTitle) || _states[propertyTitle] == isSet)
            {
                return;
            }

            _states[propertyTitle] = isSet;

            StatusChanged?.Invoke(IsReady);
        }
    }
}
