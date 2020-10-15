using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Boss : Employee
    {
        public Department.BossLevel Lvl { get { return this.lvl; } }


        public Boss(string FirstName, string LastName, string Position, Department Department, uint Age, Department.BossLevel Lvl)
        : base(FirstName, LastName, Position, Department, Age)
        {
            this.lvl = Lvl;
            SetSalary(Department);
        }


                
        public void SetSalary( Department department)
        {
            this.salary = department.BossSalary(this.lvl); 
        }


        private Department.BossLevel lvl;
    }


}
