using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Corporation
{   

    public class Department
    {
        static Random Randomize;

        static Department()
        {
            Randomize = new Random();
        }

        public string Name { get; set; }
        public uint Id { get; private set; }

        public ObservableCollection<Department> Children { get; private set; }

        public ReadOnlyObservableCollection<Employee> Staff { get; private set; }

        private ObservableCollection<Employee> panel;

        public Department(string Name)
        {
            this.Name = Name;
            this.Id = GlobalId.Next();
            
            this.Children = new ObservableCollection<Department>();
            this.panel = new ObservableCollection<Employee>();
            this.Staff = new ReadOnlyObservableCollection<Employee>(panel);
        }
        [JsonConstructor]
        public Department(uint Id, string Name)
        {
            this.Name = Name;
            this.Id = Id;
            GlobalId.InitId(Id);
            this.Children = new ObservableCollection<Department>();
            this.panel = new ObservableCollection<Employee>();
            this.Staff = new ReadOnlyObservableCollection<Employee>(panel);
        }

        public override string ToString()
        {
            return $"{this.Id, 4 : 0000} {this.Name}";
        }

        public void RecruitPerson(Person person, Level position)
        {
            if (!PositionAllowed(position))
                throw new Exception("Недопустимая должность");
            switch (position)
            {
                case Level.Intern:
                    this.panel.Add(new Intern(person, position, this));
                    break;
                case Level.Worker:
                    this.panel.Add(new Worker(person, position, this, Employee.initialHours));
                    break;

                case Level.Product_Manager:
                case Level.Deputy:
                case Level.Director:
                    this.panel.Add(new Boss(person, position, this));
                    break;
                default:
                    throw new Exception("Неизвестная должность");
            }

        }
        
        public void RecruitPerson(string firstName, string lastName, uint age, Level position)
        {
            if (!PositionAllowed(position))
                throw new Exception("Недопустимая должность");
            switch (position)                                                               
            {
                case Level.Intern:
                    this.panel.Add(new Intern(firstName, lastName, age, position, this));
                    break;
                case Level.Worker:
                    this.panel.Add(new Worker(firstName, lastName, age, position, this, Employee.initialHours));
                    break;

                case Level.Product_Manager: case Level.Deputy: case Level.Director:
                    this.panel.Add(new Boss(firstName, lastName, age, position, this));
                    break;
                default:
                    throw new Exception("Неизвестная должность");
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
                throw new Exception("Сотрудника с таким Id в отделе не существует");
            }
        }

        public void AddEmployee (Employee employee)
        {
            
            if (employee.Department != null 
                && employee.Department == this 
                && PositionAllowed(employee.Position)) //может здесь эта проверка на должность не нцжна?
                
                this.panel.Add(employee);
            else
                throw new Exception("Недопустимый отдел или должность");
        }

        public void RestoreChildren(IList<JToken> children)
        {
            Department child;
            Level position;
           
            foreach (var item in children)
            {
                child = new Department(item.Value<uint>("Id"), item.Value<string>("Name"));

                foreach (var employee in item["Staff"]) 
                {
                    position = (Level)employee.Value<byte>("Position"); 
                    switch (position)
                    {
                        case Level.Intern:
                            child.panel.Add(new Intern(employee.Value<uint>("Id"), employee.Value<string>("FirstName"),
                                employee.Value<string>("LastName"), employee.Value<uint>("Age"), position, child));
                            break;
                        case Level.Worker:
                            child.panel.Add(new Worker(employee.Value<uint>("Id"), employee.Value<string>("FirstName"),
                                employee.Value<string>("LastName"), employee.Value<uint>("Age"), 
                                position, child, employee.Value<uint>("Hours")));
                            break;
                        case Level.Product_Manager: case Level.Deputy:  case Level.Director:
                            child.panel.Add(new Boss(employee.Value<uint>("Id"), employee.Value<string>("FirstName"),
                               employee.Value<string>("LastName"), employee.Value<uint>("Age"), position, child));
                            break;
                        default:
                            throw new Exception("Неизвестная должность");
                    }
                }
                IList<JToken> descendants = item["Children"].Children().ToList(); 
                child.RestoreChildren(descendants);
                this.Children.Add(child);
            }
        }

        public void CreateRandomChildren(int maxChildren, int maxDepth, int maxStaff)
        {
            for (int i = 0; i < Randomize.Next(maxChildren < 0 ? 0 : maxChildren+1); i++)
            {
                this.Children.Add(new Department(NameChild(i+1)));
                
                this.Children[i].RecruitPerson(Guid.NewGuid().ToString().Substring(0, 5),
                    Guid.NewGuid().ToString().Substring(0, 8), (uint)Randomize.Next(20, 66), Level.Product_Manager);
               
                for (int j = 0; j < Randomize.Next(2, maxStaff < 1 ? 2 : maxStaff +1); j++)
                {
                    Level randomLevel = (Level)Randomize.Next(0, (int)Level.Worker + 1);
                    uint randomAge = (uint)Randomize.Next(18, 23 * ((int)randomLevel + 1));

                    this.Children[i].RecruitPerson(Guid.NewGuid().ToString().Substring(0, 5), 
                        Guid.NewGuid().ToString().Substring(0, 8), randomAge, randomLevel);
                }
                if (maxDepth > 1)
                    this.Children[i].CreateRandomChildren(maxChildren - 1, maxDepth - 1, maxStaff);
            }
        }

        protected virtual string NameChild(int suffix)
        {
            return $"{this.Name} {suffix}.";
        }

        public decimal BossSalary(Level lvl) //TODO проверка уровня if lvl < XXX throw...
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

        public virtual bool PositionAllowed(Level lvl)
        {
            return (lvl < Level.Deputy);
        }
    }
}
