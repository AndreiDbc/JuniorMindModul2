using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Str
{
    class Reader
    {
        Stream stream;

        public Reader(Stream stream)
        {
            this.stream = stream;
        }

        public string ReadText(bool gzip, bool crypto)
        {
            if (crypto)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = new byte[16];
                    aes.IV = new byte[16];
                    stream = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read, true);
                }
            }

            if (gzip)
            {
                stream = new GZipStream(stream, CompressionMode.Decompress, true);
            }

            using (StreamReader reader = new (stream, leaveOpen: true))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
