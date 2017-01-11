using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSLauncher.Interfaces
{
    /// <summary>
    /// Implement this Interface for views that contain some way of displaying
    /// Progress to the user.
    /// </summary>
    interface IProgressView
    {
        /// <summary>
        /// Valid values are 0-100 inclusive to represent the percentage of
        /// progress the task has completed.
        /// </summary>
        int Progress { get; set; }

        /// <summary>
        /// Information to display to the user regarding the progress.
        /// </summary>
        string ProgressInfo { get; set; }
    }
}
