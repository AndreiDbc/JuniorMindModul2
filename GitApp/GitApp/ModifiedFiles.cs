using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class ModifiedFiles : IInteractor<IElement>
    {
        private readonly string rawData;
        private readonly List<IElement> list;

        public ModifiedFiles(string rawData)
        {
            this.rawData = rawData;
            list = GetText();
            Current = 0;
            Index = 0;
        }

        public int Current { get; set; }

        public int Index { get; set; }

        public IElement this[int index]
        {
            get
            {
                return list[index];
            }
        }

        public List<IElement> GetText()
        {
            var result = new List<IElement>();
            foreach (var item in rawData.Split("\n"))
            {
                result.Add(new File(item));
            }

            return result;
        }
    }
}
