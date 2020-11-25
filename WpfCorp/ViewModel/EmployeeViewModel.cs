using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corporation;

namespace WpfCorp.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged 
    {
        readonly Employee employee;
        bool isSelected;

        public EmployeeViewModel(Employee Employee)
        {
            this.employee = Employee;
        }

        public string FirstName { get { return this.employee.FirstName; } private set { this.employee.FirstName = value; } }
        public string LastName { get { return this.employee.LastName; } private set { this.employee.LastName = value; } }
        public uint Age { get { return this.employee.Age; } private set { this.employee.Age = value; } }
        public Level Position { get { return this.employee.Position; } }
        public uint Id { get { return this.employee.Id; } }
        public decimal Wage { get { return this.employee.Wage; } } // можно  убрать это свойство из Employee - есть же Salary()

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }
        

        public void ApplyNewData(string newFirstName, string newLastName, uint newAge)
        {
            this.employee.FirstName = newFirstName;
            this.employee.LastName = newLastName;
            this.employee.Age = newAge;
            OnPropertyChanged("");
        }

        public void Refresh() //маленький хак, чтобы обновить DataContext. а как надо?
        {
            this.IsSelected = false;
            this.IsSelected = true;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) // попробовать сделать необязательный параметр с дефолтным null
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
