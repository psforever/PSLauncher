using Newtonsoft.Json;
using PSLauncher.Commands;
using PSLauncher.Interfaces;
using PSLauncher.Properties;
using PSNetCommon;
using PSNetCommon.Download;
using PSNetCommon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace PSLauncher.ViewModels
{
    class MainViewModel : BaseViewModel, IProgressView
    {
        private const string CLIENT_INI = @"\client.ini";

        private ChangeClientDirectoryCommand _changeClientDirectoryCommand = new ChangeClientDirectoryCommand();
        public ChangeClientDirectoryCommand ChangeClientDirectoryCommand
        {
            get { return _changeClientDirectoryCommand; }
        }

        private UpdateCommand _updateCommand;
        private PlayCommand _playCommand;
        public ICommand StartButtonCommand { get; private set; }

        public string StartButtonText
        {
            get { return (_updateCommand.HasExecuted) ? "Play" : "Update"; }
        }

        #region IProgressView
        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value, "Progress"); }
        }

        public string ProgressInfo
        {
            get { return InfoString; }
            set { InfoString = value; }
        }
        #endregion

        private string _patchNotes;
        public string PatchNotes
        {
            get { return _patchNotes; }
            set { SetProperty(ref _patchNotes, value, "PatchNotes"); }
        }

        private string _infoString;
        public string InfoString
        {
            get { return _infoString; }
            set { SetProperty(ref _infoString, value, "InfoString"); }
        }

        public MainViewModel()
        {
            PatchNotes = "Currently loading the launcher.\n Please wait";
            _updateCommand = new UpdateCommand(this);
            _updateCommand.CanExecuteChanged += _updateCommand_CanExecuteChanged;

            _playCommand = new PlayCommand();

            StartButtonCommand = new SequentialCompositeCommand(new List<ICommand>
            {
                _updateCommand,
                _playCommand
            });
        }

        private void _updateCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            if (!_updateCommand.IsExecuting)
            {
                OnPropertyChanged("StartButtonCommand");
                OnPropertyChanged("StartButtonText");
            }
        }

        public void ViewLoaded()
        {
            CheckSettings();
            RefreshConnectionJSON();
        }

        public void DownloadFile()
        {
            Task.Factory.StartNew(() =>
            {
                Downloader dl = new Downloader(Settings.Default.UpdateServerIP,
                                           Settings.Default.UpdateServerUser,
                                           Settings.Default.UpdateServerPassword);

                dl.Download("/planetside/path.txt");
            });
        }

        /// <summary>
        /// Performs basic checks on the various settings to ensure that 
        /// they are correct. User prompts will occur to populate any missing
        /// settings.
        /// </summary>
        private void CheckSettings()
        {
            string planetsideDir = Settings.Default.PlanetsideInstallDir;

            if (string.IsNullOrEmpty(planetsideDir) || !Directory.Exists(planetsideDir))
            {
                ChangeClientDirectoryCommand.Execute(null);
            }
        }

        /// <summary>
        /// Adjust Client Server Information
        /// </summary>
        public void RefreshConnectionJSON()
        {
            var manifest = DownloadManifestJSON(Properties.Settings.Default.Manifest);

            InfoString = $"{manifest.NetInfo.Name} - {manifest.NetInfo.Location}";

            Properties.Settings.Default.AccountURL = manifest.NetInfo.Url;

            SetPatchNotes(manifest.EmuInfo);
            UpdateClientIni(manifest.NetInfo);
        }

        /// <summary>
        /// Method that downloads Manifest as Json. Both faster and simpler in code.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public LauncherInfo DownloadManifestJSON(string url)
        {
            var client = new WebClient();
            var infoString = client.DownloadString(url);

            var info = JsonConvert.DeserializeObject<LauncherInfo>(infoString);

            return info;
        }

        /// <summary>
        /// Get the path the current executable is located in.
        /// </summary>
        /// <returns>Path</returns>
        private string GetCurrentPath()
        {
            var currentPath = System.IO.Directory.GetCurrentDirectory();
            currentPath = System.Uri.UnescapeDataString(currentPath);
            currentPath = System.IO.Path.GetFullPath(currentPath);

            return currentPath;
        }

        /// <summary>
        /// Updates Client Update Notes
        /// </summary>
        /// <param name="emuInfo"></param>
        public void SetPatchNotes(EmulatorInfo emuInfo)
        {
            string patchNotes = string.Empty;  // Clear the Patch Notes area

            foreach (var patch in emuInfo.Patches)
            {
                foreach (var change in patch.Changes)
                {
                    patchNotes += $" - {change.Date.ToShortDateString()}: \t{change.Value}{Environment.NewLine}";
                }

                patchNotes += Environment.NewLine;
            }

            PatchNotes = patchNotes;
        }

        /// <summary>
        /// Update Client.ini using the Network Information
        /// </summary>
        /// <param name="netInfo">NetworkInfo</param>
        public void UpdateClientIni(NetworkInfo netInfo)
        {
            var file = Settings.Default.PlanetsideInstallDir + CLIENT_INI;

            if (!System.IO.File.Exists(file))
            {
                if (MessageBox.Show(Resources.NoPlanetsideDirFound) == MessageBoxResult.OK)
                {
#if !DEBUG
                    Close();
#endif
                }
                else
                {
                    System.IO.File.Copy(file, $"{file}.bak", true);

                    using (var strWriter = new System.IO.StreamWriter(file, false))
                    {
                        strWriter.WriteLine("[network]" + Environment.NewLine);

                        foreach (var server in netInfo.Servers)
                        {
                            var address = $"{server.Address}:{server.Port}";

                            strWriter.WriteLine($"# {server.Namespace}{Environment.NewLine}");
                            strWriter.WriteLine($"{server.Id}={address}");
                        }
                    }
                }
            }

            Progress = 100;
        }
    }
}
