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
        private readonly TextWidthSetter textWidthSetter;
        private readonly RepositoryInfo repositoryInfo;
        private bool isRightSideInUse;
        private List<IElement> unstagedElements;
        private List<string> unstagedPageList;
        private List<IElement> stagedElements;
        private List<string> stagedPageList;
        private bool isUnstagedInUse;
        private bool isStagedInUse;

        public StatusMenu(
            IConsoleWrapper consoleWrapper,
            GitManager gitManager,
            ModifiedFiles unstaged,
            ModifiedFiles staged,
            RepositoryInfo repositoryInfo,
            bool unstagedAvailability,
            bool isRightSideInUse) : base(consoleWrapper)
        {
            this.gitManager = gitManager;
            this.repositoryInfo = repositoryInfo;
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
            textWidthSetter = new TextWidthSetter(consoleWrapper);
            this.isRightSideInUse = isRightSideInUse;
            Unstaged = unstaged;
            Staged = staged;
            UnstagedAvailability = unstagedAvailability;
            WasStagingKeyPressed = false;
        }

        public ModifiedFiles Unstaged { get; set; }

        public ModifiedFiles Staged { get; set; }

        public bool UnstagedAvailability { get; set; }

        public bool WasStagingKeyPressed { get; set; }

        public override List<string> UpdateMenu(bool isWindowToSmall, ConsoleKey key)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            UnstagedAvailability = GetUnstagedAvailability(key, windowHeight - unstagedHeight, unstagedHeight);
            if (isWindowToSmall)
            {
                return new List<string>();
            }

            return UpdateStatusMenu(key);
        }

        private static (int index, int current) SetModifiedFileIndeexes(ModifiedFiles modifiedFilesToSet, ModifiedFiles modifiedFilesToUse, int height, ConsoleKey key)
        {
            if (modifiedFilesToUse.Current == 0 && key == ConsoleKey.UpArrow)
            {
                modifiedFilesToSet.Current = modifiedFilesToUse[0].GetElement() == "" ? modifiedFilesToSet.Current : modifiedFilesToSet.ListCount - 1;
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
            var menuBar = "\u001b[32m[Esc] Exit \u001b[33m[→] Next menu \u001b[36m[B] Branches "
                + (UnstagedAvailability ? "\u001b[34m[⏎] Stage file \u001b[35m[A] Stage all" : "\u001b[34m[⏎] Unstage file \u001b[35m[A] Unstage all\u001b[0m");
            return menuBar + new string(' ', consoleWrapper.WindowWidth - menuBar.Length + 24);
        }

        private List<string> UpdateStatusMenu(ConsoleKey key)
        {
            var windowHeight = consoleWrapper.WindowHeight;
            var unstagedHeight = (windowHeight - 2) / 2 + (windowHeight - 2) % 2 - 2;
            var stagedHeight = windowHeight - unstagedHeight;
            var unstagedToString = new List<string>();
            var stagedToString = new List<string>();
            var tempList = new List<string>();
            var branch = repositoryInfo.CurrentBranchName;
            var noOfCommitsToPull = repositoryInfo.NoOfCommitsToPull;
            var noOfCommitsToPush = repositoryInfo.NoOfCommitsToPush;
            var shouldDisplayNoOfCommits = noOfCommitsToPush != "" && noOfCommitsToPull != "";
            Diff diff;
            var rightSideTextSize = textWidthSetter.SetTextWidth(false);
            var leftSideTextSize = textWidthSetter.SetTextWidth(true);

            if (UnstagedAvailability)
            {
                stagedPageList = Staged.GetHighlightedSide(stagedHeight, leftSideTextSize, stagedHeight * leftSideTextSize);
                stagedToString.AddRange(stagedPageList);
                stagedToString[0] = stagedToString[0].Replace("\x1b[7m", "");
                stagedToString[^1] = stagedToString[^1].Replace("\x1b[7m", "");
                var unstagedAvailableSpace = leftSideTextSize * unstagedHeight;
                unstagedElements = Unstaged.GetPageDataAndSize(unstagedAvailableSpace, leftSideTextSize).list;
                unstagedPageList = Unstaged[Unstaged.Current].GetElement() == ""
                       ? new List<string>()
                       : Unstaged.GetHighlightedSide(unstagedHeight, leftSideTextSize, unstagedAvailableSpace);
                var currentUnstaged = Unstaged[Unstaged.Current].GetElement();
                diff = new Diff(gitManager.GetUnstagedDiff(currentUnstaged, currentUnstaged != "" && Unstaged.GetText().Count >= 1));
                tempList.AddRange(new BoxDrawer(unstagedPageList, "Unstaged").DrawBox(
                            leftSideTextSize + 2,
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}"));
                stagedToString = new BoxDrawer(stagedToString, "Staged").DrawBox(leftSideTextSize + 2, (windowHeight - 2) / 2);
                tempList.AddRange(stagedToString);
            }
            else
            {
                unstagedPageList = Unstaged.GetHighlightedSide(unstagedHeight, leftSideTextSize, unstagedHeight * leftSideTextSize);
                unstagedToString.AddRange(unstagedPageList);
                unstagedToString[0] = unstagedToString[0].Replace("\x1b[7m", "");
                unstagedToString[^1] = unstagedToString[^1].Replace("\x1b[7m", "");
                var stagedAvailableSpace = leftSideTextSize * stagedHeight;
                stagedElements = Staged.GetPageDataAndSize(stagedAvailableSpace, leftSideTextSize).list;
                stagedPageList = Staged[Staged.Current].GetElement() == ""
                       ? new List<string>()
                       : Staged.GetHighlightedSide(stagedHeight, leftSideTextSize, stagedAvailableSpace);
                var currentStaged = Staged[Staged.Current].GetElement();
                diff = new Diff(gitManager.GetStagedDiff(currentStaged));
                tempList.AddRange(new BoxDrawer(unstagedToString, "Unstaged").DrawBox(
                            leftSideTextSize + 2,
                            unstagedHeight + 2,
                            false,
                            false,
                            shouldDisplayNoOfCommits ? $"↑{noOfCommitsToPush} ↓{noOfCommitsToPull}" + " {" + branch + "}" : "{" + branch + "}"));
                stagedToString = new BoxDrawer(stagedPageList, "Staged").DrawBox(leftSideTextSize + 2, (windowHeight - 2) / 2);
                tempList.AddRange(stagedToString);
            }

            var diffAvailableSpace = rightSideTextSize * (windowHeight - 4);
            var rightSide = new BoxDrawer(
                    diff[0].GetElement() == ""
                        ? new List<string>()
                        : diff.GetHighlightedSide(windowHeight - 4, rightSideTextSize, diffAvailableSpace, false), "Diff")
                            .DrawBox(rightSideTextSize + 2, windowHeight - 2);
            var additionalMenuBar = SetMenuBarForStatus();
            tempList.Add(additionalMenuBar);

            return new ScreenSelector(tempList, rightSide, consoleWrapper).Select(ref isRightSideInUse, key);
        }

        private bool GetUnstagedAvailability(ConsoleKey key, int stagedHeight, int unstagedHeight)
        {
            var unstagedContainsElements = Unstaged[0].GetElement() != "";
            var stagedContainsElements = Staged[0].GetElement() != "";
            (var conditionToUseUnstaged, var conditionToUseStaged) = GetConditionsToUseInteractor(key);
            KeyInteractor<IElement> keyInteractor;

            if (conditionToUseUnstaged && !isStagedInUse && stagedContainsElements)
            {
                (Staged.Index, Staged.Current) = SetModifiedFileIndeexes(Staged, Unstaged, stagedHeight, key);
                isUnstagedInUse = false;
                isStagedInUse = true;
                return false;
            }
            else if (conditionToUseStaged && !isUnstagedInUse && unstagedContainsElements)
            {
                (Unstaged.Index, Unstaged.Current) = SetModifiedFileIndeexes(Unstaged, Staged, unstagedHeight, key);
                isUnstagedInUse = true;
                isStagedInUse = false;
                return true;
            }
            else if (UnstagedAvailability && unstagedContainsElements)
            {
                keyInteractor = new KeyInteractor<IElement>(Unstaged);
                keyInteractor.KeyInfo = key;
                (Unstaged.Index, Unstaged.Current) = keyInteractor
                    .KeySwitch(unstagedPageList, unstagedElements[^1].Id, unstagedElements[0].Id, false, textWidthSetter.SetTextWidth(true), Unstaged[Unstaged.Current].GetElement().Length);
                return true;
            }
            else if (!UnstagedAvailability && stagedContainsElements)
            {
                keyInteractor = new KeyInteractor<IElement>(Staged);
                keyInteractor.KeyInfo = key;
                (Staged.Index, Staged.Current) = keyInteractor
                    .KeySwitch(stagedPageList, stagedElements[^1].Id, stagedElements[0].Id, false, textWidthSetter.SetTextWidth(true), Staged[Staged.Current].GetElement().Length);
                return false;
            }
            else if (!stagedContainsElements)
            {
                isStagedInUse = false;
                isUnstagedInUse = true;
                return true;
            }
            else if (!unstagedContainsElements)
            {
                isStagedInUse = true;
                isUnstagedInUse = false;
                return false;
            }

            return UnstagedAvailability;
        }

        private (bool shouldUseUnstaged, bool shouldUseStaged) GetConditionsToUseInteractor(ConsoleKey key)
        {
            return ((Unstaged.Current == 0 && key == ConsoleKey.UpArrow) || (Unstaged.Current == Unstaged.ListCount - 1 && key == ConsoleKey.DownArrow),
                (Staged.Current == 0 && key == ConsoleKey.UpArrow) || (Staged.Current == Staged.ListCount - 1 && key == ConsoleKey.DownArrow));
        }
    }
}
