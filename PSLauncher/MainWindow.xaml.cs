﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

using Newtonsoft.Json;
using PSNetCommon;


namespace PSLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Run The Client
        /// </summary>
        private void RunGame()
        {
            var path = GetCurrentPath();
            var file = path + @"\planetside.exe";
            System.Diagnostics.Process app;

            if (!System.IO.File.Exists(file))
            {
                if (MessageBox.Show("The Launcher is not currently in the PSForever directory.") == MessageBoxResult.OK)
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
                // Argument "/K:StagingTest" required to bypass Launcher warning error.
                app.StartInfo.Arguments = "/K:StagingTest";

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

        /*
         * The Methods Below are used for handling updating the launcher information with XML.
         * The new method that will be handled in the PSNetCommon Library will implement JSON.
         */

        #region XML Refresh

        /// <summary>
        /// Download Manifest for Server information.
        /// </summary>
        [Obsolete]
        public void RefreshConnectionXML()
        {
            var manifest = DownloadManifestXML(Properties.Settings.Default.Manifest);

            foreach (var network in manifest.Descendants("network"))
            {
                var ServerName = network.Attribute("name").Value;
                var ServerLocation = network.Attribute("location").Value;

                infoLbl.Content = ServerName + " - " + ServerLocation;

                Properties.Settings.Default.AccountURL = network.Attribute("url").Value;
            }

            SetPatchNotes(manifest);
            UpdateClientIni(manifest);
        }

        #endregion 

        #region XML Patch Notes
        /// <summary>
        /// Update Launcher Patch Information
        /// </summary>
        /// <param name="manifest">Manifest File</param>
        [Obsolete]
        private void SetPatchNotes(XDocument manifest)
        {
            patchNotesTxt.Text = string.Empty;

            foreach (var node in manifest.Descendants("patch"))
            {
                var version = node.Element("version").Value;    // This was better in VB.net unfortunately.

                patchNotesTxt.Text += "Version: " + version + Environment.NewLine;

                foreach (var change in node.Descendants("change"))
                {
                    var dateValue = change.Attribute("date").Value;
                    var changeValue = change.Value.Trim();

                    patchNotesTxt.Text += " - " + dateValue + ": \t" + changeValue + Environment.NewLine;
                }

                patchNotesTxt.Text += Environment.NewLine;
            }
        }
        #endregion

        #region XML Manifest Download
        /// <summary>
        /// Download Manifest Online
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        [Obsolete]
        private XDocument DownloadManifestXML(string url)
        {
            /*
             * Downloads the XML manifest to use to provide settings to the
             * application. This XML contains up-to-date server information,
             * server patch information, even [potentially] client mod 
             * information.
             */
            HttpWebRequest request;
            HttpWebResponse response;
            XDocument manifest;

            request = (HttpWebRequest)WebRequest.Create(url);

            request.ContentType = "text/xml";
            request.ReadWriteTimeout = 5000;
            request.Timeout = 5000;

            response = (HttpWebResponse)request.GetResponse();

            response.Close();

            // Load Document if it exists
            if (response.StatusCode == HttpStatusCode.OK)
            {
                manifest = XDocument.Load(url);

                return manifest;
            }

            return null;
        }
        #endregion

        #region XML Client.ini
        /// <summary>
        /// Update client.ini to use appropriate server settings.
        /// </summary>
        /// <param name="manifest">Manifest File</param>
        [Obsolete]
        private void UpdateClientIni(XDocument manifest)
        {
            var path = GetCurrentPath();
            var file = path + @"\client.ini";

            if (!System.IO.File.Exists(file))
            {
                if (MessageBox.Show("The launcher is not currently in the PSForever directory.") == MessageBoxResult.OK)
                {
                    /*
                     * This runs when using a Release build of the program.
                     * This allows functionality when running in debug mode inside of Visual Studio.
                     */
#if !DEBUG
                    Close();
#endif
                }
                else
                {
                    /*
                     * Write the client.ini information. This is obtained through the manifest that
                     * is hosted online. These are changeable through both the client, and the settings
                     * file that is available.
                     */
                    System.IO.File.Copy(file, file + ".bak", true);

                    using (var strWriter = new System.IO.StreamWriter(file, false))
                    {
                        strWriter.WriteLine("[network]" + Environment.NewLine);

                        foreach (var node in manifest.Descendants("server"))
                        {
                            var address = node.Element("address").Value + ":" + node.Element("port").Value;

                            strWriter.WriteLine("# " + node.Attribute("namespace").Value + Environment.NewLine);
                            strWriter.WriteLine(node.Attribute("id").Value + "=" + address + Environment.NewLine);
                        }
                    }
                }
            }

            UpdateProgress(100);
            playBtn.IsEnabled = true;
        }

        #endregion

        /// <summary>
        /// Adjust Client Server Information
        /// </summary>
        public void RefreshConnectionJSON()
        {
            var manifest = DownloadManifestJSON(Properties.Settings.Default.Manifest);

            infoLbl.Content = $"{manifest.NetInfo.Name} - {manifest.NetInfo.Location}";

            Properties.Settings.Default.AccountURL = manifest.NetInfo.Url;

            SetPatchNotes(manifest.EmuInfo);
            UpdateClientIni(manifest.NetInfo);
        }

        /// <summary>
        /// Updates Client Update Notes
        /// </summary>
        /// <param name="emuInfo"></param>
        public void SetPatchNotes(EmulatorInfo emuInfo)
        {
            patchNotesTxt.Text = string.Empty;  // Clear the Patch Notes area

            foreach (var patch in emuInfo.Patches)
            {
                foreach(var change in patch.Changes)
                {
                    patchNotesTxt.Text += $" - {change.Date.ToShortDateString()}: \t{change.Value}{Environment.NewLine}";
                }

                patchNotesTxt.Text += Environment.NewLine;
            }
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
        /// Update Client.ini using the Network Information
        /// </summary>
        /// <param name="netInfo">NetworkInfo</param>
        public void UpdateClientIni(NetworkInfo netInfo)
        {
            var path = GetCurrentPath();
            var file = path + @"\client.ini";

            if (!System.IO.File.Exists(file))
            {
                if (MessageBox.Show("The launcher is not currently in the Client directory.") == MessageBoxResult.OK)
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

            UpdateProgress(100);
            playBtn.IsEnabled = true;
        }

        /// <summary>
        /// Method used to update progress bar with animation.
        /// </summary>
        /// <param name="value">float</param>
        private void UpdateProgress(int value)
        {
            var duration = new Duration(TimeSpan.FromSeconds(0.1));
            var doubleAnimation = new DoubleAnimation(value, duration);
            prgBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
        }

        /// <summary>
        /// Progress Bar Value Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prgBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            prgLbl.Content = (int)(prgBar.Value) + " / 100";
            if (prgBar.Value > 50)
            {
                prgLbl.Foreground = Brushes.Black;
            }

            if (prgBar.Value == 100)
                prgLbl.Content = "Complete";
        }

        /// <summary>
        /// Play Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            RunGame();
        }

        /// <summary>
        /// Window Loaded Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            patchNotesTxt.Text = "Currently loading the launcher.\n"
                + "Please wait.";

            RefreshConnectionJSON();
        }

        /// <summary>
        /// Change Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            var changeServers = new ChangeServers(this);    // Pass the current instance as the MainWindow to be used.
            changeServers.Show();
        }

        /// <summary>
        /// Refresh Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshConnectionJSON();
        }

        /// <summary>
        /// Account Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.AccountURL);
        }

        /// <summary>
        /// Logo Mouse Press Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*
             * This Message Box was originally done by the clever mook, known on Github to the PSForever project as PSmook.
             * This message box has been preserved to remember his original work on the Winform version of the launcher that
             * was used before being rewritten.
             */
            MessageBox.Show(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String("Q3JlYXRlZCBieSBtb29rIChwc21vb2sp")));
        }
    }
}