using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komunikacja
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] danki = new int[5];
            danki[0] = 1;
            danki[1] = 2;
            danki[2] = 3;
            danki[3] = 4;
            danki[4] = 5;

            COM ff = new COM();
            ff.initialization();
            ff.Send(danki, 1);
            Console.WriteLine(ff.Read());
        }
    }
}