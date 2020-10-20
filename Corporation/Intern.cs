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


        public override decimal Salary() { return 500; }

        //public override string ToString()
        //{
        //    return $"{this.Id,5}\t{this.FirstName,-10}{this.LastName,-15}{this.Position,-15}{this.Salary(),10: $### ##0.00}";
        //}
    }

}
