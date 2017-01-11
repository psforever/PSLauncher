using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSNetCommon.Models
{
    public class FileCheckSumResponse
    {
        /// <summary>
        /// Web Response Data
        /// Contains all filenames that need to be updated.
        /// </summary>
        public List<string> FilesToUpdate { get; set; }

        /// <summary>
        /// Web Response Data
        /// Contains all filenames that should be removed completely.
        /// </summary>
        public List<string> FilesToDelete { get; set; }
    }
}
