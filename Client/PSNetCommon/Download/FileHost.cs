using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSNetCommon.Download
{
    /// <summary>
    /// Beginning Efforts to have multiple file hosts for game servers
    /// </summary>
    public class FileHost
    {
        public FileHost(string server, string username, string password = "")
        {
            Server = server;
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return Server;
        }

        public string Server { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
