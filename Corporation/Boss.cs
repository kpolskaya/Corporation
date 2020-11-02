using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Boss : Employee
    {
        
        public Boss(string FirstName, string LastName, Level Position, Department Department, uint Age)
        : base(FirstName, LastName, Position, Department, Age)
        {
            
        }

        public override decimal Salary()
        {
            return this.Department.BossSalary(this.Position); 
        }

    }

}
