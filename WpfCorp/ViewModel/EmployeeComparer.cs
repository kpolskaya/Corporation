using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyLib;

namespace WpfCorp.ViewModel
{
    /// <summary>
    /// Класс, предоставляющий метод для сравнения представления сотрудников
    /// </summary>
    public class EmployeeComparer : IComparer<EmployeeViewModel>
    {
        /// <summary>
        /// Перечислитель полей для сортировки
        /// </summary>
        public enum SortBy
        {
            Id,
            FirstName,
            LastName,
            Age,
            Position,
            Salary
        }

        private SortBy compareField;

        public EmployeeComparer()
        {
            this.compareField = SortBy.Position;
        }

        public EmployeeComparer(SortBy Field)
        {
            this.compareField = Field;
        }

        /// <summary>
        /// Метод сравнения сотрудников в зависимости от выбанного поля для сортировки
        /// </summary>
        /// <param name="x">Предоставление первого сотрудника</param>
        /// <param name="y">Представление второго сотрудника</param>
        /// <returns></returns>
        public int Compare(EmployeeViewModel x, EmployeeViewModel y)
        {
            switch (compareField)
            {
                case SortBy.Id:
                    return x.Id.CompareTo(y.Id);

                case SortBy.FirstName:
                    return x.FirstName.CompareTo(y.FirstName);

                case SortBy.LastName:
                    return x.LastName.CompareTo(y.LastName);

                case SortBy.Age:
                    return x.Age.CompareTo(y.Age);
               
                case SortBy.Salary:
                    return x.Salary.CompareTo(y.Salary);
               
                case SortBy.Position:
                default:
                    return y.Position.CompareTo(x.Position);
            }
        }
    }
}
