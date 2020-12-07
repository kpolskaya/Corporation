using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    /// <summary>
    /// Базовый класс для нанятых сотрудников
    /// </summary>
    public class Person
    {
        
        public string FirstName { get; set; }

        public string LastName { get; set; }
       
        public uint Age { get; set; }

        public Person (string FirstName, string LastName, uint Age)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Age = Age;

        }

        public Person()
        {

        }
    }
}
