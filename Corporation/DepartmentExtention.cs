using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    public static class DepartmentExtention
    {
        public static void PrintHierarchy(this Department dep, int tier = 0)
        {
            string indent = new string('\t', tier);
            Console.WriteLine(indent + dep);
            tier++;
            foreach (var item in dep.Children)
            {
               item.PrintHierarchy(tier);
            }
        }

        public static void PrintDepartmentPanel(this Department dep, int tier = 0)
        {
            string indent = "  " + new string('\t', tier);
           
            var sortedPanel = from p in dep.Staff 
                              orderby p.Position descending
                              select p;
            foreach (var p in sortedPanel)
            {
                Console.WriteLine(indent + p);
            }
            Console.WriteLine();
        }

        public static void PrintStaffHierarchy(this Department dep, int tier = 0)
        {
            string indent = new string('\t', tier);
            
            Console.WriteLine(indent + dep);
            dep.PrintDepartmentPanel(tier);
            tier++;
            foreach (var item in dep.Children)
            {
                item.PrintStaffHierarchy(tier);
            }
        }
    }
}
