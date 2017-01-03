using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace PSLauncher
{
    public partial class ChangeServers : Form
    {
        public ChangeServers()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create(ManifestFile.Text.ToString());

            request.ContentType = "text/xml";
            request.ReadWriteTimeout = 5000;
            request.Timeout = 5000;

            var response = (HttpWebResponse)request.GetResponse();

            response.Close();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Properties.Settings.Default.Manifest = ManifestFile.Text;
                Properties.Settings.Default.Save();

                MainWindow.MainWindowClass.RefreshConnection();
            }

            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ManifestFile.Text = Properties.Settings.Default.DefaultManifest;
        }

        private void ChangeServers_Load(object sender, EventArgs e)
        {
            ManifestFile.Text = Properties.Settings.Default.Manifest;
        }
    }
}