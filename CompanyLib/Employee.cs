using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyLib
{
    
    /// <summary>
    /// Должности сотрудников
    /// </summary>
    public enum Level : byte
    {
        Intern = 0,
        Worker = 1,
        Product_Manager = 40,
        Deputy = 70,
        Director = 80
    }

    /// <summary>
    /// Базовый абстракнтный класс для сотрудников
    /// </summary>
    public abstract class Employee : Person, IComparable<Employee>
    {
        public static decimal minBossSalary;        //минимальный оклад начальника
        public static decimal hourRate;             //часовая ставка работника
        public static decimal internSalary;         //оклад интерна
        public static decimal bossSalaryProportion; //доля от суммы зарплат нижестоящих сотрудников, составляющая оклад начальника
        public static uint initialHours;            //стандартная часовая загрузка работника

        static Employee()
        {
            minBossSalary = 2300m;
            hourRate = 12m;
            internSalary = 500m;
            bossSalaryProportion = 0.15m;
            initialHours = 165;
        }

        public Level Position { get; private set; }

        /// <summary>
        /// Отдел (не сериализуемое свойство - для исключения циклической ссылки)
        /// </summary>
        [JsonIgnore]
        public Department Department { get; private set; }

        public uint Id { get; protected set; }

        /// <summary>
        /// Создание сотрудника c автоматическим Id
        /// </summary>
        public Employee(Person Person, Level Position, Department Department)
        : base(Person.FirstName, Person.LastName, Person.Age)
        {
            this.Position = Position;
            this.Department = Department;
            this.Id = GlobalId.Next();
        }
        /// <summary>
        /// Создание сотрудника с существующим ID (при десериализации)
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Person"></param>
        /// <param name="Position"></param>
        /// <param name="Department"></param>
        public Employee(uint Id, Person Person, Level Position, Department Department)
        : base(Person.FirstName, Person.LastName, Person.Age)
        {
            this.Position = Position;
            this.Department = Department;
            this.Id = Id;
            GlobalId.InitId(Id);
        }

        /// <summary>
        /// Оплата труда
        /// </summary>
        public abstract decimal Salary();

        public override string ToString()
        {
            return $"{this.Id,5: 00000}\t{this.FirstName,-10}{this.LastName,-15}{this.Age,3}\t{this.Position.ToString(),-15}\t{this.Salary(),10: $### ##0.00}";
        }

        public int CompareTo(Employee other)
        {
            return this.CompareTo(other);
        }
    }
   
}
