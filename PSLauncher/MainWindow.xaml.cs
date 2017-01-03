using System;
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


namespace PSLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Quick Workaround for VB.net code. 
        /// Only one should instantiate so 
        /// it should be fine.
        /// </summary>
        public static MainWindow MainWindowClass { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            MainWindowClass = this;
        }

        public void RefreshConnection()
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

        private void SetPatchNotes(XDocument manifest)
        {
            patchNotesTxt.Text = string.Empty;

            foreach(var node in manifest.Descendants("patch"))
            {
                var version = node.Element("version").Value;    // This was better in VB.net unfortunately.

                patchNotesTxt.Text += "Version: " + version + Environment.NewLine;

                foreach(var change in node.Descendants("change"))
                {
                    var dateValue = change.Attribute("date").Value;
                    var changeValue = change.Value.Trim();

                    patchNotesTxt.Text += " - " + dateValue + ": " + changeValue + Environment.NewLine;
                }

                patchNotesTxt.Text += Environment.NewLine;
            }
        }

        private void UpdateClientIni(XDocument manifest)
        {
            var path = GetCurrentPath();
            var file = path + @"\client.ini";

            if (!System.IO.File.Exists(file))
            {
                if (MessageBox.Show("The launcher is not currently in the PSForever directory.") == MessageBoxResult.OK)
                {
#if !DEBUG
                    Close();
#endif
                }
                else
                {
                    System.IO.File.Copy(file, file + ".bak", true);

                    using (var strWriter = new System.IO.StreamWriter(file, false))
                    {
                        strWriter.WriteLine("[network]" + Environment.NewLine);

                        foreach(var node in manifest.Descendants("server"))
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

        private void RunGame()
        {
            var path = GetCurrentPath();
            var file = path + @"\planetside.exe";
            System.Diagnostics.Process app;

            if (!System.IO.File.Exists(file))
            {
                if (MessageBox.Show("The Launcher is not currently in the PSForever directory.") == MessageBoxResult.OK)
                {
#if !DEBUG
                    Close();
#endif
                }
            }
            else
            {
                app = new System.Diagnostics.Process();
                app.StartInfo.FileName = file;
                app.StartInfo.Arguments = "/K:StagingTest";

                app.Start();

#if !DEBUG
                Close();
#endif
            }
        }

        private string GetCurrentPath()
        {
            var currentPath = System.IO.Directory.GetCurrentDirectory();
            currentPath = System.Uri.UnescapeDataString(currentPath);
            currentPath = System.IO.Path.GetFullPath(currentPath);

            return currentPath;
        }

        private XDocument DownloadManifestXML(string url)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            XDocument manifest;

            request = (HttpWebRequest)WebRequest.Create(url);

            request.ContentType = "text/xml";
            request.ReadWriteTimeout = 5000;
            request.Timeout = 5000;

            response = (HttpWebResponse)request.GetResponse();

            response.Close();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                manifest = XDocument.Load(url);

                return manifest;
            }

            return null;
        }

        private void UpdateProgress(int value)
        {
            var duration = new Duration(TimeSpan.FromSeconds(0.1));
            var doubleAnimation = new DoubleAnimation(value, duration);
            prgBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);
        }

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

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            RunGame();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            patchNotesTxt.Text = "Currently loading the launcher.\n"
                + "Please wait.";

            RefreshConnection();
        }

        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            var changeServers = new ChangeServers();
            changeServers.Show();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshConnection();
        }

        private void accountBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.AccountURL);
        }

        private void logo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String("Q3JlYXRlZCBieSBtb29rIChwc21vb2sp")));
        }
    }
}