using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public interface IInteractor<T>
        where T : IElement
    {
        public int Current { get; set; }

        public int Index { get; set; }

        T this[int index] { get; }

        public List<T> GetText();

        public (List<IElement> list, int textSize) GetPageDataAndSize(int availableSpace, int lineSize, bool isScreenSplit = false);

        public List<string> GetLeftSide(int height, int width, int availableSpace, bool isScreenSplit = false);
    }
}
