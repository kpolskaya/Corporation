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
       //--------Статические члены-----         
        static uint lastId;

        static Department()
        {
            lastId = 0;
        }

        static uint NextId()
        {
            return ++lastId;
        }
        //-------------------------------

        public string Name { get; }
        public uint Id { get; private set; }

        public BossLevel MinBossLevel { get; private set; }

        //public bool HeadIsVacant { get; set; }
        
        //public bool DeputyIsVacant { get; set; }

        public ObservableCollection<Department> Childs { get; set; }

        public ObservableCollection<Employee> Panel { get; set; }

        public Department(string Name, BossLevel MinBossLevel)
        {
            this.Name = Name;
            this.Id = Department.NextId();
            this.MinBossLevel = MinBossLevel;
            //this.HeadIsVacant = true;
            //this.DeputyIsVacant = this.MinBossLevel == BossLevel.Head ? false : true;
            this.Childs = new ObservableCollection<Department>();
            this.Panel = new ObservableCollection<Employee>();
        }

        public override string ToString()
        {
            return $"{this.Id,4} {this.Name}";
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
        }

        public decimal BossSalary(BossLevel lvl)
        {
            decimal salaryBasis = SubalternSalary();

            if (lvl < this.MinBossLevel) // чем меньше числовое значение уровня, тем круче босс
            {
                for (BossLevel l = lvl + 1; l <= this.MinBossLevel; l++)
                {
                    salaryBasis += this.Panel.First((a) => {
                        return (a.GetType() == typeof(Boss) && ((Boss)a).Lvl == l);
                    }).Salary();                                                        // Предполагается, что в департаменте должен быть только один босс каждого уровня,
                                                                                        // или надо поставить foreach. Что будет, если ни одного не найдется? Херня будет.
                                                                                        // TODO !!!
                }
            }

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
        /// Вся зарплата только этого департамента ниже босса
        /// </summary>
        /// <returns></returns>
        private decimal SubalternSalary()  
        {
            decimal salary = 0;
            foreach (var item in this.Panel)
            {
                if (item.GetType() != typeof(Boss))
                salary += item.Salary();
            }
            return salary;
        }
                
    }
}
