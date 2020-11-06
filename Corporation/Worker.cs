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

        public Worker(string FirstName, string LastName, Level Position, Department Department, uint Age, uint Hours)
        : base(FirstName, LastName, Position, Age)

        {
            this.department = Department;
            this.Hours = Hours;
            this.Id = GenerateId.Next();
        }
        
        [JsonConstructor]
        public Worker(uint Id, string FirstName, string LastName, Level Position, Department Department, uint Age, uint Hours)
            : base(FirstName, LastName, Position, Age)
        {
            this.Id = Id;
            this.department = Department;
            GenerateId.InitId(Id);
            this.Hours = Hours;
        }

        public override decimal Salary()
        {
            return this.Hours * hourRate;
        }

        private Department department;


    }
}
