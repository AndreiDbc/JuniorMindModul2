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
        readonly Stream stream;

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
                    using (CryptoStream cryptoStream = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read, true))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

            using (StreamReader reader = gzip
                ? new (new GZipStream(stream, CompressionMode.Decompress, true), leaveOpen: true)
                : new (stream, leaveOpen: true))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
