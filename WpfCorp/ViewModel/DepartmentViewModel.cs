using System;
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
        ReadOnlyCollection<DepartmentViewModel> children;
        readonly Department department;
        readonly DepartmentViewModel parent;

        bool isExpanded;
        bool isSelected;

        public DepartmentViewModel(Department Department) : this(Department, null)
        {

        }

        private DepartmentViewModel(Department Department, DepartmentViewModel Parent)
        {
            this.department = Department;
            this.parent = Parent;
            // рекурсивно оборачиваем все нижестоящие департаменты в DepartmentViewModel класс
            this.children = new ReadOnlyCollection<DepartmentViewModel>(
                            (from child in this.department.Children
                             select new DepartmentViewModel(child, this))
                             .ToList<DepartmentViewModel>());
        }

        public ReadOnlyCollection<DepartmentViewModel> Children { get { return children; } }

        public string Name { get { return this.department.Name; } }
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

        public ReadOnlyCollection<Employee> Staff { get { return this.department.Staff; } }
        public DepartmentViewModel Parent { get { return this.parent; } }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
