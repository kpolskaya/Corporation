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
            Department div = new Department("Администрация", BossLevel.Deputy);

            div.Childs.Add(new Department("АХО", BossLevel.Head));
            div.Childs.Add(new Department("Бухгалтерия", BossLevel.Head));
            div.Childs.Add(new Department("Завод", BossLevel.Head));
            div.Childs[2].Childs.Add(new Department(" Цех № 1", BossLevel.Head));
            div.Childs[2].Childs.Add(new Department(" Цех № 2", BossLevel.Head));
            div.Childs[2].Childs.Add(new Department(" Цех № 3", BossLevel.Head));
            div.Childs[2].Childs[0].Childs.Add(new Department("Участок 11", BossLevel.Head));
            div.Childs[2].Childs[0].Childs.Add(new Department("Участок 12", BossLevel.Head));
            div.Childs[2].Childs[2].Childs.Add(new Department("Участок 31", BossLevel.Head));


            div.Panel.Add(new Boss("Александр", "Подовинников", "Самый Главный", div, 57, BossLevel.Head));
            div.Panel.Add(new Boss("Фёдор", "Попович", "Зам. Главного", div, 30, BossLevel.Deputy));
            
            div.Panel.Add(new Worker("Иван", "Лузер", "Девелопер", div, 30, 180));
            div.Panel.Add(new Worker("Степан", "Хренов", "Девелопер", div, 29, 180));
            div.Panel.Add(new Intern("Николай", "Патрушев", "Кодер", div, 29));


            div.PrintStaffHierarchy(0);

            //div.PrintHierarchy(0);

            //Console.WriteLine($"новый департамент {div.Id} | {div.Name} ");
            //foreach (var item in div.Childs)
            //{
            //    Console.WriteLine($"      + новый департамент {item.Id} | {item.Name} ");
            //}

            Console.ReadKey();
        }
    }
}
