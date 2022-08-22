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
    class Writer
    {
        Stream stream;

        public Writer(Stream stream)
        {
            this.stream = stream;
        }

        public void WriteText(string text, bool gzip, bool crypto, byte[] key = null, byte[] iv = null)
        {
            if (gzip)
            {
                stream = new GZipStream(stream, CompressionLevel.SmallestSize, true);
            }

            if (crypto)
            {
                using Aes aes = Aes.Create();
                stream = new CryptoStream(stream, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write, true);
            }

            using var writer = new StreamWriter(stream);
            writer.Write(text);
        }
    }
}
