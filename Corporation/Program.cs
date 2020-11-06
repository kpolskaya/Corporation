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

            Repository company = new Repository(6, 5, 8);

            company.Board.PrintStaffHierarchy(0);
           
            company.SerializeDb();

            Repository clone = new Repository();
            clone.Board.PrintStaffHierarchy(0);

            Console.ReadKey();
            
            
        }
    }
}
