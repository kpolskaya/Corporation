using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    abstract class Employee
    {
        static uint lastId;
        public static decimal minBossSalary;
        public static decimal hourRate;
        public static decimal internSalary;
        public static decimal bossSalaryProportion;

        static Employee()
        {
            lastId = 0;
            minBossSalary = 1300m;
            hourRate = 12m;
            internSalary = 500;
            bossSalaryProportion = 0.15m;
        }

        public static uint NextId()
        {
            return ++lastId;
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Отдел
        /// </summary>
        public Department Department { get; protected set; }

        /// <summary>
        /// Табельный номер
        /// </summary>
        public uint Id { get; protected set; }


        /// <summary>
        /// Возраст
        /// </summary>
        public uint Age { get; protected set; }

        /// <summary>
        /// Создание сотрудника со всеми полями
        /// </summary>
        /// <param name="FirstName">Имя</param>
        /// <param name="LastName">Фамилия</param>
        /// <param name="Age">Возраст</param>
        /// <param name="Position">Должность</param>
        /// <param name="Department">Отдел</param>
        
        public Employee(string FirstName, string LastName, string Position, Department Department, uint Age)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Position = Position;
            this.Department = Department;
            this.Age = Age;
            this.Id = Employee.NextId();
                        
        }
 
        /// <summary>
        /// Оплата труда
        /// </summary>
        public abstract decimal Salary();

        public override string ToString()
        {
            return $"{this.Id, 5 : 00000}\t{this.FirstName,-10}{this.LastName,-15}{this.Position,-15}\t{this.Salary(), 10: $### ##0.00}";
        }
       
    }

}
