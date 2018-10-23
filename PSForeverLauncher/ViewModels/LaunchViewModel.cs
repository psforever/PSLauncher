using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PSForeverLauncher.Framework.Messages;

namespace PSForeverLauncher.ViewModels
{
    public interface ILaunchViewModel : IScreen
    {
        void CheckForUpdates();
        void ServerList();
        void LaunchGame();
    }


    public class LaunchViewModel : Screen, ILaunchViewModel
    {
        public LaunchViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        /// <summary>
        /// Get Updates
        /// </summary>
        public void CheckForUpdates()
        {
            
        }

        /// <summary>
        /// Use current configuration and launch game
        /// </summary>
        public void LaunchGame()
        {

        }

        /// <summary>
        /// Lookup different servers available
        /// </summary>
        public void ServerList()
        {
            _eventAggregator.PublishOnUIThread(new OpenServerListMessage());

            //var check = _eventAggregator.HandlerExistsFor(typeof(OpenServerListMessage));

            //if (_eventAggregator.HandlerExistsFor(typeof(OpenServerListMessage)))
            //{
            //    _eventAggregator.BeginPublishOnUIThread(new OpenServerListMessage());
            //}
        }

        private readonly IEventAggregator _eventAggregator;
    }
}
