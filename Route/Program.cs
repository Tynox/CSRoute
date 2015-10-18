using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route
{
    class Program
    {
        static void Main(string[] args)
        {
            string src = args[0];
            string dst = args[1];

            new Worker(src, dst);

            Console.Read();
        }
    }
}
