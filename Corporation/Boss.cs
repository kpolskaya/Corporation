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
        public BossLevel Lvl { get; private set; }


        public Boss(string FirstName, string LastName, string Position, Department Department, uint Age, BossLevel Lvl)
        : base(FirstName, LastName, Position, Department, Age)
        {
            this.Lvl = Lvl;
        }

        public override decimal Salary()
        {
            return this.Department.BossSalary(this.Lvl); 
        }

    }

}
