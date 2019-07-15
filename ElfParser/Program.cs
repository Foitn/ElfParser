using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElfParser
{
    class Program
    {





        static void Main(string[] args)
        {
            ElfParser ep = new ElfParser();
            ep.ParseFromFile(args[0]);
        }
    }
}
