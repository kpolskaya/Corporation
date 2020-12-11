using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Corporation
{
    class Worker : Employee
    {

        public uint Hours { get; set; }

        public Worker(Person Person, Level Position, Department Department, uint Hours)
        : base(Person, Position, Department)
        {
            this.Hours = Hours;
        }
        
        [JsonConstructor]
        public Worker(uint Id, Person Person, Level Position, Department Department, uint Hours)
        : base(Id, Person, Position, Department)
        {
            this.Hours = Hours;
        }

        public override decimal Salary()
        {
            return this.Hours * hourRate;
        }

    }
}
