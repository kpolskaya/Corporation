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
        public BossLevel Lvl { get { return this.lvl; } }


        public Boss(string FirstName, string LastName, string Position, Department Department, uint Age, BossLevel Lvl)
        : base(FirstName, LastName, Position, Department, Age)
        {
            if ((Lvl == BossLevel.Head && Department.HeadIsVacant) || (Lvl == BossLevel.Deputy && Department.DeputyIsVacant))
            {
                this.lvl = Lvl;
                //TODO сбросить флаг вакансии босса
            }

            else throw new Exception($"Должность {Lvl} занята");
        }

        //public override string ToString()
        //{
        //    return $"{this.Id,5}\t{this.FirstName,-10}{this.LastName,-15}{this.Position,-15}{this.Salary(),10: $### ##0.00}";
        //}

        public override decimal Salary()
        {
            return this.Department.BossSalary(this.lvl); 
        }


        public BossLevel lvl; //TODO свойство
    }


}
