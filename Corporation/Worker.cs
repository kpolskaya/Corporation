﻿using System;
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

        public Worker(string FirstName, string LastName, uint Age, Level Position, Department Department, uint Hours)
        : base(FirstName, LastName, Age, Position, Department)

        {
            this.Hours = Hours;
           
        }
        
        [JsonConstructor]
        public Worker(uint Id, string FirstName, string LastName, uint Age, Level Position, Department Department, uint Hours)
            : base(Id, FirstName, LastName, Age, Position, Department)
        {
            
            this.Hours = Hours;
        }

        public override decimal Salary()
        {
            return this.Hours * hourRate;
        }

    }
}
