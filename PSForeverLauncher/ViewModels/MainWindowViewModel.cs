using Caliburn.Micro;
using PSForeverLauncher.Framework.Messages;

namespace PSForeverLauncher.ViewModels
{
    public interface IMainWindowViewModel : IScreen
    {
        void OpenNotice();
        void OpenAbout();
        void Exit();
        IEventAggregator EventAggregator { get; set; }
    }

    public class MainWindowViewModel : Conductor<Screen>.Collection.OneActive, IMainWindowViewModel, IHandle<OpenServerListMessage>, IHandle<OpenLaunchViewMessage>
    {
        public IEventAggregator EventAggregator { get; set; }
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            var launchViewModel = new LaunchViewModel(eventAggregator);
            this.ActivateItem(launchViewModel);

            launchViewModel.ConductWith(this);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            EventAggregator.Subscribe(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenAbout()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenNotice()
        {

        }

        /// <summary>
        /// Exit the application
        /// </summary>
        public void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void Handle(OpenServerListMessage message)
        {
            var serverListViewModel = new ServerListViewModel(EventAggregator);
            this.ActivateItem(serverListViewModel);
        }

        public void Handle(OpenLaunchViewMessage message)
        {
            var launchViewModel = new LaunchViewModel(EventAggregator);
            this.ActivateItem(launchViewModel);
        }

        public string WindowTitle { get; set; }
    }
}
