using PSLauncher.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PSLauncher.Commands
{
    class ChangeClientDirectoryCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = Resources.PlanetsideDirPrompt;

            // HACK: There should be a more elegant way to do this.
            // Just quick and dirty here... Prompt until we get something.
            do
            {
                var result = dialog.ShowDialog();
            } while (!Directory.Exists(dialog.SelectedPath));

            Settings.Default.PlanetsideInstallDir = dialog.SelectedPath;
            Settings.Default.Save();
        }
    }
}
