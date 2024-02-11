using Castle.Components.DictionaryAdapter.Xml;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GitApp
{
    public class DisplayText
    {
        private readonly GitManager gitManager;
        private readonly IConsoleWrapper consoleWrapper;
        private readonly ConsoleKey[] menuKeys;
        private readonly RepositoryInfo repositoryInfo;
        private FullCommits commits;
        ConsoleKey key;

        public DisplayText(FullCommits commits, GitManager gitManager, IConsoleWrapper consoleWrapper, RepositoryInfo repositoryInfo)
        {
            this.commits = commits;
            this.gitManager = gitManager;
            this.consoleWrapper = consoleWrapper;
            IsRightSideInUse = false;
            WasWindowResizedSmaller = false;
            menuKeys = new[] { ConsoleKey.D1, ConsoleKey.D2 };
            key = ConsoleKey.NoName;
            this.repositoryInfo = repositoryInfo;
        }

        private bool WasWindowResizedSmaller { get; set; }

        private bool IsRightSideInUse { get; }

        public void Display()
        {
            DisplayStatus();
            do
            {
                if (key.Equals(ConsoleKey.D1))
                {
                    DisplayStatus();
                }
                else if (key.Equals(ConsoleKey.D2))
                {
                    DisplayLogMenu();
                }
            }
            while (key != ConsoleKey.Escape);
        }

        private bool GetExitConditionForHighlightedHunksPage(IElement currentUnstaged, IElement diffElement)
        {
            var exitCondition = key.Equals(ConsoleKey.Escape);
            exitCondition = exitCondition || menuKeys.Contains(key);
            exitCondition = exitCondition || key.Equals(ConsoleKey.LeftArrow);
            exitCondition = exitCondition || currentUnstaged.GetElement() == null;
            exitCondition = exitCondition || key.Equals(ConsoleKey.A);
            exitCondition = exitCondition || diffElement.GetElement() == "";
            return exitCondition;
        }

        private void DisplayLogMenu()
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var isWindowToSmall = windowHeight < 5 && windowWidth < 35;
            var menu = new LogMenu(consoleWrapper, commits);
            menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.Escape))
                {
                    break;
                }

                if (key.Equals(ConsoleKey.Enter))
                {
                    DisplayCommitDetailsMenu();
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }

                if (menuKeys.Contains(key))
                {
                    break;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = consoleWrapper.WindowHeight < 3;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void DisplayStatus()
        {
            var unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            var staged = new ModifiedFiles(gitManager.GetStagedFiles());
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var isWindowToSmall = windowHeight < 8 && windowWidth < 35;
            var unstagedAvailability = true;
            var menu = new StatusMenu(consoleWrapper, gitManager, unstaged, staged, repositoryInfo, unstagedAvailability, IsRightSideInUse);
            menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;
            var keys = new[] { ConsoleKey.A, ConsoleKey.Enter, ConsoleKey.S };
            do
            {
                key = consoleWrapper.ReadKey(true).Key;

                if (key.Equals(ConsoleKey.Escape) || menuKeys.Contains(key))
                {
                    break;
                }
                else if (key.Equals(ConsoleKey.C))
                {
                    DisplayCommitPage(menu, staged);
                    menu.WasStagingKeyPressed = key.Equals(ConsoleKey.Enter);
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }

                Task.Run(() =>
                {
                    unstagedAvailability = menu.UnstagedAvailability;
                    if (key.Equals(ConsoleKey.RightArrow))
                    {
                        unstaged = menu.Unstaged;
                        staged = menu.Staged;
                        DisplayHighlightedHunksPage(ref unstaged, ref staged, unstagedAvailability);
                        menu.Unstaged = unstaged;
                        menu.Staged = staged;
                    }

                    menu.WasStagingKeyPressed = keys.Any(necessaryKey => necessaryKey.Equals(key));
                    if (menu.WasStagingKeyPressed)
                    {
                        ApplyStageOrUnstageOption(ref menu, ref unstaged, ref staged, unstagedAvailability);
                    }

                    menu.UpdateMenu(isWindowToSmall, key);

                    windowWidth = consoleWrapper.WindowWidth;
                    windowSize = consoleWrapper.WindowHeight * windowWidth;
                    WasWindowResizedSmaller = windowSize < windowSizeTemp;
                    windowSizeTemp = windowSize;
                    isWindowToSmall = windowHeight < 8 && windowWidth < 35;
                    WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
                });
                Task.WaitAll();
            }
            while (true);
        }

        private void ApplyStageOrUnstageOption(ref StatusMenu menu, ref ModifiedFiles unstaged, ref ModifiedFiles staged, bool unstagedAvailability)
        {
            unstaged = menu.Unstaged;
            staged = menu.Staged;

            if (unstagedAvailability)
            {
                var current = unstaged[unstaged.Current].GetElement();
                if (key.Equals(ConsoleKey.Enter) && current != "")
                {
                    gitManager.StageFile(current);
                    menu.Unstaged.Current = Math.Max(0, menu.Unstaged.Current - 1);
                    menu.Unstaged.Index = Math.Min(menu.Unstaged.Current, menu.Unstaged.Index);
                }
                else if (key.Equals(ConsoleKey.A))
                {
                    gitManager.StageAllFiles();
                    menu.Unstaged.Current = 0;
                    menu.Unstaged.Index = 0;
                }
            }
            else
            {
                var current = staged[staged.Current].GetElement();
                if (key.Equals(ConsoleKey.Enter) && current != "")
                {
                    gitManager.UnstageFile(current);
                    menu.Staged.Current = Math.Max(0, menu.Staged.Current - 1);
                    menu.Staged.Index = Math.Min(menu.Staged.Current, menu.Staged.Index);
                }
                else if (key.Equals(ConsoleKey.A))
                {
                    gitManager.UnstageAllFiles();
                    menu.Staged.Current = 0;
                    menu.Staged.Index = 0;
                }
            }

            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Index = menu.Unstaged.Index;
            unstaged.Current = menu.Unstaged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Index = menu.Staged.Index;
            staged.Current = menu.Staged.Current;
            menu.Unstaged = unstaged;
            menu.Staged = staged;
        }

        private void DisplayHighlightedHunksPage(ref ModifiedFiles unstaged, ref ModifiedFiles staged, bool unstagedAvailability)
        {
            var currentElement = unstagedAvailability ? unstaged[unstaged.Current].GetElement() : staged[staged.Current].GetElement();
            if (currentElement == "")
            {
                return;
            }

            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var isWindowToSmall = windowHeight < 8 && windowWidth < 35;
            var diff = new Diff(unstagedAvailability
                ? gitManager.GetUnstagedDiff(
                    currentElement,
                    unstaged.GetText().Count >= 1)
                : gitManager.GetStagedDiff(staged[staged.Current].GetElement()));
            var menu = new HighlightedHunksMenu(consoleWrapper, gitManager, diff, unstaged, staged, unstagedAvailability, IsRightSideInUse);
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                var stageSuccesful = SelectStageOptionForDiffMenu(ref menu, ref diff, ref unstaged, ref staged, unstagedAvailability);
                menu.UpdateMenu(isWindowToSmall, key);

                if (GetExitConditionForHighlightedHunksPage(unstagedAvailability ? unstaged[unstaged.Current] : staged[staged.Current], diff[diff.Current]))
                {
                    break;
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.C))
                {
                    DisplayCommitPage(menu, staged);
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowSize = consoleWrapper.WindowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = windowHeight < 8 && windowWidth < 35;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private bool SelectStageOptionForDiffMenu(
            ref HighlightedHunksMenu menu,
            ref Diff diff,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            bool unstagedAvailability)
        {
            var stageSuccesful = false;
            diff = menu.Diff;
            unstaged = menu.Unstaged;
            staged = menu.Staged;
            var currentFile = unstagedAvailability
                ? unstaged[unstaged.Current].GetElement()
                : staged[staged.Current].GetElement();
            if (key.Equals(ConsoleKey.Enter))
            {
                UpdateVariablesWhenStagingOrUnstagingHunk(ref diff, ref unstaged, ref staged, unstagedAvailability);
                stageSuccesful = true;
            }
            else if (key.Equals(ConsoleKey.S))
            {
                UpdateVariablesWhenStagingOrUnstagingLine(ref diff, unstagedAvailability, ref unstaged, ref staged);
                stageSuccesful = true;
            }
            else if (key.Equals(ConsoleKey.A))
            {
                if (unstagedAvailability)
                {
                    gitManager.StageFile(currentFile);
                    stageSuccesful = true;
                }
                else
                {
                    gitManager.UnstageFile(currentFile);
                    stageSuccesful = true;
                }

                unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
            }

            menu.Diff = diff;
            menu.Unstaged = unstaged;
            menu.Staged = staged;

            return stageSuccesful;
        }

        private void UpdateVariablesWhenStagingOrUnstagingHunk(
            ref Diff diff,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            bool unstagedAvailability)
        {
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var currentStaged = staged[staged.Current].GetElement();
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current;
            var currentFile = unstagedAvailability ? currentUnstaged : currentStaged;
            var patchPath = new Patcher(repositoryInfo.RepositoryPath, gitManager, diff, currentFile != "").GetHunkPatchPath();
            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            var stagedIndex = staged.Index;
            var stagedCurrent = staged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Current = stagedCurrent;
            staged.Index = stagedIndex;

            if (unstagedAvailability)
            {
                gitManager.Stage(patchPath);
            }
            else
            {
                var tempFilePath = Path.GetTempPath() + "tempFile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                System.IO.File.Create(tempFilePath).Close();
                var currentElementLines = System.IO.File.ReadAllLines(repositoryInfo.RepositoryPath + "/" + currentFile);
                currentElementLines[0] = "﻿\ufeff" + currentElementLines[0];
                System.IO.File.WriteAllLines(tempFilePath, currentElementLines);
                gitManager.RestoreFileToMatchIndex(currentFile);
                gitManager.UnstageFile(currentFile);
                gitManager.ApplyPatchInReverse(patchPath);
                gitManager.StageFile(currentFile);
                System.IO.File.WriteAllLines(repositoryInfo.RepositoryPath + "/" + currentFile, System.IO.File.ReadAllLines(tempFilePath));
                System.IO.File.Delete(tempFilePath);
            }

            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            diff = unstagedAvailability
                ? new Diff(gitManager.GetUnstagedDiff(currentUnstaged, currentFile != "" && unstaged.GetText().Count >= 1))
                : new Diff(gitManager.GetStagedDiff(currentStaged));
            diff.Current = Math.Min(diffCurrent, diff.GetText().Count - 1);
            diff.Index = diffIndex;
        }

        private void UpdateVariablesWhenStagingOrUnstagingLine(
            ref Diff diff,
            bool unstagedAvailability,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged)
        {
            var diffElements = diff.GetText();
            var currentStaged = staged[staged.Current].GetElement();
            var currentUnstaged = unstaged[unstaged.Current].GetElement();
            var currentFile = unstagedAvailability ? currentUnstaged : currentStaged;
            var diffIndex = diff.Index == 0 ? 0 : diff.Index;
            var diffCurrent = diff.Current == 0 ? 0 : diff.Current - 1;
            var previous = diff[Math.Max(0, diff.Current - 1)].GetElement();
            var next = diff[Math.Min(diffElements.Count - 1, diff.Current + 1)].GetElement();
            var isLineReplaced = previous.StartsWith("-") && diff[diff.Current].GetElement().StartsWith("+");
            isLineReplaced = isLineReplaced || (diff[diff.Current].GetElement().StartsWith("-") && next.StartsWith("+"));
            var patchPath = new Patcher(gitManager.GetRepoPath(), gitManager, diff, currentFile != "")
                .GetLinePatchPath(
                    isLineReplaced,
                    staged.GetText().Any(unstagedFile => unstagedFile.GetElement() == currentUnstaged),
                    currentFile);

            if (unstagedAvailability)
            {
                gitManager.Stage(patchPath);
            }
            else
            {
                var tempFilePath = Path.GetTempPath() + "tempFile" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                string[] currentElementLines;
                System.IO.File.Create(tempFilePath).Close();
                currentElementLines = System.IO.File.ReadAllLines(repositoryInfo.RepositoryPath + "/" + currentFile);
                currentElementLines[0] = "﻿\ufeff" + currentElementLines[0];
                System.IO.File.WriteAllLines(tempFilePath, currentElementLines);
                gitManager.RestoreFileToMatchIndex(currentFile);
                gitManager.UnstageFile(currentFile);
                gitManager.ApplyPatchInReverse(patchPath);
                gitManager.StageFile(currentFile);
                System.IO.File.WriteAllLines(repositoryInfo.RepositoryPath + "/" + currentFile, System.IO.File.ReadAllLines(tempFilePath));
                System.IO.File.Delete(tempFilePath);
            }

            if (System.IO.File.Exists(patchPath))
            {
                System.IO.File.Delete(patchPath);
            }

            var unstagedIndex = unstaged.Index;
            var unstagedCurrent = unstaged.Current;
            unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
            unstaged.Current = unstagedCurrent;
            unstaged.Index = unstagedIndex;
            var stagedIndex = staged.Index;
            var stagedCurrent = staged.Current;
            staged = new ModifiedFiles(gitManager.GetStagedFiles());
            staged.Index = stagedIndex;
            staged.Current = stagedCurrent;
            diff = unstagedAvailability
                ? new Diff(gitManager.GetUnstagedDiff(currentUnstaged, currentFile != "" && unstaged.GetText().Count >= 1))
                : new Diff(gitManager.GetStagedDiff(currentStaged));
            diff.Current = Math.Min(diffCurrent, diff.GetText().Count - 1);
            diff.Index = diffIndex;
        }

        private void DisplayCommitDetailsMenu()
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var menu = new CommitDetailsMenu(consoleWrapper, gitManager, commits, IsRightSideInUse);
            var isWindowToSmall = consoleWrapper.WindowHeight < 11 && consoleWrapper.WindowWidth < 20;
            var windowSizeTemp = windowSize;
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.Escape) || menuKeys.Contains(key))
                {
                    break;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    DisplayDiffMenu(commits[commits.Current], commits[commits.Current + 1]);
                }
                else if (key == ConsoleKey.B)
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void DisplayDiffMenu(IElement currentCommit, IElement previousCommit)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var modifiedFiles = new ModifiedFiles(gitManager.GetCommitModifiedFileNames(currentCommit.Id));
            var diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, modifiedFiles[0].GetElement()));
            var menu = new DiffMenu(consoleWrapper, modifiedFiles, diff, previousCommit, currentCommit, gitManager, IsRightSideInUse);
            var isWindowToSmall = consoleWrapper.WindowHeight < 11 && consoleWrapper.WindowWidth < 20;
            var windowSizeTemp = windowSize;
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.RightArrow))
                {
                    diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, modifiedFiles[modifiedFiles.Current].GetElement()));
                    DisplayHighlightedDiffMenu(diff, modifiedFiles);
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.Escape) || key.Equals(ConsoleKey.LeftArrow) || menuKeys.Contains(key))
                {
                    break;
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void DisplayHighlightedDiffMenu(Diff diff, ModifiedFiles modifiedFiles)
        {
            var windowWidth = consoleWrapper.WindowWidth;
            var windowHeight = consoleWrapper.WindowHeight;
            var windowSize = windowHeight * windowWidth;
            var menu = new HighlightedDiffMenu(consoleWrapper, diff, modifiedFiles, IsRightSideInUse);
            var isWindowToSmall = consoleWrapper.WindowHeight < 11 && consoleWrapper.WindowWidth < 20;
            var windowSizeTemp = windowSize;
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);

            do
            {
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.Escape) || key.Equals(ConsoleKey.LeftArrow) || menuKeys.Contains(key))
                {
                    break;
                }

                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * consoleWrapper.WindowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                WasWindowResizedSmaller = new MenuWriter(menu.LastPageDisplayed, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void DisplayBranchesMenu(Menu background, bool isWindowToSmall)
        {
            var branchesNames = gitManager.GetBranchesNames();
            if (branchesNames == "")
            {
                return;
            }

            var height = consoleWrapper.WindowHeight;
            var width = consoleWrapper.WindowWidth;
            var windowSize = height * width;
            var branches = new ModifiedFiles(branchesNames);
            var keyInteractor = new KeyInteractor<IElement>(branches);
            keyInteractor.KeyInfo = ConsoleKey.NoName;
            var menu = new BranchMenu(consoleWrapper, gitManager, branchesNames, branches);
            menu.UpdateMenu(isWindowToSmall, key);
            WasWindowResizedSmaller = new MenuWriter(background.LastPageDisplayed, isWindowToSmall, consoleWrapper)
                .UpdateDisplay(WasWindowResizedSmaller, menu.LastPageDisplayed);

            do
            {
                var windowSizeTemp = windowSize;
                key = consoleWrapper.ReadKey(true).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                height = consoleWrapper.WindowHeight;
                width = consoleWrapper.WindowWidth;
                windowSize = height * width;
                keyInteractor.KeyInfo = key;
                if (keyInteractor.KeyInfo == ConsoleKey.Escape)
                {
                    break;
                }
                else if (keyInteractor.KeyInfo == ConsoleKey.Enter)
                {
                    var currentBranchName = branches[branches.Current].GetElement();
                    gitManager.ChangeBranch(currentBranchName.StartsWith("* ") ? currentBranchName[2..] : currentBranchName.Trim());
                    branchesNames = gitManager.GetBranchesNames();
                    branches = new ModifiedFiles(branchesNames);
                    menu.Branches = branches;
                    menu.BranchesNames = branchesNames;
                    repositoryInfo.CurrentBranchName = gitManager.GetCurrentGitBranch();
                }

                WasWindowResizedSmaller = windowSizeTemp > windowSize;
                if (WasWindowResizedSmaller)
                {
                    background.UpdateMenu(isWindowToSmall, key);
                }

                WasWindowResizedSmaller = new MenuWriter(background.LastPageDisplayed, isWindowToSmall, consoleWrapper)
                    .UpdateDisplay(WasWindowResizedSmaller, menu.LastPageDisplayed);
            }
            while (true);
        }

        private void DisplayCommitPage(Menu backgroundMenu, ModifiedFiles staged)
        {
            if (staged[0].GetElement() == "")
            {
                return;
            }

            var isWindowToSmall = consoleWrapper.WindowHeight < 8 && consoleWrapper.WindowWidth < 35;
            backgroundMenu.UpdateMenu(isWindowToSmall, key);
            var mainPage = backgroundMenu.LastPageDisplayed;
            var windowSize = 0;
            var windowSizeTemp = 0;
            var commitMessage = "";
            var keyInfo = new ConsoleKeyInfo((char)ConsoleKey.NoName, ConsoleKey.NoName, false, false, false);
            var commitMenu = new CommitMenu(consoleWrapper);
            commitMenu.UpdateMenu(isWindowToSmall, key);
            var commitBox = commitMenu.LastPageDisplayed;
            new MenuWriter(mainPage, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller, commitBox);

            do
            {
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    commitMessage = commitMessage.Replace("\n", "");
                    gitManager.Commit(commitMessage);
                    commits = new FullCommits(gitManager.GetLog());
                    repositoryInfo.NoOfCommitsToPush = gitManager.GetNumberOfCommitsToPush(repositoryInfo.CurrentBranchName);
                    break;
                }

                if (WasWindowResizedSmaller)
                {
                    backgroundMenu.UpdateMenu(isWindowToSmall, key);
                }

                mainPage = backgroundMenu.LastPageDisplayed;
                commitMenu = new CommitMenu(consoleWrapper);
                commitMenu.UpdateMenu(isWindowToSmall, key);
                commitBox = commitMenu.LastPageDisplayed;
                var commitBoxToArray = commitBox.Split(Environment.NewLine);
                commitMessage = commitMessage[..Math.Min((commitBoxToArray[0].Length - 13) * (commitBox.Length - 3), commitMessage.Length)];
                new MenuWriter(mainPage, isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller, commitBox);
                var height = consoleWrapper.WindowHeight;
                var width = consoleWrapper.WindowWidth;
                var left = (int)(0.125 * width) + 1;
                var top = (int)(0.125 * height) + 1;
                if (height < 15 && width < 60)
                {
                    left = 1;
                    top = 1;
                }

                (commitMessage, keyInfo) = new TextReader(left, top, commitBoxToArray[0].Length - 13, commitBoxToArray.Length - 2, consoleWrapper)
                    .GetText(ref commitMessage, keyInfo);
                key = keyInfo.Key;
                windowSize = height * width;
                WasWindowResizedSmaller = windowSizeTemp > windowSize;
                windowSizeTemp = windowSize;
            }
            while (true);
        }
    }
}