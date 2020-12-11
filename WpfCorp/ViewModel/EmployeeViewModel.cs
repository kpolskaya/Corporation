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

        public string FirstName 
        { 
            get 
            { 
                return this.employee.FirstName; 
            } 
            set 
            { 
                if (value != this.employee.FirstName)
                {
                    this.employee.FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            } 
        }
        public string LastName 
        { 
            get 
            { 
                return this.employee.LastName; 
            } 
            set 
            { 
                if (value != this.employee.LastName)
                {
                    this.employee.LastName = value;
                    OnPropertyChanged("LastName");
                }
            } 
        }
        public uint Age 
        { 
            get 
            { 
                return this.employee.Age; 
            } 
            set 
            {
                if (value != this.employee.Age)
                {
                    this.employee.Age = value;
                    OnPropertyChanged("Age");
                }
            } 
        }
        public Level Position { get { return this.employee.Position; } 
        
            set
            {
                if (value != this.employee.Position)
                {
                    this.employee.Position = value;
                    OnPropertyChanged("Position");
                }
            }
        }
        
        public uint Id { get { return this.employee.Id; } }
        public decimal Salary { get { return this.employee.Salary(); } } 
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

        public void Refresh() //маленький хак, чтобы обновить DataContext. а как надо?
        {
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyNameOrDefault = null) 
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyNameOrDefault));
        }
    }
}
