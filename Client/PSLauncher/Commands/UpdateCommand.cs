using PSLauncher.Interfaces;
using PSLauncher.Properties;
using PSNetCommon;
using PSNetCommon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PSLauncher.Commands
{
    class UpdateCommand : BaseCommand
    {
        private IProgressView _view;

        public bool IsExecuting { get; private set; }
        public bool HasExecuted { get; private set; }

        public UpdateCommand(IProgressView view)
        {
            _view = view;
            HasExecuted = false;
        }

        public override bool CanExecute(object parameter)
        {
            return !IsExecuting && !HasExecuted;
        }

        public override void Execute(object parameter)
        {
            HasExecuted = false;
            IsExecuting = true;
            CallCanExecuteChanged();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    FileCheckSumRequest request = new FileCheckSumRequest();
                    ComputeCheckSums(request);

                    List<string> filesToUpdate = request.Send();

                    HasExecuted = true;
                }
                catch (Exception ex)
                {
                    _view.ProgressInfo = "Failed to update client files.";
                }

                IsExecuting = false;
                DispatchCanExecuteChanged();
            });
        }

        private void ComputeCheckSums(FileCheckSumRequest request)
        {
            _view.ProgressInfo = Resources.CheckSumInfoString;
            _view.Progress = 0;

            string[] files = Directory.GetFiles(Settings.Default.PlanetsideInstallDir,
                "*", SearchOption.AllDirectories);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var writer = new StreamWriter(File.OpenWrite("extractFile.csv"));

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] != "extractFile.csv")
                {
                    string checksum = CheckSum.CalculateMD5(files[i]);
                    request.AddFile(files[i], checksum);
                    _view.Progress = (int)(100 * ((i + 1F) / files.Length));

                    writer.WriteLine("INSERT INTO client_files (filename, checksum) VALUES ('{0}', '{1}');",
                        files[i].Replace(Settings.Default.PlanetsideInstallDir, "").Replace(@"\", @"/"),
                        checksum);
                }
            }

            writer.Close();
            sw.Stop();

#if DEBUG
            _view.ProgressInfo = $"Checksum calculation took: {sw.Elapsed.Seconds.ToString()} seconds";
#endif
        }
    }
}
