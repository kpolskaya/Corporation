using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CompanyLib;

namespace WpfCorp.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public Person Person { get; private set; }

        public PersonViewModel()
        {
            this.Person = new Person
            {
                FirstName = "Иван",
                LastName = "Петров",
                Age = 18
            };
        }

        public String FirstName 
        {
            get { return Person.FirstName; }

            set 
            {
                if (value != Person.FirstName)
                {
                    Person.FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        public String LastName
        {
            get { return Person.LastName; }

            set
            {
                if (value != Person.LastName)
                {
                    Person.LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }
        
        public uint Age
        {
            get { return Person.Age; }

            set
            {
                if (value != Person.Age)
                {
                    Person.Age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyNameOrDefault = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyNameOrDefault));
        }

        public void SetDefaults()
        {
            this.Person.FirstName = "Иван";
            this.Person.LastName = "Петров";
            this.Person.Age = 18;
            OnPropertyChanged();
        }
    }
}
