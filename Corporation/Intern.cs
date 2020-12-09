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
        public Intern(string FirstName, string LastName, uint Age, Level Position, Department Department)
        : base(FirstName, LastName, Age, Position, Department)
        {
            
        }

        public Intern(Person Person, Level Position, Department Department)
        : base(Person, Position, Department)
        {

        }

        [JsonConstructor]
        public Intern(uint Id, Person Person, Level Position, Department Department) 
        : base(Id, Person, Position, Department)
        {
           
        }

        public override decimal Salary() { return internSalary; }

    }

}
