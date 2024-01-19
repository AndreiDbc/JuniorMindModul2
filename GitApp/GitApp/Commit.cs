using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitApp
{
    public struct CommitProperties
    {
        public string Id;
        public string Author;
        public string Date;
        public string Message;
        public string Sha;
    }

    public class Commit : IElement
    {
        public readonly string CommitId;
        public readonly string Author;
        public readonly string Date;
        public readonly string Message;
        public readonly string Sha;
        private readonly string rawCommitDetails;

        public Commit(string rawCommitDetails)
        {
            this.rawCommitDetails = rawCommitDetails;
            var props = ExtractProperties();
            this.CommitId = props.Id;
            this.Author = props.Author;
            this.Date = props.Date;
            this.Message = props.Message;
            this.Sha = props.Sha;
        }

        public string Id { get => CommitId; }

        public string GetElement()
        {
            return CommitId + "\n" + Author + "\n" + Date + "\n" + Message + "\n" + Sha;
        }

        private static string GetDate(string[] data)
        {
            foreach (var item in data)
            {
                if (item.StartsWith("Date:"))
                {
                    var temp = item.Split(" ");
                    var hhmmss = temp[6].Split(":");
                    var monthNames = CultureInfo.InvariantCulture.DateTimeFormat.AbbreviatedMonthNames;
                    var monthIndex = Array.IndexOf(monthNames, temp[4]) + 1;
                    var day = temp[5].Length == 1 ? "0" + temp[5] : temp[5];
                    var commitDate = new DateTime(int.Parse(temp[7]), monthIndex, int.Parse(day), int.Parse(hhmmss[0]), int.Parse(hhmmss[1]), int.Parse(hhmmss[2]));
                    var timeDiference = DateTime.Now.Subtract(commitDate);
                    return commitDate.Year + "-"
                        + (commitDate.Month.ToString().Length == 1 ? "0" + commitDate.Month : commitDate.Month) + "-"
                        + (commitDate.Day.ToString().Length == 1 ? "0" + commitDate.Day : commitDate.Day) +
                        (timeDiference.TotalDays < 1 ? GetAdditionalTime(timeDiference) : "");
                }
            }

            return "";
        }

        private static string GetAdditionalTime(TimeSpan data)
        {
            var result = "";
            if (data.Hours == 0 && data.Minutes < 1 && data.Seconds <= 59)
            {
                result = "less than a minute ago";
            }
            else if (data.Hours == 0)
            {
                result = data.Minutes == 1
                    ? "1 minute ago"
                    : $"{data.Minutes} minutes ago";
            }
            else
            {
                result = data.Hours == 1
                    ? "1 hour ago"
                    : $"{data.Hours} hours ago";
            }

            return result.StartsWith("0") ? "(" + result[1..] + ")" : "(" + result + ")";
        }

        private static string GetMessage(string[] data)
        {
            var result = "";
            for (int i = 3; i < data.Length; i++)
            {
                result += data[i].Trim() + " ";
            }

            return result.Trim();
        }

        private CommitProperties ExtractProperties()
        {
            var temp = rawCommitDetails.Split("\n");
            return new CommitProperties()
            {
                Id = temp[0][7..14].Trim(),
                Author = GetAuthorName()[7..].Trim(),
                Date = GetDate(temp),
                Message = GetMessage(temp),
                Sha = temp[0][7..47]
            };
        }

        private string GetAuthorName()
        {
            var temp = rawCommitDetails.Split("\n")[1];
            var result = "";
            foreach (var character in temp)
            {
                if (character == '<')
                {
                    break;
                }

                result += character;
            }

            return result.Trim();
        }
    }
}