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
        public Boss(Person Person, Level Position, Department Department)
        : base(Person, Position, Department)
        {
            if (Position < Level.Product_Manager)
                throw new Exception("Должность не соответствует типу сотрудника");
        }

        [JsonConstructor]
        public Boss(uint Id, Person Person, Level Position, Department Department)
        : base(Id, Person, Position, Department)
        {
            if (Position < Level.Product_Manager)
                throw new Exception("Должность не соответствует типу сотрудника");
        }

        public override decimal Salary()
        {
            return (decimal)this.Department.GetBossSalaryOrNull(this.Position); 
        }
 
    }

    
}
