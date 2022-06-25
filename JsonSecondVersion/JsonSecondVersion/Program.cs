using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonSecondVersion
{
    class Program
    {
        static void Main()
        {
            string path = System.IO.File.ReadAllText(Console.ReadLine());
            Console.WriteLine(string.IsNullOrEmpty(new Value().Match(path).RemainingText()));
            Console.Read();
        }
    }
}
