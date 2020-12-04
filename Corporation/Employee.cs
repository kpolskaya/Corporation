using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
   
    public enum Level : byte
    {
        Intern = 0,
        Worker = 1,
        Product_Manager = 40,
        Deputy = 70,
        Director = 80
    }

    public abstract class Employee : Person
    {
        public static decimal minBossSalary;
        public static decimal hourRate;
        public static decimal internSalary;
        public static decimal bossSalaryProportion;
        public static uint initialHours;

        static Employee() // TODO запретить создание работников с несоответствующей типу позицией! И вообще убрать отсюда эти статические члены в репо или департамент
        {
            minBossSalary = 1300m;
            hourRate = 12m;
            internSalary = 500m;
            bossSalaryProportion = 0.15m;
            initialHours = 190;
        }
       
         public Level Position { get;  set; }

        /// <summary>
        /// Отдел (не сериализуемое свойство - для исключения циклической ссылки)
        /// </summary>
        [JsonIgnore]
        public Department Department { get; private set; } 
       
        public uint Id { get; protected set; }
      
        /// <summary>
        /// Создание сотрудника c автоматическим Id
        /// </summary>
        /// <param name="FirstName">Имя</param>
        /// <param name="LastName">Фамилия</param>
        /// <param name="Age">Возраст</param>
        /// <param name="Position">Должность</param>
        /// <param name="Department">Отдел</param>
        public Employee(string FirstName, string LastName, uint Age, Level Position, Department Department) :
            base(FirstName, LastName, Age)
        {
            this.Position = Position;
            this.Department = Department;
            this.Id = GlobalId.Next();
        }

        /// <summary>
        /// Конструктор для создания сотрудника с существующим Id - из файла
        /// </summary>
        /// <param name="Id">Табельный номер</param>
        /// <param name="FirstName">Имя</param>
        /// <param name="LastName">Фамилия</param>
        /// <param name="Age">Возраст</param>
        /// <param name="Position">Должжность</param>
        /// <param name="Department">Отдел</param>
        public Employee(uint Id, string FirstName, string LastName, uint Age, Level Position, Department Department) :
            base(FirstName, LastName, Age)
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
            return $"{this.Id, 5 : 00000}\t{this.FirstName,-10}{this.LastName,-15}{this.Age, 3}\t{this.Position.ToString(),-15}\t{this.Salary(), 10: $### ##0.00}";
        }
    }
}
