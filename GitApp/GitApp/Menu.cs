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
            LastPageDisplayed = "";
        }

        public string LastPageDisplayed { get; protected set; }

        public bool WasInteractorModified { get; protected set; }

        public virtual void UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
        }
    }
}
