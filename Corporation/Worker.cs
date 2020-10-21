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
           
        }

        public override decimal Salary()
        {
            return this.hours * hourRate;
        }

        
        private uint hours;

    }
}
