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
        /// <summary>
        /// Main window that the game launched with
        /// </summary>
        private MainWindow mainWindow;

        // Removed Default Constructor because this form cannot function without it.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mainWindow"></param>
        public ChangeServers(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;   // Assign the appropriate instance of MainWindow
        }

        /// <summary>
        /// Button Press Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                mainWindow.RefreshConnectionJSON();
            }

            Close();
        }

        /// <summary>
        /// Cancel Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Reset Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object sender, EventArgs e)
        {
            ManifestFile.Text = Properties.Settings.Default.DefaultManifest;
        }

        /// <summary>
        /// Form Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeServers_Load(object sender, EventArgs e)
        {
            ManifestFile.Text = Properties.Settings.Default.Manifest;
        }
    }
}