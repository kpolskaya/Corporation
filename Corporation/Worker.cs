using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Worker : Employee
    {
        public Worker(string FirstName, string LastName, string Position, Department Department, uint Age, uint Hours)
        : base(FirstName, LastName, Position, Department, Age)

        {
            this.hours = Hours;
            SetSalary();
        }

        public  void SetSalary()
        {
            this.salary = this.hours * 12;
        }
        private uint hours;

    }
}
