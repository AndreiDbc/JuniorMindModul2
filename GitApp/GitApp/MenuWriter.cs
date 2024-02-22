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

        public void UpdateDisplay(bool wasWindowResizedSmaller, List<string>? overlayMenu = null)
        {
            if (isWindowToSmall)
            {
                consoleWrapper.Clear();
                consoleWrapper.Write("Increase window size!");
            }
            else
            {
                var windowWidth = consoleWrapper.WindowWidth;
                var menuBar = "\u001b[0mStatus [1] | Log [2]" + new string(' ', windowWidth - "\u001b[0mStatus [1] | Log [2]".Length + 4);
                toWrite.Insert(0, menuBar);
                if (wasWindowResizedSmaller)
                {
                    consoleWrapper.Clear();
                }

                consoleWrapper.SetCursorPosition(0, 0);
                Write(toWrite);
                consoleWrapper.Flush();
                if (overlayMenu != null)
                {
                    var width = consoleWrapper.WindowWidth;
                    var height = consoleWrapper.WindowHeight;
                    var left = (int)(0.125 * width);
                    var top = (int)(0.125 * height);
                    WriteOverlay(overlayMenu, ref left, ref top);
                    consoleWrapper.Flush();
                }

                consoleWrapper.SetCursorPosition(windowWidth, consoleWrapper.WindowHeight);
            }
        }

        private void Write(List<string> paddedPage)
        {
            var result = string.Join(Environment.NewLine, paddedPage);
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