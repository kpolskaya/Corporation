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
{   

    class Department
    {
        //static uint lastId { get; set; }
        static Random Randomize;

        static Department()
        {
            //lastId = 0;
            Randomize = new Random();
        }

        //static uint NextId()
        //{
        //    return ++lastId;
        //}

        public string Name { get; }
        public uint Id { get; private set; }

       

        public ObservableCollection<Department> Childs { get; set; }

        public ObservableCollection<Employee> Panel { get; set; }

        public Department(string Name)
        {
            this.Name = Name;
            this.Id = Uid.GetId();
            //this.Id = Department.NextId();
           
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
                this.Childs.Add(new Department($"Отдел {tier}-{this.Id}-{i + 1}"));
                this.Childs[i].Panel.Add(new Boss(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), Level.CPO, this.Childs[i], (uint)Randomize.Next(20, 66)));

                for (int j = 0; j < Randomize.Next(2,maxStaff+1); j++)
                {
                    switch (Randomize.Next(0,2))
                    {
                        case 0 :
                            this.Childs[i].Panel.Add(new Worker(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), Level.Worker, this.Childs[i], (uint)Randomize.Next(20, 46), 190));
                            break;
                        case 1:
                            this.Childs[i].Panel.Add(new Intern(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), Level.Intern, this.Childs[i], (uint)Randomize.Next(18, 21)));
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

            //DONE много повторяющегося кода - можно сделать сортировку по позиции и выводить одним циклом
            var sortedPanel = from p in this.Panel
                              orderby p.Position descending
                              select p;
            foreach (var p in sortedPanel)
            {
                Console.WriteLine(indent + p);
            }


            //foreach (var item in this.Panel)
            //{
            //    if (item.GetType() == typeof(Boss) && ((Boss)item).Lvl == BossLevel.Head)
            //        Console.WriteLine(indent + (Boss)item);
            //}
            //foreach (var item in this.Panel)
            //{
            //    if (item.GetType() == typeof(Boss) && ((Boss)item).Lvl == BossLevel.Deputy)
            //        Console.WriteLine(indent + (Boss)item);
            //}
            //foreach (var item in this.Panel)
            //{
            //    if (item.GetType() == typeof(Worker))
            //        Console.WriteLine(indent + (Worker)item);
            //}
            //foreach (var item in this.Panel)
            //{
            //    if (item.GetType() == typeof(Intern))
            //        Console.WriteLine(indent + (Intern)item);
            //}
            Console.WriteLine();
        }

        public decimal BossSalary(Level lvl)
        {
            decimal salaryBasis = SubalternSalary(lvl); 

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
        private decimal SubalternSalary( Level lvl)  
        {
            decimal salary = 0;

            foreach (var item in this.Panel)
            {
                if (item.Position < lvl)
                    salary += item.Salary();
            }

            //foreach (var item in this.Panel)
            //{
            //    if (item.GetType() != typeof(Boss) || ((Boss)item).Lvl > lvl)
            //        salary += item.Salary();
            //}
            return salary;
        }
    }
}
