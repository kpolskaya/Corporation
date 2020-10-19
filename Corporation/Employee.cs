using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    abstract class Employee
    {
        //--------Статические члены --------------
        static uint lastId;

        static Employee()
        {
           
            lastId = 0;
        }

        public static uint NextId()
        {
            return ++lastId;
        }

        //---------------------------------------

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
        public Department Department { get; set; }

        /// <summary>
        /// Табельный номер
        /// </summary>
        public uint Id { get { return id; } }


        /// <summary>
        /// Возраст
        /// </summary>
        public uint Age { get { return age; } }

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
            this.age = Age;
            this.id = Employee.NextId();
            Employee.lastId++;
            //this.salary = 0;
            
        }

        public override string ToString()
        {
            return $"{this.Id, 5}\t{this.FirstName, -10}{this.LastName, -15}{this.Position, -15}{this.Salary(), 10: $### ##0.00}"; 
        }

        /// <summary>
        /// Оплата труда
        /// </summary>
        public abstract decimal Salary();


        //public Division GetDepartment()
        //{ }

        protected uint age;
        protected uint id;
        //protected decimal salary;


    }






}
