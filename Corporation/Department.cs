using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{   public enum BossLevel : byte
        {
            Head = 0,
            Deputy = 1
        }

    class Department
    {
        static uint lastId;
        static Random Randomize;

        static Department()
        {
            lastId = 0;
            Randomize = new Random();
        }

        static uint NextId()
        {
            return ++lastId;
        }

        public string Name { get; }
        public uint Id { get; private set; }

        public BossLevel MinBossLevel { get; private set; }

        public ObservableCollection<Department> Childs { get; set; }

        public ObservableCollection<Employee> Panel { get; set; }

        public Department(string Name, BossLevel MinBossLevel)
        {
            this.Name = Name;
            this.Id = Department.NextId();
            this.MinBossLevel = MinBossLevel;
            this.Childs = new ObservableCollection<Department>();
            this.Panel = new ObservableCollection<Employee>();
        }

        public override string ToString()
        {
            return $"{this.Id, 4 : 0000} {this.Name}";
        }

        public void CreateRandomChilds(int maxChilds, int maxDepth, int maxStaff, int tier)
        {
            for (int i = 0; i < Randomize.Next(maxChilds+1); i++)
            {
                this.Childs.Add(new Department($"Отдел_{tier}_{this.Id}_{i + 1}", BossLevel.Head));
                this.Childs[i].Panel.Add(new Boss(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), "Начальник", this.Childs[i], (uint)Randomize.Next(20, 66), BossLevel.Head));

                for (int j = 0; j < Randomize.Next(2,maxStaff+1); j++)
                {
                    switch (Randomize.Next(0,2))
                    {
                        case 0 :
                            this.Childs[i].Panel.Add(new Worker(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), "Специалист", this.Childs[i], (uint)Randomize.Next(20, 46), 190));
                            break;
                        case 1:
                            this.Childs[i].Panel.Add(new Intern(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), "Стажер", this.Childs[i], (uint)Randomize.Next(18, 21)));
                            break;

                        default:
                            break;
                    }
                }

                if (maxDepth > 1)
                    this.Childs[i].CreateRandomChilds(maxChilds - tier - 1, maxDepth - 1, maxStaff, tier + 1);
                
            }
        }

        /// <summary>
        /// Рекурсивно выводит структуру департаментов
        /// </summary>
        /// <param name="tier">Отступ, с которого нужно начинать печать</param>
        public void PrintHierarchy(int tier)
        {
            string indent = "";
            for (int i = 0; i < tier; i++)
            {
                indent += "\t";
            }
            Console.WriteLine(indent + this);
            tier++;
            foreach (var item in this.Childs)
            {
                item.PrintHierarchy(tier);
            }
        }

        public void PrintStaffHierarchy(int tier)
        {
            string indent = "";
            for (int i = 0; i < tier; i++)
            {
                indent += "\t";
            }
            Console.WriteLine(indent + this);
            PrintDepartmentPanel(tier);
            tier++;
            foreach (var item in this.Childs)
            {
                item.PrintStaffHierarchy(tier);
            }
        }

        public void PrintDepartmentPanel(int tier)
        {
            string indent = "  ";
            for (int i = 0; i < tier; i++)
            {
                indent += "\t";
            }

            //много повторяющегося кода - можно сделать сортировку по позиции и выводить одним циклом
            foreach (var item in this.Panel)
            {
                if (item.GetType() == typeof(Boss) && ((Boss)item).Lvl == BossLevel.Head)
                    Console.WriteLine(indent + (Boss)item);
            }
            foreach (var item in this.Panel)
            {
                if (item.GetType() == typeof(Boss) && ((Boss)item).Lvl == BossLevel.Deputy)
                    Console.WriteLine(indent + (Boss)item);
            }
            foreach (var item in this.Panel)
            {
                if (item.GetType() == typeof(Worker))
                    Console.WriteLine(indent + (Worker)item);
            }
            foreach (var item in this.Panel)
            {
                if (item.GetType() == typeof(Intern))
                    Console.WriteLine(indent + (Intern)item);
            }
            Console.WriteLine();
        }

        public decimal BossSalary(BossLevel lvl)
        {
            decimal salaryBasis = SubalternSalary( lvl); //DONE переделать SubalternSalary() чтобы принимала параметр босса и убрать туда блок кода ниже

            //if (lvl < this.MinBossLevel) // чем меньше числовое значение уровня, тем круче босс -> да, если считаем зарплату для босса главнее, чем возможен в этом департаменте
            //{
            //    foreach (var item in this.Panel)
            //    {
            //        if (item.GetType() == typeof(Boss) && ((Boss)item).Lvl > lvl)
            //            salaryBasis += item.Salary();
            //    }
            //}

            foreach (var item in this.Childs)
            {
                salaryBasis += item.TotalSalary();
            }

            return salaryBasis * Employee.bossSalaryProportion > Employee.minBossSalary ? 
                   salaryBasis * Employee.bossSalaryProportion : Employee.minBossSalary;
        }

        /// <summary>
        /// Считает всю зарплату департамента и всех дочерних департаментов
        /// </summary>
        /// <returns></returns>
        public decimal TotalSalary()
        {
            decimal total = 0;
            foreach (var item in this.Panel)
            {
                total += item.Salary();
            }
            if (this.Childs.Count > 0)
            {
                foreach (var item in Childs)
                {
                    total += item.TotalSalary();
                }
            }
            return total;
        }

        /// <summary>
        /// Вся зарплата только этого департамента ниже босса указанного уровня
        /// </summary>
        /// <returns></returns>
        private decimal SubalternSalary( BossLevel lvl)  
        {
            decimal salary = 0;
            foreach (var item in this.Panel)
            {
                if (item.GetType() != typeof(Boss) || ((Boss)item).Lvl > lvl)
                salary += item.Salary();
            }
            return salary;
        }
    }
}
