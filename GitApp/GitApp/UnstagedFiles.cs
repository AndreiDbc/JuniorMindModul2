using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class UnstagedFiles : ModifiedFiles
    {
        public UnstagedFiles(string unstagedRawData) : base(unstagedRawData)
        {
        }

        protected override List<IElement> GetElements()
        {
            var result = new List<IElement>();
            var files = rawData.Split("\n");
            for (int i = 0; i < files.Length; i++)
            {
                var item = files[i];
                if (item.StartsWith("AD") || item.StartsWith("M") || item.StartsWith("D"))
                {
                    continue;
                }

                result.Add(new File(item, i.ToString()));
            }

            return result;
        }
    }
}
