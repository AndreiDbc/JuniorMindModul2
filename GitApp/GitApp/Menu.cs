using System;
using System.Collections.Generic;
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
            return new List<string>();
        }
    }
}
