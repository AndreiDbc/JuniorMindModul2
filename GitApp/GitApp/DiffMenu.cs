using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitApp
{
    public class DiffMenu : Menu
    {
        private readonly TextWidthSetter textWidthSetter;
        private readonly ModifiedFiles modifiedFiles;
        private readonly GitManager gitManager;
        private readonly IElement previousCommit;
        private readonly IElement currentCommit;
        bool isRightSideInUse;
        private Diff diff;
        private List<IElement> page;
        private List<string> leftSide;
        private int width;

        public DiffMenu(
            IConsoleWrapper consoleWrapper,
            ModifiedFiles modifiedFiles,
            Diff diff,
            IElement previousCommit,
            IElement currentCommit,
            GitManager gitManager,
            bool isRightSideInUse) : base(consoleWrapper)
        {
            this.isRightSideInUse = isRightSideInUse;
            this.modifiedFiles = modifiedFiles;
            this.gitManager = gitManager;
            this.previousCommit = previousCommit;
            this.currentCommit = currentCommit;
            this.diff = diff;
            textWidthSetter = new TextWidthSetter(consoleWrapper);
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            width = windowWidth < 50 && windowHeight < 15 ? windowWidth : textWidthSetter.SetTextWidth(true);
            page = modifiedFiles.GetPageDataAndSize(width * (consoleWrapper.WindowHeight - 4), width, true).list;
            leftSide = new List<string>();
        }

        public override void UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            if (isWindowToSmall)
            {
                return;
            }

            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            width = windowWidth < 50 && windowHeight < 15 ? windowWidth : textWidthSetter.SetTextWidth(true);
            var rightSideWidth = textWidthSetter.SetTextWidth(false);
            var diffAvailableSpace = rightSideWidth * (windowHeight - 4);
            var keyInteractor = new KeyInteractor<IElement>(modifiedFiles);
            keyInteractor.KeyInfo = key;
            (modifiedFiles.Index, modifiedFiles.Current) = keyInteractor.KeySwitch(leftSide, page[^1].Id, page[0].Id, false, windowWidth, page[^1].GetElement().Length);
            page = modifiedFiles.GetPageDataAndSize(width * (windowHeight - 4), width, true).list;
            leftSide = modifiedFiles.GetHighlightedSide(windowHeight - 4, width, width * (windowHeight - 4), true);
            diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, modifiedFiles[modifiedFiles.Current].GetElement()));
            var rightSide = windowWidth < 50 && windowHeight < 15
                ? diff.GetHighlightedSide(windowHeight - 4, windowWidth - 2, (windowHeight - 3) * (windowWidth - 2), false)
                : diff.GetHighlightedSide(windowHeight - 4, windowWidth - (int)(0.6 * windowWidth) - 2, diffAvailableSpace, false);
            var result = new List<string>();
            result.AddRange(leftSide);
            result = new BoxDrawer(result, "Files").DrawBox(width + 2, windowHeight - 2);
            rightSide = new BoxDrawer(rightSide, "Diff").DrawBox(textWidthSetter.SetTextWidth(false) + 2, windowHeight - 2);
            result.Add("\u001b[32m[Esc][←] Return \u001b[33m[→] Next menu \u001b[36m[B] Branches \u001b[0m");
            LastPageDisplayed = string.Join(Environment.NewLine, new ScreenSelector(result, rightSide, consoleWrapper).Select(ref isRightSideInUse, key));
        }
    }
}
