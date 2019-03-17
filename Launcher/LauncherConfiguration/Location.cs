using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.LauncherConfiguration
{
    /// <summary>
    /// Get the user's location for application configuration
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Returns Local Application Data
        /// </summary>
        /// <returns>File Path</returns>
        public string GetAppData()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}
