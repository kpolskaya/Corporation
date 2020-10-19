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
        public Department.BossLevel Lvl { get { return this.lvl; } }


        public Boss(string FirstName, string LastName, string Position, Department Department, uint Age, Department.BossLevel Lvl)
        : base(FirstName, LastName, Position, Department, Age)
        {
            if ((Lvl == Department.BossLevel.Head && Department.HeadIsVacant) ||(Lvl == Department.BossLevel.Deputy && Department.DeputyIsVacant))
            {
                this.lvl = Lvl;
                
            }
            
            else throw new Exception("Должность {Lvl} занята");
        }


                
        public override decimal Salary()
        {
            return this.Department.BossSalary(this.lvl); 
        }


        private Department.BossLevel lvl;
    }


}
