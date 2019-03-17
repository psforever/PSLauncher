using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Launcher.GameCheck
{
    public class File
    {
        private readonly string _fileHash;

        public File(string filename)
        {
            FileName = filename;

            _fileHash = GetMD5Hash();
        }

        /// <summary>
        /// Basic MD5 hashing of files
        /// </summary>
        /// <returns></returns>
        private string GetMD5Hash()
        {
            using (var md5 = MD5.Create())
            {
                using (var fileStream = System.IO.File.OpenRead(FileName))
                {
                    var md5hash = md5.ComputeHash(fileStream);

                    return Encoding.Default.GetString(md5hash);
                }
            }
        }

        public string FileHash => _fileHash;
        public string FileName { get; set; }
    }
}
