using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSForeverLauncher.Framework.Messages;

namespace PSForeverLauncher.ViewModels
{
    public interface IServerListViewModel : IScreen
    {
        void Return();
    }

    public class ServerListViewModel : Screen, IServerListViewModel
    {
        public IEventAggregator EventAggregator { get; set; }

        public ServerListViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }


        public void Return()
        {
            _eventAggregator.PublishOnUIThread(new OpenLaunchViewMessage());
        }

        private readonly IEventAggregator _eventAggregator;
    }
}
