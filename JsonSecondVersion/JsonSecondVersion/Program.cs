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
            foreach (var arg in args)
            {
                var result = new Value().Match(System.IO.File.ReadAllText(arg));
                Console.WriteLine(result.Success() && string.IsNullOrEmpty(result.RemainingText()));
            }

            Console.Read();
        }
    }
}
