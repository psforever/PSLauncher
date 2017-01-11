using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PSLauncher.Commands
{
    class SequentialCompositeCommand : BaseCommand
    {
        private List<ICommand> _commands { get; set; }
        private int _index = 0;

        public SequentialCompositeCommand(List<ICommand> commands)
        {
            _commands = new List<ICommand>(commands); // Encapsulate
        }

        public override bool CanExecute(object parameter)
        {
            if (_commands.Count > 0 && _index < _commands.Count)
            {
                return _commands[_index].CanExecute(parameter);
            }

            return false;
        }

        public override void Execute(object parameter)
        {
            _commands[_index].Execute(parameter);
            _index++;
        }
    }
}
