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
                    var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
                    return temp[7] + "-" + (Array.IndexOf(monthNames, temp[4]) + 1) + "-" + temp[5];
                }
            }

            return "";
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