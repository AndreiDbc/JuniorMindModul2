using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class ScreenSelector
    {
        private readonly List<string> leftSide;
        private readonly List<string> rightSide;
        private readonly IConsoleWrapper consoleWrapper;

        public ScreenSelector(List<string> first, List<string> second, IConsoleWrapper consoleWrapper)
        {
            this.leftSide = first;
            this.rightSide = second;
            this.consoleWrapper = consoleWrapper;
        }

        public List<string> Select(ref bool isRightSideInUse, KeyInteractor<IElement> keyInteractor)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            if (height < 15 && width < 50)
            {
                return SelectSide(ref isRightSideInUse, keyInteractor);
            }

            return MergeSides();
        }

        public List<string> MergeSides()
        {
            var result = new List<string>();

            for (int i = 0; i < leftSide.Count; i++)
            {
                result.Add(leftSide[i]);
                if (i < rightSide.Count)
                {
                    result[i] += rightSide[i];
                }
            }

            return result;
        }

        private List<string> SelectSide(ref bool isRightSideInUse, KeyInteractor<IElement> keyInteractor)
        {
            isRightSideInUse = keyInteractor.KeyInfo.Equals(ConsoleKey.Spacebar) ? !isRightSideInUse : isRightSideInUse;
            return isRightSideInUse ? rightSide : leftSide;
        }
    }
}
