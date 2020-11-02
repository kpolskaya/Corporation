using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Program
    {
        static void Main(string[] args)
        {

            Repository company = new Repository();

            company.Board.PrintStaffHierarchy(0);
            Console.ReadKey();

            company.SerializeDb();

            Console.ReadKey();
            
            
        }
    }
}
