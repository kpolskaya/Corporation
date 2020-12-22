using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyLib
{
    /// <summary>
    /// Расширение класса Department для вывода информации
    /// в консоль
    /// </summary>
    public static class DepartmentExtention
    {
        /// <summary>
        /// Выводит в консоль всю иерархическую структуру,
        /// начиная с заданного департамента (без сотрудников)
        /// </summary>
        /// <param name="d">Корневой департамент</param>
        /// <param name="tier">Уровень в иерерхии - задает начальный отступ</param>
        public static void PrintHierarchy(this Department d, int tier = 0)
        {
            string indent = new string('\t', tier);
            Console.WriteLine(indent + d);
            tier++;
            foreach (var item in d.Children)
            {
                item.PrintHierarchy(tier);
            }
        }

        /// <summary>
        /// Выводит в консоль список сотрудников департамента
        /// </summary>
        /// <param name="d">Департамент</param>
        /// <param name="tier">Уровень департамента в иерархии - задает отступ</param>
        public static void PrintDepartmentPanel(this Department d, int tier = 0)
        {
            string indent = "  " + new string('\t', tier);

            var sortedPanel = from p in d.Staff
                              orderby p.Position descending
                              select p;
            foreach (var p in sortedPanel)
            {
                Console.WriteLine(indent + p);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Выводит в консоль всю иерархическую структуру
        /// включая сотрудников департаментов, начиная с заданного департамента
        /// </summary>
        /// <param name="d">Департамент</param>
        /// <param name="tier">Уровень в иерерхии - задает начальный отступ</param>
        public static void PrintStaffHierarchy(this Department d, int tier = 0)
        {
            string indent = new string('\t', tier);

            Console.WriteLine(indent + d);
            d.PrintDepartmentPanel(tier);
            tier++;
            foreach (var item in d.Children)
            {
                item.PrintStaffHierarchy(tier);
            }
        }
    }
}
