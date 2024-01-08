using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitMenu : Menu
    {
        public CommitMenu(IConsoleWrapper consoleWrapper) : base(consoleWrapper)
        {
        }

        public override List<string> UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            const string commitMenu = "\u001b[37m'Esc' Return \u001b[38[←] Commit";
            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var isWindowSmall = height < 15 && width < 60;
            if (!isWindowSmall)
            {
                height = (int)(height * 0.75 - 2);
                width = (int)(width * 0.75 - 2);
            }

            return new BoxDrawer(new List<string>(), "Commit").DrawBox(width, height, false, true, "", commitMenu);
        }
    }
}
