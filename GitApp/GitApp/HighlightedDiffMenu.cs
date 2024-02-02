using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class HighlightedDiffMenu : Menu
    {
        private readonly TextWidthSetter textWidthSetter;
        private readonly Diff diff;
        private readonly ModifiedFiles modifiedFiles;
        private List<IElement> diffPage;
        private List<string> rightSide;
        private int diffSize;

        public HighlightedDiffMenu(IConsoleWrapper consoleWrapper, Diff diff, ModifiedFiles modifiedFiles) : base(consoleWrapper)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var shouldDisplayOnWholeWindow = windowWidth < 50 && windowHeight < 15;
            textWidthSetter = new TextWidthSetter(consoleWrapper);
            var diffWidth = shouldDisplayOnWholeWindow ? windowWidth : textWidthSetter.SetTextWidth(false);
            var availableSpace = diffWidth * (windowHeight - 4);
            this.modifiedFiles = modifiedFiles;
            this.diff = diff;
            rightSide = new List<string>();
            (diffPage, diffSize) = diff.GetPageDataAndSize(availableSpace, diffWidth, true);
        }

        public override void UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            if (isWindowToSmall)
            {
                return;
            }

            var keyInteractor = new KeyInteractor<IElement>(diff);
            keyInteractor.KeyInfo = key;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var shouldDisplayOnWholeWindow = windowWidth < 50 && windowHeight < 15;
            var diffWidth = shouldDisplayOnWholeWindow ? windowWidth : textWidthSetter.SetTextWidth(false);
            var filesWidth = shouldDisplayOnWholeWindow ? windowWidth : textWidthSetter.SetTextWidth(true);
            var availableSpace = diffWidth * (windowHeight - 4);
            (diff.Index, diff.Current) = keyInteractor.KeySwitch(rightSide, diffPage[^1].Id, diffPage[0].Id, diffSize > availableSpace, diffWidth - 2, diff[diff.Current].GetElement().Length);
            (diffPage, diffSize) = diff.GetPageDataAndSize(availableSpace, diffWidth, true);
            rightSide = diff.GetHighlightedSide(windowHeight - 4, diffWidth, availableSpace, true);
            var rightSideTemp = new BoxDrawer(rightSide, "Diff").DrawBox(diffWidth + 2, windowHeight - 2, false, true);
            var leftSide = new BoxDrawer(
                modifiedFiles.GetHighlightedSide(windowHeight - 4, filesWidth, (windowHeight - 4) * filesWidth, true),
                "Files").DrawBox((int)(0.6 * windowWidth), windowHeight - 2);
            leftSide.Add("\u001b[32m[Esc][←] Return\u001b[0m \u001b[36m[B] Branches");
            LastPageDisplayed = string.Join(Environment.NewLine, new ScreenSelector(leftSide, rightSideTemp, consoleWrapper).MergeSides());
        }
    }
}
