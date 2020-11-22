﻿using System;
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
        
        public Boss(string FirstName, string LastName, uint Age, Level Position, Department Department)
        : base(FirstName, LastName, Age, Position, Department)
        {
            
        }

        [JsonConstructor]
        public Boss(uint Id, string FirstName, string LastName, uint Age, Level Position, Department Department)
            : base(Id, FirstName, LastName, Age, Position, Department)
        {
           
        }

        public override decimal Salary()
        {
            return this.Department.BossSalary(this.Position); 
        }
 
    }

    
}
