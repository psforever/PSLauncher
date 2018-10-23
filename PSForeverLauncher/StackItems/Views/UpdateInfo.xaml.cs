using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PSForeverLauncher.StackItems
{
    /// <summary>
    /// Interaction logic for UpdateInfo.xaml
    /// </summary>
    public partial class UpdateInfo : UserControl
    {
        private DateTime _timeStamp;

        public UpdateInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Only has to be set at runtime for the moment
        /// </summary>
        public string Title
        {
            get { return UpdateTitle.Text; }
            set
            {
                UpdateTitle.Text = value;
            }
        }

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                _timeStamp = value;
                UpdateTime.Text = _timeStamp.ToShortDateString();
            }
        }
    }
}
