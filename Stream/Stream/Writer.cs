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
        readonly Stream stream;

        public Writer(Stream stream)
        {
            this.stream = stream;
        }

        public void WriteText(string text, bool gzip, bool crypto)
        {
            if (crypto)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = new byte[16];
                    aes.IV = new byte[16];
                    byte[] data = ASCIIEncoding.ASCII.GetBytes(text);
                    using (CryptoStream cryptoStream = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        return;
                    }
                }
            }

            using (StreamWriter writer = gzip
                ? new (new GZipStream(stream, CompressionLevel.SmallestSize, true), leaveOpen: true)
                : new (stream, leaveOpen: true))
            {
                writer.Write(text);
            }
        }
    }
}
