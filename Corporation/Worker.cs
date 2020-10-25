using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Worker : Employee
    {

        public uint Hours { get; set; }

        public Worker(string FirstName, string LastName, string Position, Department Department, uint Age, uint Hours)
        : base(FirstName, LastName, Position, Department, Age)

        {
            this.Hours = Hours;
        }

        public override decimal Salary()
        {
            return this.Hours * hourRate;
        }

    }
}
