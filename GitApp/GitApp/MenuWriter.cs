using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class MenuWriter
    {
        private readonly List<string> toWrite;
        private readonly bool isWindowToSmall;
        private readonly IConsoleWrapper consoleWrapper;

        public MenuWriter(List<string> toWrite, bool isWindowToSmall, IConsoleWrapper consoleWrapper)
        {
            this.toWrite = toWrite;
            this.isWindowToSmall = isWindowToSmall;
            this.consoleWrapper = consoleWrapper;
        }

        public bool UpdateDisplay(bool wasWindowResizedSmaller, List<string>? overlayMenu = null)
        {
            var result = wasWindowResizedSmaller;
            if (isWindowToSmall)
            {
                consoleWrapper.Clear();
                consoleWrapper.Write("Increase window size!");
            }
            else
            {
                var paddedPage = new PageUtilities(toWrite.ToArray()).PadElement(consoleWrapper.WindowWidth);
                paddedPage.Insert(0, "\u001b[0mStatus [1] | Log [2]");
                if (wasWindowResizedSmaller)
                {
                    result = false;
                    consoleWrapper.Clear();
                }

                consoleWrapper.SetCursorPosition(0, 0);
                Write(paddedPage);

                if (overlayMenu != null)
                {
                    var width = consoleWrapper.WindowWidth;
                    var height = consoleWrapper.WindowHeight;
                    var left = (int)(0.125 * width);
                    var top = (int)(0.125 * height);
                    WriteOverlay(overlayMenu, ref left, ref top);
                }
            }

            return result;
        }

        private void Write(List<string> paddedPage)
        {
            var result = string.Join("\n", paddedPage);
            consoleWrapper.Write(result);
        }

        private void WriteOverlay(List<string> overlayPage, ref int left, ref int top)
        {
            var width = consoleWrapper.WindowWidth;
            var height = consoleWrapper.WindowHeight;

            if (height < 15 && width < 60)
            {
                left = 0;
                top = 0;
            }

            foreach (var line in overlayPage)
            {
                consoleWrapper.SetCursorPosition(left, top);
                consoleWrapper.Write(line);
                top++;
            }
        }
    }
}