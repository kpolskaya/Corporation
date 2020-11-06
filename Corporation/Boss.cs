using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Corporation
{
    class Boss : Employee
    {
        
        public Boss(string FirstName, string LastName, Level Position, Department Department, uint Age)
        : base(FirstName, LastName, Position, Age)
        {
            this.department = Department;
            this.Id = GenerateId.Next();
        }

        [JsonConstructor]
        public Boss(uint Id, string FirstName, string LastName, Level Position, Department Department, uint Age)
            : base(FirstName, LastName, Position, Age)
        {
            this.Id = Id;
            this.department = Department;
            GenerateId.InitId(Id);
        }

        public override decimal Salary()
        {
            return this.department.BossSalary(this.Position); 
        }

        private Department department;
    }

}
