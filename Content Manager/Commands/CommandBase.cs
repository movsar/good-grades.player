using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace GGManager.Commands
{
    internal abstract class CommandBase : ICommand
    {
        protected static void CloseCurrentWindow()
        {
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)?.Close();
        }

        public event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public abstract void Execute(object? parameter);

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
