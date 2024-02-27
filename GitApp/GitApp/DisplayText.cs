using Castle.Components.DictionaryAdapter.Xml;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly KeyReader keyReader;
        private FullCommits commits;
        ConsoleKey key;

        public DisplayText(FullCommits commits, GitManager gitManager, IConsoleWrapper consoleWrapper, RepositoryInfo repositoryInfo)
        {
            this.commits = commits;
            this.gitManager = gitManager;
            this.consoleWrapper = consoleWrapper;
            IsRightSideInUse = false;
            WasWindowResizedSmaller = false;
            menuKeys = new[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3 };
            key = ConsoleKey.NoName;
            this.repositoryInfo = repositoryInfo;
            this.keyReader = new KeyReader(consoleWrapper);
        }

        private bool WasWindowResizedSmaller { get; set; }

        private bool IsRightSideInUse { get; }

        public void Display()
        {
            Test();
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
                else if (key.Equals(ConsoleKey.D3))
                {
                    new TestClass(repositoryInfo.RepositoryPath, consoleWrapper).DisplayTestMenu(ref key);
                }
            }
            while (key != ConsoleKey.Escape);
        }

        private static void DrawLine(int x, int y, int length, char c)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(new string(c, length));
        }

        private static void GitDiff()
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "diff -- Program.cs",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void Test()
        {
            Console.CursorVisible = false;
            do
            {
                GitDiff();
                GitDiff();
                GitDiff();
                DrawLine(0, 0, Console.WindowWidth, '-');
                DrawLine(0, Console.WindowHeight - 1, Console.WindowWidth, '-');
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            Console.Clear();
            Console.CursorVisible = true;
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
            new MenuWriter(menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;

            do
            {
                key = keyReader.SetKeyValue(windowHeight, windowWidth).Key;
                if (key.Equals(ConsoleKey.Escape))
                {
                    break;
                }
                else if (key.Equals(ConsoleKey.Enter))
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
                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = consoleWrapper.WindowHeight < 3;
                new MenuWriter(menu.UpdateMenu(isWindowToSmall, key), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
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
            new MenuWriter(menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;
            var keys = new[] { ConsoleKey.A, ConsoleKey.Enter, ConsoleKey.S };

            do
            {
                consoleWrapper.SetCursorPosition(0, 0);
                key = keyReader.SetKeyValue(windowHeight, windowWidth, true).Key;

                if (key.Equals(ConsoleKey.Escape))
                {
                    break;
                }
                else if (key.Equals(ConsoleKey.C))
                {
                    DisplayCommitPage(menu, staged);
                    unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
                    staged = new ModifiedFiles(gitManager.GetStagedFiles());
                    menu.Staged = staged;
                    menu.Unstaged = unstaged;
                }
                else if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.RightArrow))
                {
                    unstaged = menu.Unstaged;
                    staged = menu.Staged;
                    DisplayHighlightedHunksPage(ref unstaged, ref staged, unstagedAvailability);
                    menu.Unstaged = unstaged;
                    menu.Staged = staged;
                }

                if (menuKeys.Contains(key))
                {
                    break;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                unstagedAvailability = menu.UnstagedAvailability;
                menu.WasStagingKeyPressed = keys.Any(necessaryKey => necessaryKey.Equals(key));

                if (menu.WasStagingKeyPressed)
                {
                    ApplyStageOrUnstageOption(ref menu, ref unstaged, ref staged, unstagedAvailability);
                }

                isWindowToSmall = windowHeight < 8 && windowWidth < 35;
                new MenuWriter(menu.UpdateMenu(isWindowToSmall, key), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void ApplyStageOrUnstageOption(ref StatusMenu menu, ref ModifiedFiles unstaged, ref ModifiedFiles staged, bool unstagedAvailability)
        {
            var current = unstagedAvailability
                ? unstaged[unstaged.Current].GetElement()
                : staged[staged.Current].GetElement();
            if (current == "")
            {
                return;
            }

            unstaged = menu.Unstaged;
            staged = menu.Staged;

            if (unstagedAvailability)
            {
                if (key.Equals(ConsoleKey.Enter))
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
                if (key.Equals(ConsoleKey.Enter))
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
            new MenuWriter(menu.UpdateMenu(isWindowToSmall, key), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            var windowSizeTemp = windowSize;

            do
            {
                consoleWrapper.SetCursorPosition(0, 0);
                key = keyReader.SetKeyValue(windowHeight, windowWidth, true).Key;
                SelectStageOptionForDiffMenu(ref menu, ref diff, ref unstaged, ref staged, unstagedAvailability);
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

                if (menuKeys.Contains(key))
                {
                    break;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                isWindowToSmall = windowHeight < 8 && windowWidth < 35;
                new MenuWriter(menu.UpdateMenu(isWindowToSmall, key), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            }
            while (true);
        }

        private void SelectStageOptionForDiffMenu(
            ref HighlightedHunksMenu menu,
            ref Diff diff,
            ref ModifiedFiles unstaged,
            ref ModifiedFiles staged,
            bool unstagedAvailability)
        {
            diff = menu.Diff;
            unstaged = menu.Unstaged;
            staged = menu.Staged;
            var currentFile = unstagedAvailability
                ? unstaged[unstaged.Current].GetElement()
                : staged[staged.Current].GetElement();
            if (key.Equals(ConsoleKey.Enter))
            {
                UpdateVariablesWhenStagingOrUnstagingHunk(ref diff, ref unstaged, ref staged, unstagedAvailability);
            }
            else if (key.Equals(ConsoleKey.S))
            {
                UpdateVariablesWhenStagingOrUnstagingLine(ref diff, unstagedAvailability, ref unstaged, ref staged);
            }
            else if (key.Equals(ConsoleKey.A))
            {
                if (unstagedAvailability)
                {
                    gitManager.StageFile(currentFile);
                }
                else
                {
                    gitManager.UnstageFile(currentFile);
                }

                unstaged = new ModifiedFiles(gitManager.GetUnstagedFiles());
                staged = new ModifiedFiles(gitManager.GetStagedFiles());
            }

            menu.Diff = diff;
            menu.Unstaged = unstaged;
            menu.Staged = staged;
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
            new MenuWriter(menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
            do
            {
                key = keyReader.SetKeyValue(windowHeight, windowWidth).Key;
                if (key.Equals(ConsoleKey.Escape))
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

                if (menuKeys.Contains(key))
                {
                    break;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                new MenuWriter(menu.UpdateMenu(isWindowToSmall, key), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
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
            new MenuWriter(menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);

            do
            {
                key = keyReader.SetKeyValue(windowHeight, windowWidth).Key;
                if (key.Equals(ConsoleKey.Escape) || key.Equals(ConsoleKey.LeftArrow))
                {
                    break;
                }
                else if (key.Equals(ConsoleKey.RightArrow))
                {
                    diff = new Diff(gitManager.GetDiffBetweenConsecutiveCommits(previousCommit.Id, currentCommit.Id, modifiedFiles[modifiedFiles.Current].GetElement()));
                    DisplayHighlightedDiffMenu(diff, modifiedFiles);
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
                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                new MenuWriter(menu.UpdateMenu(isWindowToSmall, key), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
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
            new MenuWriter(menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);

            do
            {
                key = keyReader.SetKeyValue(windowHeight, windowWidth).Key;
                if (key.Equals(ConsoleKey.B))
                {
                    DisplayBranchesMenu(menu, isWindowToSmall);
                }
                else if (key.Equals(ConsoleKey.Escape) || key.Equals(ConsoleKey.LeftArrow))
                {
                    break;
                }

                if (menuKeys.Contains(key))
                {
                    break;
                }

                windowWidth = consoleWrapper.WindowWidth;
                windowHeight = consoleWrapper.WindowHeight;
                windowSize = windowHeight * windowWidth;
                WasWindowResizedSmaller = windowSize < windowSizeTemp;
                windowSizeTemp = windowSize;
                new MenuWriter(menu.UpdateMenu(isWindowToSmall, key), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller);
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
            new MenuWriter(background.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper)
                .UpdateDisplay(WasWindowResizedSmaller, menu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName));

            do
            {
                var windowSizeTemp = windowSize;
                key = keyReader.SetKeyValue(height, width).Key;
                menu.UpdateMenu(isWindowToSmall, key);
                height = consoleWrapper.WindowHeight;
                width = consoleWrapper.WindowWidth;
                windowSize = height * width;
                keyInteractor.KeyInfo = key;
                if (keyInteractor.KeyInfo == ConsoleKey.Escape)
                {
                    key = ConsoleKey.NoName;
                    break;
                }
                else if (keyInteractor.KeyInfo == ConsoleKey.Enter)
                {
                    key = ConsoleKey.NoName;
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

                new MenuWriter(background.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper)
                    .UpdateDisplay(WasWindowResizedSmaller, menu.UpdateMenu(isWindowToSmall, key));
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
            var windowSize = 0;
            var windowSizeTemp = 0;
            var commitMessage = "";
            var keyInfo = new ConsoleKeyInfo((char)ConsoleKey.NoName, ConsoleKey.NoName, false, false, false);
            var commitMenu = new CommitMenu(consoleWrapper);
            new MenuWriter(backgroundMenu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper)
                .UpdateDisplay(isWindowToSmall, commitMenu.UpdateMenu(isWindowToSmall, key));

            do
            {
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    key = ConsoleKey.NoName;
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    key = ConsoleKey.NoName;
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

                commitMenu = new CommitMenu(consoleWrapper);
                var commitBox = commitMenu.UpdateMenu(isWindowToSmall, key);
                commitMessage = commitMessage[..Math.Min((commitBox[0].Length - 13) * (commitBox[0].Length - 3), commitMessage.Length)];
                new MenuWriter(backgroundMenu.UpdateMenu(isWindowToSmall, ConsoleKey.NoName), isWindowToSmall, consoleWrapper).UpdateDisplay(WasWindowResizedSmaller, commitBox);
                var height = consoleWrapper.WindowHeight;
                var width = consoleWrapper.WindowWidth;
                var left = (int)(0.125 * width) + 1;
                var top = (int)(0.125 * height) + 1;
                if (height < 15 && width < 60)
                {
                    left = 1;
                    top = 1;
                }

                (commitMessage, keyInfo) = new TextReader(left, top, commitBox[0].Length - 13, commitBox[0].Length - 2, consoleWrapper, keyReader)
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