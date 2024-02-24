using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class Menu
    {
        protected readonly IConsoleWrapper consoleWrapper;

        public Menu(IConsoleWrapper consoleWrapper)
        {
            this.consoleWrapper = consoleWrapper;
        }

        public virtual List<string> UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            var box1 = new BoxDrawer(new List<string>(), "LeftBox").DrawBox(consoleWrapper.WindowWidth / 2, consoleWrapper.WindowHeight - 3);
            var box2 = new BoxDrawer(new List<string>(), "RightBox").DrawBox(consoleWrapper.WindowWidth / 2, consoleWrapper.WindowHeight - 3);
            return new ScreenSelector(box1, box2, consoleWrapper).MergeSides();
        }
    }
}
