using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace PSF.Data.Attributes
{
    /// <summary>
    /// This means secure in air-quotes.
    /// TODO: WORK IN PROGRESS
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SecureProperty : Attribute
    {
        private static ICryptoTransform _encryptoTransform;
        private static ICryptoTransform _decryptoTransform;

        public SecureProperty()
        {
            var info = typeof(SecureProperty).GetTypeInfo();

            Debug.WriteLine($"Base is {info.BaseType}");
        }

        static SecureProperty()
        {
            SecureProperties = new Dictionary<string, string>();

            var entropy = new byte[512];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);

                using (var aes = Aes.Create("AES"))
                {
                    aes.Key = Encoding.ASCII.GetBytes($"psforever");

                    _encryptoTransform = aes.CreateEncryptor();
                    _decryptoTransform = aes.CreateDecryptor();
                }
            }
        }

        static string Encrypt(string value)
        {
            var plaintext = Encoding.ASCII.GetBytes(value);

            var encryptBlocks = _encryptoTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Encoding.ASCII.GetString(encryptBlocks);
        }

        static string Decrypt(string value)
        {
            var encrypted = Encoding.ASCII.GetBytes(value);

            var decryptBlocks = _decryptoTransform.TransformFinalBlock(encrypted, 0, encrypted.Length);

            return Encoding.ASCII.GetString(decryptBlocks);
        }

        internal static string GetValueFromSecureProperty(string name)
        {
            return SecureProperties[name];
        }

        /// <summary>
        /// Contains the plaintext data for attributes
        /// </summary>
        private static Dictionary<string, string> SecureProperties { get; set; }
    }
}
