﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{   

    class Department
    {
        static Random Randomize;

        static Department()
        {
            Randomize = new Random();
        }

        public string Name { get; }
        public uint Id { get; private set; }


        public ObservableCollection<Department> Children { get; set; }

        public ObservableCollection<Employee> Panel { get; set; }

        
        public Department(string Name)
        {
            this.Name = Name;
            this.Id = GenerateId.Next();
            
            this.Children = new ObservableCollection<Department>();
            this.Panel = new ObservableCollection<Employee>();
        }
        [JsonConstructor]
        public Department(uint Id, string Name)
        {
            this.Name = Name;
            this.Id = Id;
            GenerateId.InitId(Id);
            this.Children = new ObservableCollection<Department>();
            this.Panel = new ObservableCollection<Employee>();
        }


        public override string ToString()
        {
            return $"{this.Id, 4 : 0000} {this.Name}";
        }

        public void RestoreChildren(IList<JToken> children)
        {
            Department child;
            Level position;
            int i = 0; // итератор списка дочерних департаментов TODO переделать foreach на нормальный for
            
            foreach (var item in children)
            {
                child = new Department(item.Value<uint>("Id"), item.Value<string>("Name"));

                foreach (var person in item["Panel"]) // так можно было?
                {
                    position = (Level)person.Value<byte>("Position");
                    switch (position)
                    {
                        case Level.Intern:
                            child.Panel.Add(new Intern(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                                person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        case Level.Worker:
                            child.Panel.Add(new Worker(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                                person.Value<string>("LastName"), position, child, person.Value<uint>("Age"), person.Value<uint>("Hours")));
                            break;
                        case Level.CPO:
                            child.Panel.Add(new Boss(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                               person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        case Level.CTO:
                            child.Panel.Add(new Boss(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                               person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        case Level.CEO:
                            child.Panel.Add(new Boss(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                               person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        default:
                            break;
                    }

                }

                IList<JToken> grandChildren = children[i++]["Children"].Children().ToList();
                child.RestoreChildren(grandChildren);
                this.Children.Add(child);
            }
        }

        public void CreateRandomChildren(int maxChilds, int maxDepth, int maxStaff, int tier)
        {
            for (int i = 0; i < Randomize.Next(maxChilds < 0 ? 0 : maxChilds+1); i++)
            {
                this.Children.Add(new Department($"Отдел {tier}-{this.Id}-{i + 1}"));
                this.Children[i].Panel.Add(new Boss(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), Level.CPO, this.Children[i], (uint)Randomize.Next(20, 66)));

                for (int j = 0; j < Randomize.Next(2, maxStaff < 1 ? 2 : maxStaff +1); j++)
                {
                    switch (Randomize.Next(0,2))
                    {
                        case 0 :
                            this.Children[i].Panel.Add(new Worker(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), Level.Worker, this.Children[i], (uint)Randomize.Next(20, 46), 190));
                            break;
                        case 1:
                            this.Children[i].Panel.Add(new Intern(Guid.NewGuid().ToString().Substring(0, 5), Guid.NewGuid().ToString().Substring(0, 8), Level.Intern, this.Children[i], (uint)Randomize.Next(18, 21)));
                            break;

                        default:
                            break;
                    }
                }

                if (maxDepth > 1)
                    this.Children[i].CreateRandomChildren(maxChilds - tier - 1, maxDepth - 1, maxStaff, tier + 1);
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
            foreach (var item in this.Children)
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
            foreach (var item in this.Children)
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

            var sortedPanel = from p in this.Panel
                              orderby p.Position descending
                              select p;
            foreach (var p in sortedPanel)
            {
                Console.WriteLine(indent + p);
            }

            Console.WriteLine();
        }

        public decimal BossSalary(Level lvl)
        {
            decimal salaryBasis = SubalternSalary(lvl); 

            foreach (var item in this.Children)
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
            if (this.Children.Count > 0)
            {
                foreach (var item in Children)
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
            
            return salary;
        }
    }
}
