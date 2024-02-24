using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class CommitDetailsMenu : Menu
    {
        private readonly GitManager gitManager;
        private readonly FullCommits commits;
        private TextWidthSetter textWidthSetter;
        private List<IElement> page;
        private bool isRightSideInUse;
        private List<string> leftSide;

        public CommitDetailsMenu(
            IConsoleWrapper consoleWrapper,
            GitManager gitManager,
            FullCommits commits,
            bool isRightSideInUse) : base(consoleWrapper)
        {
            this.gitManager = gitManager;
            this.commits = commits;
            var windowHeight = consoleWrapper.WindowHeight;
            textWidthSetter = new TextWidthSetter(consoleWrapper);
            var windowWidth = consoleWrapper.WindowWidth;
            page = commits.GetPageDataAndSize(windowWidth * (windowHeight - 4), windowWidth, true).list;
            this.isRightSideInUse = isRightSideInUse;
            leftSide = new List<string>();
        }

        public override List<string> UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            if (isWindowToSmall)
            {
                return new List<string>();
            }

            textWidthSetter = new TextWidthSetter(consoleWrapper);
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var leftSideWidth = textWidthSetter.SetTextWidth(true);
            var keyInteractor = new KeyInteractor<IElement>(commits);
            keyInteractor.KeyInfo = key;
            (commits.Index, commits.Current) = keyInteractor
                .KeySwitch(leftSide, page[^1].Id, page[0].Id, false, consoleWrapper.WindowWidth, commits[commits.Current].GetElement().Length - 41);
            page = commits.GetPageDataAndSize(windowWidth * (windowHeight - 4), windowWidth, true).list;
            leftSide = commits.GetHighlightedSide(windowHeight - 4, leftSideWidth, leftSideWidth * (windowHeight - 4), true);
            var rightSide = commits.FormateUnhighlightedSide(windowHeight - 2, textWidthSetter.SetTextWidth(false), gitManager);
            var leftBox = new BoxDrawer(leftSide, "Commits").DrawBox(windowHeight < 15 && windowWidth < 50 ? windowWidth : leftSideWidth + 2, windowHeight - 2);
            const string menuBar = "\u001b[32m[Esc][←] Return \u001b[33m[→] Next menu \u001b[36m[B] Branches \u001b[0m";
            leftBox.Add(menuBar + new string(' ', windowWidth - menuBar.Length + 19));
            return new ScreenSelector(
                    leftBox,
                    rightSide,
                    consoleWrapper).Select(ref isRightSideInUse, key);
        }
    }
}
