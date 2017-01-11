using System;
using System.IO;
using System.Security.Cryptography;

namespace PSNetCommon
{
    public static class CheckSum
    {
        public static string CalculateMD5(string filename)
        {
            using (var stream = File.OpenRead(filename)) 
            {
                using (var md5 = MD5.Create())
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
        }
    }
}
