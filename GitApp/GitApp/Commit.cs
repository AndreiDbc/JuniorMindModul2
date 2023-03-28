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

    public class Commit
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
