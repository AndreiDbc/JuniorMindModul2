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

        public List<string> Select(ref bool isRightSideInUse, ConsoleKey key)
        {
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            if (height < 15 && width < 50)
            {
                return SelectSide(ref isRightSideInUse, key);
            }

            return MergeSides();
        }

        public List<string> MergeSides()
        {
            var result = leftSide.Zip(rightSide, (x, y) => x + y).ToList();
            result.Add(leftSide[^1]);
            return result;
        }

        private List<string> SelectSide(ref bool isRightSideInUse, ConsoleKey key)
        {
            isRightSideInUse = key.Equals(ConsoleKey.Spacebar) ? !isRightSideInUse : isRightSideInUse;
            return isRightSideInUse ? rightSide : leftSide;
        }
    }
}
