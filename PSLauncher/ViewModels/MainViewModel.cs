using Newtonsoft.Json;
using PSLauncher.Properties;
using PSNetCommon;
using PSNetCommon.Download;
using PSNetCommon.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace PSLauncher.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private const string CLIENT_INI = @"\client.ini";
        private const string PLANETSIDE_EXE = @"\planetside.exe";

        // Command line argument required to bypass Launcher warning error.
        private const string STAGING_TEST = "/K:StagingTest"; 

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value, "Progress"); }
        }

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

        public void ComputeCheckSums()
        {
            Task.Factory.StartNew(() =>
            {
                InfoString = Resources.CheckSumInfoString;
                Progress = 0;

                FileCheckSumRequest request = new FileCheckSumRequest();

                string[] files = Directory.GetFiles(Settings.Default.PlanetsideInstallDir, 
                    "*", SearchOption.AllDirectories);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < files.Length; i++)
                {
                    string checksum = CheckSum.CalculateMD5(files[i]);
                    request.AddFile(files[i], checksum);
                    Progress = (int)(100 * ((i + 1F) / files.Length));
                }
                sw.Stop();

#if DEBUG
                InfoString = "Checksum calculation took: " + sw.Elapsed.ToString();
#endif
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
        /// Runs the Planetside.exe from the current directory.
        /// </summary>
        public void RunPlanetsideExe()
        {
            var file = Settings.Default.PlanetsideInstallDir + PLANETSIDE_EXE;
            System.Diagnostics.Process app;

            if (!System.IO.File.Exists(file))
            {
                if (MessageBox.Show(Resources.NoPlanetsideDirFound) == MessageBoxResult.OK)
                {
                    /*
                     * This runs when using a Release build of the program.
                     * This allows functionality when running in debug mode inside of Visual Studio.
                     */
#if !DEBUG
                    Close();
#endif
                }
            }
            else
            {
                app = new System.Diagnostics.Process();
                app.StartInfo.FileName = file;
                app.StartInfo.Arguments = STAGING_TEST; // Required to bypass launcher.
                app.Start();
                /*
                 * This runs when using a Release build of the program.
                 * This allows functionality when running in debug mode inside of Visual Studio.
                 */
#if !DEBUG
                Close();
#endif
            }
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
