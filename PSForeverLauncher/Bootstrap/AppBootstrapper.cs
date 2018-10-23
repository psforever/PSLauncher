using Caliburn.Micro;
using PSForeverLauncher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PSForeverLauncher.Bootstrap
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override void Configure()
        {
            base.Configure();
            _simpleContainer.Singleton<IWindowManager, WindowManager>();
            _simpleContainer.Singleton<IEventAggregator, EventAggregator>();
            _simpleContainer.RegisterPerRequest(typeof(MainWindowViewModel), null, typeof(MainWindowViewModel));
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return _simpleContainer.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _simpleContainer.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _simpleContainer.BuildUp(instance);
        }

        private readonly SimpleContainer _simpleContainer = new SimpleContainer();
    }
}
