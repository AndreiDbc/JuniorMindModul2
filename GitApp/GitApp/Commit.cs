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

    public class Commit
    {
        public readonly string Id;
        public readonly string Author;
        public readonly string Date;
        public readonly string Message;
        public readonly string Sha;
        private readonly string rawCommitDetails;

        public Commit(string rawCommitDetails)
        {
            this.rawCommitDetails = rawCommitDetails;
            var props = ExtractProperties();
            this.Id = props.Id;
            this.Author = props.Author;
            this.Date = props.Date;
            this.Message = props.Message;
            this.Sha = props.Sha;
        }

        public string GetElement()
        {
            return Id + " " + Author + " " + Date + " " + Message;
        }

        private static string GetDate(string[] data)
        {
            foreach (var item in data)
            {
                if (item.StartsWith("Date:"))
                {
                    var temp = item.Split(" ");
                    var hhmmss = temp[6].Split(":");
                    var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
                    var monthIndex = Array.IndexOf(monthNames, temp[4]) + 1;
                    var monthIndexToString = monthIndex < 10 ? "0" + monthIndex.ToString() : monthIndex.ToString();
                    var day = temp[5].Length == 1 ? "0" + temp[5] : temp[5];
                    var commitDate = temp[7] + "-" + monthIndexToString + "-" + day;
                    var timeDiference = DateTime.Now.Subtract(
                        new DateTime(int.Parse(temp[7]), int.Parse(monthIndexToString), int.Parse(temp[5]), int.Parse(hhmmss[0]), int.Parse(hhmmss[1]), int.Parse(hhmmss[2])))
                        .ToString().Split(new[] { ':', '.' });
                    return commitDate + (int.Parse(timeDiference[0]) < 24 ? GetAdditionalTime(timeDiference) : "");
                }
            }

            return "";
        }

        private static string GetAdditionalTime(string[] data)
        {
            var result = "";
            if (data[0] == "00" && data[1] == "00")
            {
                result = "less than a minute ago";
            }
            else if (data[0] == "00")
            {
                result = data[2] == "01"
                    ? "1 minute ago"
                    : $"{data[2]} minutes ago";
            }
            else
            {
                result = data[0] == "01"
                    ? "1 hour ago"
                    : $"{data[0]} hours ago";
            }

            return result.StartsWith("0") ? "(" + result[1..] + ")" : "(" + result + ")";
        }

        private CommitProperties ExtractProperties()
        {
            var temp = rawCommitDetails.Split("\n");
            return new CommitProperties()
            {
                Id = temp[0][7..14].Trim(),
                Author = GetAuthorName()[7..].Trim(),
                Date = GetDate(temp),
                Message = temp[3].Trim(),
                Sha = temp[0][7..]
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