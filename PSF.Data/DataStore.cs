using System;
using System.Collections.Generic;
using System.Text;

namespace PSF.Data
{
    public static class DataStore
    {
        public static LauncherData LoadData()
        {
            return new LauncherData();
        }        
    }

    public class LauncherData
    {
        public string DataFile { get; internal set; }
        public DateTime CreatedDate { get; internal set; }

        internal LauncherData() { }
    }
}
