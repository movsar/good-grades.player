using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.ViewModels {
    internal class ViewModelBase {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T oldValue, T newValue, string fieldName) {
            if (!EqualityComparer<T>.Default.Equals(oldValue, newValue)) {
                oldValue = newValue;
                var handler = PropertyChanged;
                if (handler != null) {
                    handler(this, new PropertyChangedEventArgs(fieldName));
                }
            }
        }

        public void OnPropertyChanged(string fieldName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(fieldName));
        }
    }
}
