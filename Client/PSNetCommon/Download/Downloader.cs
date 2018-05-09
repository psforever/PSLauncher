using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PSNetCommon.Download
{
    /// <summary>
    /// Static Class for downloading Client files
    /// </summary>
    public class Downloader
    {
        public Downloader(string server, string username, string password)
        {
            ServerFileHost = new FileHost(server, username, password);
        }

        /// <summary>
        /// Retrieves the specified file from the server.
        /// </summary>
        /// <param name="fullfilename">The filename including path to file.</param>
        public void Download(string fullFileName)
        {
            try
            {
                // See: https://msdn.microsoft.com/en-us/library/ms229711(v=vs.110).aspx

                // Get the object used to communicate with the server.
                Uri uri = new Uri("ftp://" + ServerFileHost + ":22" + fullFileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                //request.KeepAlive = false;
                //request.Timeout = -1;
                //request.UsePassive = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ServerFileHost.Username, ServerFileHost.Password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                        Console.WriteLine("Download Complete, status {0}", response.StatusDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to retrieve file: " + fullFileName);
                Console.WriteLine(ex.Message);
            }
        }

        public FileHost ServerFileHost { get; private set; }
    }
}
