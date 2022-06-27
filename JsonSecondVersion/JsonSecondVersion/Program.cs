using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Value value = new ();
            foreach (var arg in args)
            {
                Console.WriteLine(value.Match(System.IO.File.ReadAllText(arg)).Success() && string.IsNullOrEmpty(value.Match(System.IO.File.ReadAllText(arg)).RemainingText()));
            }

            Console.Read();
        }
    }
}
