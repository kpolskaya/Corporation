using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corporation;

namespace WpfCorp.ViewModel
{
    public class EmployeeViewModel 
    {
        readonly Employee employee;

        public EmployeeViewModel(Employee Employee)
        {
            this.employee = Employee;
        }

        public string FirstName { get { return this.employee.FirstName; } set { this.employee.FirstName = value; } }
        public string LastName { get { return this.employee.LastName; } set { this.employee.LastName = value; } }
        public uint Age { get { return this.employee.Age; } set { this.employee.Age = value; } }
        public Level Position { get { return this.employee.Position; } }
        public uint Id { get { return this.employee.Id; } }
        public decimal Wage { get { return this.employee.Wage; } }



    }
}
