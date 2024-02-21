using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class LogMenu : Menu
    {
        private readonly FullCommits commits;
        private int commitOcupancyRate;
        private List<IElement> page;

        public LogMenu(IConsoleWrapper consoleWrapper, FullCommits commits) : base(consoleWrapper)
        {
            this.commits = commits;
            page = new List<IElement>();
        }

        public override List<string> UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            if (isWindowToSmall)
            {
                return new List<string>();
            }

            var result = new List<string>();
            var keyInteractor = new KeyInteractor<IElement>(commits);
            keyInteractor.KeyInfo = key;
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var availableSpace = (windowHeight - 4) * (windowWidth - 2);
            var isPageLargerThanWindow = commitOcupancyRate > availableSpace;
            var commitsToString = new List<string>();
            page.ForEach(line =>
            {
                var commitDetails = line.GetElement().Split("\n");
                commitsToString.Add(commitDetails[0] + " " + commitDetails[1] + " " + commitDetails[2] + " " + commitDetails[3]);
            });
            (commits.Index, commits.Current) = page.Count == 0
                ? (0, 0)
                : keyInteractor.KeySwitch(commitsToString, page[^1].Id, page[0].Id, isPageLargerThanWindow, windowWidth - 2, commits[commits.Current].GetElement().Length - 41);
            (page, commitOcupancyRate) = commits.GetPageDataAndSize(availableSpace, windowWidth - 2);
            var toWrite = commits.GetHighlightedSide(windowHeight - 4, windowWidth - 2, availableSpace);
            result.AddRange(new BoxDrawer(toWrite, "Commits").DrawBox(windowWidth, windowHeight - 2, true));
            const string menuBar = "\u001b[32m[Esc] Exit \u001b[33m[⏎] Details \u001b[36m[B] Branches\u001b[0m";
            result.Add(menuBar + new string(' ', windowWidth - menuBar.Length + 19));

            return result;
        }
    }
}
