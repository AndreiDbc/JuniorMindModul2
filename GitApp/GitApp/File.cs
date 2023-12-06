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
        private readonly string id;

        public File(string rawFileDetails, string id)
        {
            this.rawFileDetails = rawFileDetails;
            this.id = id;
        }

        public string Id
        {
            get => id;
        }

        public string GetElement()
        {
            var result = rawFileDetails;
            if (rawFileDetails.Contains(" ") && !result.StartsWith("*"))
            {
                var temp = rawFileDetails.Split(" ");
                result = temp.Length == 3 ? temp[2] : temp[1];
            }

            return result;
        }
    }
}
