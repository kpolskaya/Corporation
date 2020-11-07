﻿using System;
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
        CPO = 40,
        CTO = 70,
        CEO = 80
    }

    abstract class Employee
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
        public Level Position { get; protected set; }

        /// <summary>
        /// Отдел
        /// </summary>
        protected Department Department { get; set; } //поле должно быть закрытым, иначе jsonconverter будет уходить в вечный цикл!

        /// <summary>
        /// Табельный номер
        /// </summary>
        public uint Id { get; protected set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public uint Age { get;  set; }

        /// <summary>
        /// Создание сотрудника c автоматическим Id
        /// </summary>
        /// <param name="FirstName">Имя</param>
        /// <param name="LastName">Фамилия</param>
        /// <param name="Age">Возраст</param>
        /// <param name="Position">Должность</param>
        /// <param name="Department">Отдел</param>
        public Employee(string FirstName, string LastName, Level Position, Department Department, uint Age)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Position = Position;
            this.Department = Department;
            this.Age = Age;
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
