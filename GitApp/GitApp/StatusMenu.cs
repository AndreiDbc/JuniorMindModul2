using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public class StatusMenu : Menu
    {
        private readonly GitManager gitManager;
        private ModifiedFiles unstaged;
        private ModifiedFiles staged;
        private List<IElement> unstagedElements;
        private List<string> unstagedPageList;
        private List<IElement> stagedElements;
        private List<string> stagedPageList;
        private bool isUnstagedInUse;
        private bool isStagedInUse;
        private bool unstagedAvailability;

        public StatusMenu(IConsoleWrapper consoleWrapper, GitManager gitManager, ModifiedFiles unstaged, ModifiedFiles staged, bool unstagedAvailability) : base(consoleWrapper)
        {
            this.gitManager = gitManager;
            this.unstaged = unstaged;
            this.staged = staged;
            this.unstagedAvailability = unstagedAvailability;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
            unstagedElements = unstaged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
            unstagedPageList = unstaged[unstaged.Current].GetElement() == ""
                   ? new List<string>()
                   : unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedAvailableSpace);
            var stagedHeight = windowHeight - unstagedHeight;
            var stagedAvailableSpace = stagedHeight * ((int)(0.6 * windowWidth) - 2);
            stagedElements = staged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
            stagedPageList = staged[staged.Current].GetElement() == ""
                    ? new List<string>()
                    : staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedAvailableSpace);
            isUnstagedInUse = true;
            isStagedInUse = false;
        }

        public override List<string> UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var unstagedTempIndex = unstaged.Index;
            var unstagedTempCurrent = unstaged.Current;
            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            var unstagedCount = unstaged.ListCount;
            unstaged.Index = Math.Min(unstagedTempIndex, unstagedCount - 1);
            unstaged.Current = Math.Min(unstagedTempCurrent, unstagedCount - 1);
            var stagedTempIndex = staged.Index;
            var stagedTempCurrent = staged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            var stagedCount = staged.ListCount;
            staged.Index = Math.Min(stagedTempIndex, stagedCount - 1);
            staged.Current = Math.Min(stagedTempCurrent, stagedCount - 1);
            unstagedAvailability = GetUnstagedAvailability(ref key, windowHeight - unstagedHeight, unstagedHeight);

            return UpdateStatusMenu(key);
        }

        private static (int index, int current) SetModifiedFileIndeexes(ModifiedFiles modifiedFilesToSet, ModifiedFiles modifiedFilesToUse, int height, ConsoleKey key)
        {
            if (modifiedFilesToUse.Current == 0 && key == ConsoleKey.UpArrow)
            {
                modifiedFilesToSet.Current = modifiedFilesToSet.ListCount - 1;
                modifiedFilesToSet.Index = modifiedFilesToSet.ListCount - height < 0 ? 0 : modifiedFilesToSet.ListCount - height;
            }
            else
            {
                modifiedFilesToSet.Current = 0;
                modifiedFilesToSet.Index = 0;
            }

            return (modifiedFilesToSet.Index, modifiedFilesToSet.Current);
        }

        private string SetMenuBarForStatus()
        {
            return "\u001b[32m[Esc] Exit \u001b[33m[→] Next menu \u001b[36m[B] Branches "
                + (unstagedAvailability ? "\u001b[34m[⏎] Stage file \u001b[35m[A] Stage all" : "\u001b[34m[⏎] Unstage file \u001b[35m[A] Unstage all\u001b[0m");
        }

        private List<string> UpdateStatusMenu(ConsoleKey key)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var windowWidth = consoleWrapper.WindowWidth;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var stagedHeight = windowHeight - unstagedHeight;
            var unstagedToString = new List<string>();
            var stagedToString = new List<string>();
            var tempList = new List<string>();
            var branch = gitManager.GetCurrentGitBranch();
            var noOfCommitsToPull = gitManager.GetNumberOfCommitsToPull(branch);
            var noOfCommitsToPush = gitManager.GetNumberOfCommitsToPush(branch);
            var shouldDisplayNoOfCommits = noOfCommitsToPush != "" && noOfCommitsToPull != "";
            Diff diff;

            if (isUnstagedInUse)
            {
                var keyInteractor = new KeyInteractor<IElement>(unstaged);
                keyInteractor.KeyInfo = key;
                (unstaged.Index, unstaged.Current) = keyInteractor
                    .KeySwitch(unstagedPageList, unstagedElements[^1].Id, unstagedElements[0].Id, false, (int)(0.6 * windowWidth) - 2, unstaged[unstaged.Current].GetElement().Length);
                stagedPageList = staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedHeight * (int)(0.6 * windowWidth) - 2);
                stagedToString.AddRange(stagedPageList);
                stagedToString[0] = stagedToString[0].Replace("\x1b[7m", "");
                stagedToString[^1] = stagedToString[^1].Replace("\x1b[7m", "");
                var unstagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * unstagedHeight;
                unstagedElements = unstaged.GetPageDataAndSize(unstagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
                unstagedPageList = unstaged[unstaged.Current].GetElement() == ""
                       ? new List<string>()
                       : unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedAvailableSpace);
                var currentUnstaged = unstaged[unstaged.Current].GetElement();
                diff = new Diff(gitManager.GetUnstagedDiff(currentUnstaged, gitManager.GetRepoPath(), currentUnstaged != "" && unstaged.GetText().Count >= 1));
                tempList.AddRange(new BoxDrawer(unstagedPageList, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}"));
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox((int)(0.6 * windowWidth), (windowHeight - 2) / 2, true);
                tempList.AddRange(stagedToString);
            }
            else
            {
                var keyInteractor = new KeyInteractor<IElement>(staged);
                keyInteractor.KeyInfo = key;
                (staged.Index, staged.Current) = keyInteractor
                    .KeySwitch(stagedPageList, stagedElements[^1].Id, stagedElements[0].Id, false, (int)(0.6 * windowWidth) - 2, staged[staged.Current].GetElement().Length);
                unstagedPageList = unstaged.GetHighlightedSide(unstagedHeight, (int)(0.6 * windowWidth) - 2, unstagedHeight * (int)(0.6 * windowWidth) - 2);
                unstagedToString.AddRange(unstagedPageList);
                unstagedToString[0] = unstagedToString[0].Replace("\x1b[7m", "");
                unstagedToString[^1] = unstagedToString[^1].Replace("\x1b[7m", "");
                var stagedAvailableSpace = ((int)(0.6 * windowWidth) - 2) * stagedHeight;
                stagedElements = staged.GetPageDataAndSize(stagedAvailableSpace, (int)(0.6 * windowWidth) - 2).list;
                stagedPageList = staged[staged.Current].GetElement() == ""
                       ? new List<string>()
                       : staged.GetHighlightedSide(stagedHeight, (int)(0.6 * windowWidth) - 2, stagedAvailableSpace);
                var currentStaged = staged[staged.Current].GetElement();
                diff = new Diff(gitManager.GetStagedDiff(currentStaged, gitManager.GetRepoPath()));
                tempList.AddRange(new BoxDrawer(unstagedToString, "Unstaged").DrawBox(
                            (int)(0.6 * windowWidth),
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}"));
                stagedToString = new BoxDrawer(stagedPageList, "Staged").DrawBox((int)(0.6 * windowWidth), (windowHeight - 2) / 2, true);
                tempList.AddRange(stagedToString);
            }

            var diffAvailableSpace = (windowWidth - (int)(0.6 * windowWidth) - 2) * (windowHeight - 4);
            var rightSide = new BoxDrawer(
                    diff[0].GetElement() == ""
                        ? new List<string>()
                        : diff.GetHighlightedSide(windowHeight - 4, windowWidth - (int)(0.6 * windowWidth) - 2, diffAvailableSpace, false), "Diff")
                            .DrawBox(windowWidth - (int)(0.6 * windowWidth), windowHeight - 2);
            var result = new ScreenSelector(tempList, rightSide, consoleWrapper).MergeSides();
            var additionalMenuBar = SetMenuBarForStatus();
            result.Add(additionalMenuBar);
            return result;
        }

        private bool GetUnstagedAvailability(ref ConsoleKey key, int stagedHeight, int unstagedHeight)
        {
            var conditionToUseUnstaged = (unstaged.Current == 0 && key == ConsoleKey.UpArrow) || (unstaged.Current == unstaged.ListCount - 1 && key == ConsoleKey.DownArrow);
            var conditionToUseStaged = (staged.Current == 0 && key == ConsoleKey.UpArrow) || (staged.Current == staged.ListCount - 1 && key == ConsoleKey.DownArrow);
            if (conditionToUseUnstaged && !isStagedInUse)
            {
                (staged.Index, staged.Current) = SetModifiedFileIndeexes(staged, unstaged, stagedHeight, key);
                key = ConsoleKey.NoName;
                isUnstagedInUse = false;
                isStagedInUse = true;
                return false;
            }
            else if (conditionToUseStaged && !isUnstagedInUse)
            {
                (unstaged.Index, unstaged.Current) = SetModifiedFileIndeexes(unstaged, staged, unstagedHeight, key);
                key = ConsoleKey.NoName;
                isUnstagedInUse = true;
                isStagedInUse = false;
                return true;
            }

            return unstagedAvailability;
        }
    }
}
