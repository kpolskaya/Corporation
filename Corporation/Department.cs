using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Corporation
{   
    /// <summary>
    /// Класс для департаментов общего назначения
    /// </summary>
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

        private ObservableCollection<Employee> staff;

        public Department(string Name)
        {
            this.Name = Name;
            this.Id = GlobalId.Next();
            
            this.Children = new ObservableCollection<Department>();
            this.staff = new ObservableCollection<Employee>();
            this.Staff = new ReadOnlyObservableCollection<Employee>(staff);
        }
        [JsonConstructor]
        public Department(uint Id, string Name)
        {
            this.Name = Name;
            this.Id = Id;
            GlobalId.InitId(Id);
            this.Children = new ObservableCollection<Department>();
            this.staff = new ObservableCollection<Employee>();
            this.Staff = new ReadOnlyObservableCollection<Employee>(staff);
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
                    this.staff.Add(new Intern(person, position, this));
                    break;
                case Level.Worker:
                    this.staff.Add(new Worker(person, position, this, Employee.initialHours));
                    break;
                case Level.Product_Manager:
                case Level.Deputy:
                case Level.Director:
                    this.staff.Add(new Boss(person, position, this));
                    break;
                default:
                    throw new Exception("Неизвестная должность");
            }
        }
               
        public void DismissEmployee(uint id)
        {
            try
            {
                this.staff.Remove(this.staff.First(p => p.Id == id));
            }
            catch (Exception)
            {
                throw new Exception("Сотрудника с таким Id в отделе не существует");
            }
        }
  
        /// <summary>
        ///  Восстановление иерархической структуры компании начиная с данного департамента
        ///  из предварительно прочитанного и обработанного json-формата 
        /// </summary>
        /// <param name="staff">персонал этого департамента</param>
        /// <param name="children">дочерние департаменты</param>
        public void Restore(IList<JToken> staff, IList<JToken> children)
        {
            foreach (var employee in staff)
            {
                Level position = (Level)employee.Value<byte>("Position");

                Person person = new Person  (employee.Value<string>("FirstName"),
                                            employee.Value<string>("LastName"), 
                                            employee.Value<uint>("Age"));
                switch (position)
                {
                    case Level.Intern:
                        this.staff.Add(new Intern(employee.Value<uint>("Id"), person, 
                            position, this));
                        break;

                    case Level.Worker:
                        this.staff.Add(new Worker(employee.Value<uint>("Id"), person,
                            position, this, employee.Value<uint>("Hours")));
                        break;

                    case Level.Product_Manager:
                    case Level.Deputy:
                    case Level.Director:
                        this.staff.Add(new Boss(employee.Value<uint>("Id"), person, 
                            position, this));
                        break;

                    default:
                        throw new Exception("Неизвестная должность");
                }
            }
            foreach (var item in children)
            {
                Department child = new Department(item.Value<uint>("Id"), item.Value<string>("Name"));
                this.Children.Add(child);
                IList<JToken> descendants = item["Children"].Children().ToList();
                IList<JToken> panel = item["Staff"].Children().ToList();
                child.Restore(panel, descendants);
            }
        }

        /// <summary>
        /// Создает случайную иерархическую структуру компании
        /// и наполняет ее случайными людьми
        /// </summary>
        /// <param name="maxChildren"></param>
        /// <param name="maxDepth"></param>
        /// <param name="maxStaff"></param>
        public void CreateRandom(int maxChildren, int maxDepth, int maxStaff)
        {

            for (int i = 0; i < Randomize.Next(maxChildren < 0 ? 0 : maxChildren+1); i++)
            {
                this.Children.Add(new Department(NameChild(i+1)));
               
                this.Children[i].RecruitPerson( RandomPerson.Next(), Level.Product_Manager);
               
                for (int j = 0; j < Randomize.Next(2, maxStaff < 1 ? 2 : maxStaff +1); j++)
                {
                    Level randomLevel = (Level)Randomize.Next(0, (int)Level.Worker + 1);
                    
                    this.Children[i].RecruitPerson(
                        RandomPerson.Next(18 + (uint)randomLevel * 2, 25 * ((uint)randomLevel +1)), 
                        randomLevel);
                }
                if (maxDepth > 1)
                    this.Children[i].CreateRandom(maxChildren - 1, maxDepth - 1, maxStaff);
            }
        }
        
        /// <summary>
        /// Создает наименование отдела, добавляя в конец 
        /// его порядковый номер
        /// </summary>
        /// <param name="suffix">Порядковый номер</param>
        /// <returns></returns>
        protected virtual string NameChild(int suffix)
        {
            return $"{this.Name} {suffix}.";
        }
        
        /// <summary>
        /// Расчитывает заработную плату босса
        /// </summary>
        /// <param name="lvl">уровень босса</param>
        /// <returns>Зарплата босса. Исключения - задан уровень ниже уровня босса</returns>
        public decimal GetBossSalary(Level lvl) 
        {
            if (lvl < Level.Product_Manager)
                throw new Exception($"Для позиции {lvl}  данный метод неприменим");
            
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
            foreach (var item in this.staff)
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

            foreach (var item in this.staff)
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
