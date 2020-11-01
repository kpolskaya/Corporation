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


            //Console.ReadKey();
            //company.SerializeDb();

            //Console.WriteLine(Uid.GetId());
            //Console.WriteLine(Uid.GetId());
            //Uid.InitId(100);
            //Console.WriteLine(Uid.GetId());
            //Uid.InitId(1000);
            //Console.WriteLine(Uid.GetId());
            
            
            
            Console.ReadKey();
            
            
        }
    }
}
