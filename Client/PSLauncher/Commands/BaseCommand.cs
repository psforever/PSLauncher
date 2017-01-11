using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PSLauncher.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        /// <summary>
        /// Dispatches the CanExecuteChanged command to the GUI thread.
        /// </summary>
        protected void DispatchCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                    CanExecuteChanged(this, null)));
            }
        }

        protected void CallCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
    }
}
