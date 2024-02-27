using System;
using System.Diagnostics;

namespace TestApp
{
    public class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            do
            {
                GitDiff();
                GitDiff();
                GitDiff();
                DrawLine(0, 0, Console.WindowWidth, '-');
                for (int i = 1; i < Console.WindowHeight - 1; i++)
                {
                    DrawLine(0, i, Console.WindowWidth, ' ');
                }

                DrawLine(0, Console.WindowHeight - 1, Console.WindowWidth, '-');
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            Console.Clear();
            Console.CursorVisible = true;
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
    }
}