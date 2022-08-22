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

        public string ReadText(bool gzip, bool crypto, byte[] key = null, byte[] iv = null)
        {
            if (gzip)
            {
                stream = new GZipStream(stream, CompressionMode.Decompress, true);
            }

            if (crypto)
            {
                using Aes aes = Aes.Create();
                stream = new CryptoStream(stream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read, true);
            }

            using StreamReader reader = new (stream);
            return reader.ReadToEnd();
        }
    }
}
