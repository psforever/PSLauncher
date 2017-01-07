using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSNetCommon.Models
{
    public class FileCheckSumRequest
    {
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
    }
}
