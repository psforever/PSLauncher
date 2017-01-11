using PSNetCommon.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace PSNetCommon.Models
{
    public class FileCheckSumRequest
    {
        private const string SERVER_FILE = "validateChecksums.php";

        /// <summary>
        /// Web Request Parameter
        /// Contains a dictionary of all the client's filenames (keys) 
        /// and their base 64 checksum (value).
        /// </summary>
        public Dictionary<string, string> Files { get; private set; }

        public FileCheckSumRequest()
        {
            Files = new Dictionary<string, string>();
        }

        public void AddFile(string filename, string localChecksum)
        {
            Files.Add(filename, localChecksum);
        }

        public List<string> Send()
        {
            // TODO: Make this Async by remove the .Result's
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(Files);
                var response = client.PostAsync(Settings.Default.UpdateServerUrl + SERVER_FILE, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseString);
            }

            return new List<string>();
        }
    }
}
