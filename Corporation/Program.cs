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

            Company company = new Company(10, 5, 8);

            company.Board.PrintStaffHierarchy();
           
            

            Console.ReadKey();
            
            
        }
    }
}
