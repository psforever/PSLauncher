using PSLauncher.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PSLauncher.Commands
{
    class PlayCommand : BaseCommand
    {
        private const string PLANETSIDE_EXE = @"\planetside.exe";
        private bool _gameIsRunning = false;

        public override bool CanExecute(object parameter)
        {
            return !_gameIsRunning;
        }

        public override void Execute(object parameter)
        {
            if(!_gameIsRunning)
                RunPlanetsideExe();
        }

        /// <summary>
        /// Runs the Planetside.exe from the current directory.
        /// </summary>
        private void RunPlanetsideExe()
        {
            try
            {
                var file = Settings.Default.PlanetsideInstallDir + PLANETSIDE_EXE;
                System.Diagnostics.Process app;

                if (!System.IO.File.Exists(file))
                {
                    MessageBox.Show(Resources.NoPlanetsideDirFound);
                    return;
                }

                app = new Process();
                app.StartInfo.WorkingDirectory = Settings.Default.PlanetsideInstallDir;
                app.StartInfo.FileName = file;
                app.StartInfo.Arguments = Settings.Default.PlanetsideExeArgs;
                app.StartInfo.RedirectStandardError = true;
                app.StartInfo.RedirectStandardOutput = true;
                app.StartInfo.UseShellExecute = false;
                app.EnableRaisingEvents = true;
                app.Exited += Planetside_Exited;

                if (app.Start())
                {
                    _gameIsRunning = true;
                    CallCanExecuteChanged();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Be sure you are running in admin mode.",
                    "Failed to Launch Planetside");
            }
        }

        private void Planetside_Exited(object sender, EventArgs e)
        {
            _gameIsRunning = false;
            CallCanExecuteChanged();
        }
    }
}
