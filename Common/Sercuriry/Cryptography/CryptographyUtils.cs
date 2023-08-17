using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Common.Sercuriry.Cryptography
{
    public static class CryptographyUtils
    {
        public static string SHA1(Stream buffer)
        {
            using (var cryptoProvider = new SHA1CryptoServiceProvider())
            {
                return BitConverter
                        .ToString(cryptoProvider.ComputeHash(buffer));
            }
        }

        public static string SHA1(string filePath)
        {
            return SHA1(File.OpenRead(filePath));
        }
    }
}
