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
 
        public Intern(Person Person, Level Position, Department Department)
        : base(Person, Position, Department)
        {
            if (Position != Level.Intern)
                throw new Exception("Должность не соответствует типу сотрудника");
        }

        [JsonConstructor]
        public Intern(uint Id, Person Person, Level Position, Department Department) 
        : base(Id, Person, Position, Department)
        {
            if (Position != Level.Intern)
                throw new Exception("Должность не соответствует типу сотрудника");
        }

        public override decimal Salary() { return internSalary; }

    }

}
