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
            Department div = new Department("Администрация", Department.BossLevel.Deputy);

            div.Childs.Add(new Department("АХО", Department.BossLevel.Head));

            Console.WriteLine($"новый департамент {div.Id} | {div.Name} ");
            foreach (var item in div.Childs)
            {
                Console.WriteLine($"      + новый департамент {item.Id} | {item.Name} ");
            }

            Console.ReadKey();
        }
    }
}
