using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{   

    public class Department
    {
        static Random Randomize;

        static Department()
        {
            Randomize = new Random();
        }

        public string Name { get; private set; }
        public uint Id { get; private set; }

        public ObservableCollection<Department> Children { get; set; }

        public ReadOnlyObservableCollection<Employee> Staff { get; private set; } 
        
        public Department(string Name)
        {
            this.Name = Name;
            this.Id = GenerateId.Next();
            
            this.Children = new ObservableCollection<Department>();
            this.panel = new ObservableCollection<Employee>();
            this.Staff = new ReadOnlyObservableCollection<Employee>(panel);
        }
        [JsonConstructor]
        public Department(uint Id, string Name)
        {
            this.Name = Name;
            this.Id = Id;
            GenerateId.InitId(Id);
            this.Children = new ObservableCollection<Department>();
            this.panel = new ObservableCollection<Employee>();
            this.Staff = new ReadOnlyObservableCollection<Employee>(panel);
        }


        public override string ToString()
        {
            return $"{this.Id, 4 : 0000} {this.Name}";
        }

        public void RecruitPerson(string firstName, string lastName, Level position, uint age)
        {
            switch (position)
            {
                case Level.Intern:
                    this.panel.Add(new Intern(firstName, lastName, position, this, age));
                    break;
                case Level.Worker:
                    this.panel.Add(new Worker(firstName, lastName, position, this, age, Employee.initialHours));
                    break;
                case Level.CPO:
                    this.panel.Add(new Boss(firstName, lastName, position, this, age));
                    break;
                case Level.CTO:
                    this.panel.Add(new Boss(firstName, lastName, position, this, age));
                    break;
                case Level.CEO:
                    this.panel.Add(new Boss(firstName, lastName, position, this, age));
                    break;
                default:
                    throw new Exception("Неизвестная должность");
                    break;
            }
        }
               
        public void DismissEmployee(uint id)
        {
            try
            {
                this.panel.Remove(this.panel.First(p => p.Id == id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddEmployee (Employee employee)
        {
            if (employee.Department != null && employee.Department == this)
                this.panel.Add(employee);
            else
                throw new Exception("Добавляемый сотрудник не принадлежит данному департаменту");
        }

        public void RestoreChildren(IList<JToken> children)
        {
            Department child;
            Level position;
           
            foreach (var item in children)
            {
                child = new Department(item.Value<uint>("Id"), item.Value<string>("Name"));

                foreach (var person in item["Staff"]) // так можно было?
                {
                    position = (Level)person.Value<byte>("Position");
                    switch (position)
                    {
                        case Level.Intern:
                            child.panel.Add(new Intern(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                                person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        case Level.Worker:
                            child.panel.Add(new Worker(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                                person.Value<string>("LastName"), position, child, person.Value<uint>("Age"), person.Value<uint>("Hours")));
                            break;
                        case Level.CPO:
                            child.panel.Add(new Boss(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                               person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        case Level.CTO:
                            child.panel.Add(new Boss(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                               person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        case Level.CEO:
                            child.panel.Add(new Boss(person.Value<uint>("Id"), person.Value<string>("FirstName"),
                               person.Value<string>("LastName"), position, child, person.Value<uint>("Age")));
                            break;
                        default:
                            throw new Exception("Неизвестная должность");
                            break;
                    }
                }
                IList<JToken> grandChildren = item["Children"].Children().ToList(); 
                child.RestoreChildren(grandChildren);
                this.Children.Add(child);
            }
        }

        public void CreateRandomChildren(int maxChilds, int maxDepth, int maxStaff, int tier)
        {
            for (int i = 0; i < Randomize.Next(maxChilds < 0 ? 0 : maxChilds+1); i++)
            {
                this.Children.Add(new Department($"Отдел {tier}-{this.Id}-{i + 1}"));
                
                this.Children[i].RecruitPerson(Guid.NewGuid().ToString().Substring(0, 5),
                    Guid.NewGuid().ToString().Substring(0, 8), Level.CPO, (uint)Randomize.Next(20, 66));
               
                for (int j = 0; j < Randomize.Next(2, maxStaff < 1 ? 2 : maxStaff +1); j++)
                {
                    Level randomLevel = (Level)Randomize.Next(0, (int)Level.Worker + 1);
                    uint randomAge = (uint)Randomize.Next(18, 23 * ((int)randomLevel + 1));

                    this.Children[i].RecruitPerson(Guid.NewGuid().ToString().Substring(0, 5), 
                        Guid.NewGuid().ToString().Substring(0, 8), randomLevel, randomAge);
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
            for (int i = 0; i < tier; i++) //TODO избавиться от этих циклов
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
            var sortedPanel = from p in this.Staff //не panel - потому что идея убрать все эти методы печати в расширение класса
                              orderby p.Position descending
                              select p;
            foreach (var p in sortedPanel)
            {
                Console.WriteLine(indent + p);
            }
            Console.WriteLine();
        }

        public decimal BossSalary(Level lvl) //TODO проверка уровня
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
            foreach (var item in this.panel)
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

            foreach (var item in this.panel)
            {
                if (item.Position < lvl)
                    salary += item.Salary();
            }
            return salary;
        }

        private ObservableCollection<Employee> panel;
    }
}
