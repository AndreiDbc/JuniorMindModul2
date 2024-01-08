using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class HighlightedHunksMenu : Menu
    {
        private readonly GitManager gitManager;
        private readonly TextWidthSetter textWidthSetter;
        private readonly bool unstagedAvailability;
        private List<IElement> page;
        private bool isRightSideInUse;
        private List<string> diffToString;
        private int pageSize;

        public HighlightedHunksMenu(
            IConsoleWrapper consoleWrapper,
            GitManager gitManager,
            Diff diff,
            bool unstagedAvailability,
            bool isRightSideInUse) : base(consoleWrapper)
        {
            this.gitManager = gitManager;
            this.unstagedAvailability = unstagedAvailability;
            this.isRightSideInUse = isRightSideInUse;
            textWidthSetter = new TextWidthSetter(consoleWrapper);
            var windowHeight = consoleWrapper.WindowHeight;
            var rightSideWidth = textWidthSetter.SetTextWidth(false);
            var availableSpace = rightSideWidth * (windowHeight - 4);
            page = diff.GetPageDataAndSize(availableSpace, rightSideWidth, true).list;
            diffToString = diff[diff.Current].GetElement() == ""
                ? new List<string>()
                : diff.GetHighlightedSide(windowHeight - 4, rightSideWidth, availableSpace, true);
            pageSize = 0;
        }

        public ModifiedFiles Unstaged { get; set; }

        public ModifiedFiles Staged { get; set; }

        public Diff Diff { get; set; }

        public override List<string> UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            var keyInteractor = new KeyInteractor<IElement>(Diff);
            keyInteractor.KeyInfo = key;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var rightSideWidth = textWidthSetter.SetTextWidth(false);
            var availableSpace = rightSideWidth * (windowHeight - 4);
            (Diff.Index, Diff.Current) = keyInteractor.KeySwitch(diffToString, page[^1].Id, page[0].Id, pageSize > availableSpace, rightSideWidth, Diff[Diff.Current].GetElement().Length);
            (page, pageSize) = Diff.GetPageDataAndSize(availableSpace, rightSideWidth, true);
            diffToString = Diff[Diff.Current].GetElement() == ""
                ? new List<string>()
                : Diff.GetHighlightedSide(windowHeight - 4, rightSideWidth, availableSpace, true);
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            List<string> rightSide;
            List<string> leftSide;
            var branch = gitManager.GetCurrentGitBranch();
            var noOfCommitsToPull = gitManager.GetNumberOfCommitsToPull(branch);
            var noOfCommitsToPush = gitManager.GetNumberOfCommitsToPush(branch);
            var shouldDisplayNoOfCommits = noOfCommitsToPull != "" && noOfCommitsToPush != "";
            if (windowHeight < 15 && windowWidth < 50)
            {
                rightSide = new BoxDrawer(diffToString, "Diff").DrawBox(windowWidth, windowHeight - 2, false, true);

                var unstagedToString = Unstaged.GetHighlightedSide(unstagedHeight, windowWidth - 2, (unstagedHeight - 3) * (windowWidth - 2));
                var stagedToString = Staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 2) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                if (unstagedAvailability)
                {
                    stagedToString[0] = stagedToString[0].Replace("\u001b[7m", "");
                    stagedToString[^1] = stagedToString[^1].Replace("\u001b[7m", "");
                }
                else
                {
                    unstagedToString[0] = unstagedToString[0].Replace("\u001b[7m", "");
                    unstagedToString[^1] = unstagedToString[^1].Replace("\u001b[7m", "");
                }

                leftSide = new BoxDrawer(unstagedToString, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}");
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox(windowWidth, windowHeight - unstagedHeight - 4, true);
                leftSide.AddRange(stagedToString);
            }
            else
            {
                var textwidth = new TextWidthSetter(consoleWrapper);
                var leftSideWidth = textwidth.SetTextWidth(true);
                rightSide = new BoxDrawer(diffToString, "Diff").DrawBox(textwidth.SetTextWidth(false) + 2, windowHeight - 2, false, true);

                var unstagedToString = Unstaged.GetHighlightedSide(unstagedHeight, leftSideWidth, unstagedAvailableSpace);
                var stagedToString = Staged.GetHighlightedSide(
                        windowHeight - ((windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2) - 2,
                        (int)(0.6 * windowWidth) - 2,
                        ((windowHeight - 2) / 2 - 2) * (int)(0.6 * windowWidth) - 2);
                if (unstagedAvailability)
                {
                    stagedToString[0] = stagedToString[0].Replace("\u001b[7m", "");
                    stagedToString[^1] = stagedToString[^1].Replace("\u001b[7m", "");
                }
                else
                {
                    unstagedToString[0] = unstagedToString[0].Replace("\u001b[7m", "");
                    unstagedToString[^1] = unstagedToString[^1].Replace("\u001b[7m", "");
                }

                leftSide = new BoxDrawer(unstagedToString, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}");
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), windowHeight - unstagedHeight - 4, true);
                leftSide.AddRange(stagedToString);
            }

            return new ScreenSelector(leftSide, rightSide, consoleWrapper).Select(ref isRightSideInUse, keyInteractor);
        }
    }
}
