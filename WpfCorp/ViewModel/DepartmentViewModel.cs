﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Corporation;

namespace WpfCorp.ViewModel
{
    public class DepartmentViewModel : INotifyPropertyChanged
    {
        ObservableCollection<DepartmentViewModel> children;
        readonly Department department;
        readonly DepartmentViewModel parent;
        

        bool isExpanded;
        bool isSelected;
        
        //ObservableCollection<EmployeeViewModel> staff;

        public DepartmentViewModel(Department Department) : this(Department, null)
        {

        }

        private DepartmentViewModel(Department Department, DepartmentViewModel Parent)
        {
            this.department = Department;
            this.parent = Parent;
            // рекурсивно оборачиваем все нижестоящие департаменты в DepartmentViewModel класс
            this.children = new ObservableCollection<DepartmentViewModel>(
                            
                (from child in this.department.Children
                select new DepartmentViewModel(child, this))
                .ToList<DepartmentViewModel>());

            //this.staff = InitPanel();
        }

        public ReadOnlyObservableCollection<EmployeeViewModel> Staff { get { return new ReadOnlyObservableCollection<EmployeeViewModel>(InitPanel()); } }
        public DepartmentViewModel Parent { get { return this.parent; } }

        public ObservableCollection<DepartmentViewModel> Children { get { return children; } } //readonly?

        public string Name { get { return this.department.Name; } 
            set {
                if (value != this.department.Name)
                {
                    this.department.Name = value;
                    OnPropertyChanged("Name");
                } 
            } 
        }
        public string Id { get { return this.department.Id.ToString(); } }

        

       /// <summary>
       /// Читает и устанавливает триггер объекта в соответствии с  
       /// его представлением в TreeViewItem: развернуто или свернуто
       /// </summary>
       public bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                if (value != this.isExpanded)
                    this.isExpanded = value;
                // дальше нужно рекурсивно развернуть все узлы наверх
                if (this.isExpanded && this.parent != null)
                    this.parent.IsExpanded = true;

            }
        }

        /// <summary>
        /// Читает и устанавливает триггер объекта, в соответствии с 
        /// с его представлением в TreeViewItem: выбран или не выбран
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public void RecruitPerson(Person person, Level position)
        {
            this.department.RecruitPerson(person, position);
            this.OnPropertyChanged("Staff");
        }

        internal void CreateDepartment() 
        {
            Department newDepartment = new Department("Новый департамент");
            this.department.Children.Add(newDepartment);
            DepartmentViewModel newDepartmentVM = new DepartmentViewModel(newDepartment, this);
            this.children.Add(newDepartmentVM);
            if (!IsExpanded)
                this.IsExpanded = true;
            OnPropertyChanged("IsExpanded");
            OnPropertyChanged("Children");
        }

        internal void Remove(DepartmentViewModel selectedItem)
        {
            this.department.Children.Remove(selectedItem.department);
            this.children.Remove(selectedItem);
            OnPropertyChanged("Children");
        }

        public void DismissEmployee(EmployeeViewModel employee)
        {
            this.department.DismissEmployee(employee.Id);
            this.OnPropertyChanged("Staff");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        /// <summary>
        ///  Оборачивает всех сотрудников  этого департамента в EmployeeViewModel
        /// </summary>
        /// <returns>Возвращает коллекцию сотрудников департамента, попутно отсортированную по должности</returns>
        private ObservableCollection<EmployeeViewModel> InitPanel() 
        {
            return new ObservableCollection<EmployeeViewModel>(
                            
                (from employee in this.department.Staff
                 orderby employee.Position descending
                 select new EmployeeViewModel(employee))
                .ToList<EmployeeViewModel>());
        }
    }
}
