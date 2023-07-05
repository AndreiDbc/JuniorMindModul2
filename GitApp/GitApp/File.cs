using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class File : IElement
    {
        private readonly string rawFileDetails;

        public File(string rawFileDetails)
        {
            this.rawFileDetails = rawFileDetails;
        }

        public string Id
        {
            get => GetId();
        }

        public string GetElement()
        {
            return rawFileDetails;
        }

        private string GetId()
        {
            var result = new char[0];
            var temp = rawFileDetails.ToCharArray();
            for (int i = temp.Length - 1, j = 0; i >= 0; i--, j++)
            {
                Array.Resize(ref result, result.Length + 1);
                result[j] += rawFileDetails[i];
                if (rawFileDetails[i] == '/')
                {
                    break;
                }
            }

            Array.Reverse(result);
            return new string(result);
        }
    }
}
