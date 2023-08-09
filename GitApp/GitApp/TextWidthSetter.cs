using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class TextWidthSetter
    {
        private readonly IConsoleWrapper consoleWrapper;

        public TextWidthSetter(IConsoleWrapper consoleWrapper)
        {
            this.consoleWrapper = consoleWrapper;
        }

        public int SetTextWidth(bool forLeftSide)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var result = forLeftSide ? (int)(0.6 * windowWidth) - 2 : windowWidth - (int)(0.6 * windowWidth) - 2;
            var windowHeight = consoleWrapper.WindowHeight;
            return windowWidth < 50 && windowHeight < 15
                ? windowWidth - 2
                : result;
        }
    }
}
