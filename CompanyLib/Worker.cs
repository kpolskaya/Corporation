using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyLib
{
    class Worker : Employee
    {

        public uint Hours { get; set; }

        public Worker(Person Person, Level Position, Department Department, uint Hours)
        : base(Person, Position, Department)
        {
            if (Position != Level.Worker)
                throw new Exception("Должность не соответствует типу сотрудника");
            this.Hours = Hours;
        }

        [JsonConstructor]
        public Worker(uint Id, Person Person, Level Position, Department Department, uint Hours)
        : base(Id, Person, Position, Department)
        {
            if (Position != Level.Worker)
                throw new Exception("Должность не соответствует типу сотрудника");
            this.Hours = Hours;
        }

        public override decimal Salary()
        {
            return this.Hours * hourRate;
        }

    }
}
