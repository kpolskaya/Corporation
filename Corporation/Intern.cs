using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Intern : Employee
    {
        public Intern(string FirstName, string LastName, string Position, Department Department, uint Age)
        : base(FirstName, LastName, Position, Department, Age)
        {
           
        }

        public override decimal Salary() { return internSalary; }

    }

}
