using System;
using System.Collections.Generic;
using System.Dynamic;
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
    }

    public class Commit : IElement
    {
        public readonly string Id;
        public readonly string Author;
        public readonly string Date;
        public readonly string Message;
        private readonly string rawCommitDetails;

        public Commit(string rawCommitDetails)
        {
            this.rawCommitDetails = rawCommitDetails;
            var props = ExtractProperties();
            this.Id = props.Id;
            this.Author = props.Author;
            this.Date = props.Date;
            this.Message = props.Message;
        }

        public string GetElement()
        {
            return Id + "\n" + Author + "\n" + Date + "\n" + Message;
        }

        private CommitProperties ExtractProperties()
        {
            var temp = rawCommitDetails.Split("\n");
            return temp[0].Length == 7 ? new CommitProperties()
            {
                Id = temp[0].Trim(),
                Author = GetAuthorName(),
                Date = temp[2].Trim(),
                Message = temp[3].Trim()
            }
            :
            new CommitProperties()
            {
                Id = temp[0][7..14].Trim(),
                Author = GetAuthorName()[7..].Trim(),
                Date = temp[2][5..].Trim(),
                Message = temp[3].Trim()
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
