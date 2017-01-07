using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

using Newtonsoft.Json;
using PSNetCommon;
using PSLauncher.ViewModels;
using System.ComponentModel;

namespace PSLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel = new MainViewModel();

        public MainWindow()
        {
            this.DataContext = _viewModel;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
            InitializeComponent();
        }

        private void _viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Progress")
            {
                Application.Current.Dispatcher.Invoke(new Action(() => 
                    _viewModel_UpdateProgress(_viewModel.Progress)));
            }
        }

        /// <summary>
        /// Method used to update progress bar with animation.
        /// </summary>
        /// <param name="value">float</param>
        private void _viewModel_UpdateProgress(int progress)
        {
            if (progress > 0)
            {
                prgBar.Visibility = Visibility.Visible;

                var duration = new Duration(TimeSpan.FromSeconds(0.1));
                var doubleAnimation = new DoubleAnimation(progress, duration);
                prgBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);

                if (progress == 100)
                {
                    playBtn.IsEnabled = true;
                }
            }
            else
            {
                prgBar.Visibility = Visibility.Hidden;
            }
            
        }

        /// <summary>
        /// Progress Bar Value Changed Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prgBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            prgLbl.Content = (int)(prgBar.Value) + " / 100";
            if (prgBar.Value > 50)
            {
                prgLbl.Foreground = Brushes.Black;
            }

            if (prgBar.Value == 100)
                prgLbl.Content = "Complete";
        }

        /// <summary>
        /// Play Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RunPlanetsideExe();
        }

        /// <summary>
        /// Window Loaded Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.ViewLoaded();
        }

        /// <summary>
        /// Change Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeBtn_Click(object sender, RoutedEventArgs e)
        {
            var changeServers = new ChangeServers(this);    // Pass the current instance as the MainWindow to be used.
            changeServers.Show();
        }

        /// <summary>
        /// Refresh Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshConnectionJSON();
        }

        /// <summary>
        /// Account Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.AccountURL);
        }

        /// <summary>
        /// Logo Mouse Press Event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*
             * This Message Box was originally done by the clever mook, known on Github to the PSForever project as PSmook.
             * This message box has been preserved to remember his original work on the Winform version of the launcher that
             * was used before being rewritten.
             */
            MessageBox.Show(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String("Q3JlYXRlZCBieSBtb29rIChwc21vb2sp")));
        }

        // HACK: This should not be in the View class.
        public void RefreshConnectionJSON()
        {
            _viewModel.RefreshConnectionJSON();
        }
    }
}