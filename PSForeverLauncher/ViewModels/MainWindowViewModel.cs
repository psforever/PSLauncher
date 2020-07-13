using PSF.Data.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSForeverLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => $"Welcome to Avalonia! Now using {creds.EncryptedPassword}";
        private Credentials creds = new Credentials("Test", "Test", null);
    }
}
