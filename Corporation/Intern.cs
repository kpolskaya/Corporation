using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Intern : Employee
    {
        public Intern(string FirstName, string LastName, Level Position, Department Department, uint Age)
            : base(FirstName, LastName, Position, Department, Age)
        {
            this.Id = GenerateId.Next();
        }

        [JsonConstructor]
        public Intern(uint Id, string FirstName, string LastName, Level Position, Department Department, uint Age) 
            : base(FirstName, LastName, Position, Department, Age)
        {
            this.Id = Id;
            GenerateId.InitId(Id);
        }

        public override decimal Salary() { return internSalary; }

    }

}
