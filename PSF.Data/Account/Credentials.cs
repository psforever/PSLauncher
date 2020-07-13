using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using PSF.Data.Attributes;
using PSF.Data.Server;
using PSF.Data.Extensions;

namespace PSF.Data.Account
{
    /// <summary>
    /// Class that serves the purpose of tying credentials
    /// and only stores active account information that is
    /// returned or chosen to be stored by the launcher
    /// </summary>
    public class Credentials
    {
        public Credentials(string username, string password, PSFServer server)
        {
            this.Username = username;
            this.Password = password;

            InitCryptography();
        }

        private void InitCryptography()
        {
            // WIP
        }
        
        public PSFServer Server { get; set; }
        /// <summary>
        /// Account username in plaintext
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// <para>Account password "encrypted".</para>
        /// <list type="bullet">
        ///     <item>Uses AES</item>
        ///     <item>Not to guarantee security</item>
        /// </list>
        /// </summary>
        public string EncryptedPassword { get; set; }
        /// <summary>
        /// Account password in plaintext
        /// </summary>
        [SecureProperty]
        public string Password { get; set; }
    }
}
