using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyLib
{
    /// <summary>
    /// Класс, предоставляющий метод для сравнения сотрудников
    /// </summary>
    public class EmployeeComparer : IComparer<Employee>
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
            this.compareField = SortBy.Id;
        }

        public EmployeeComparer (SortBy Field)
        {
            this.compareField = Field;
        }

        /// <summary>
        /// Метод сравнения сотрудников в зависимости от выбанного поля для сортировки
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Employee x, Employee y)
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
                case SortBy.Position:
                    return x.Position.CompareTo(y.Position);
                case SortBy.Salary:
                    decimal xSalary = x.Salary();
                    decimal ySalary = y.Salary();
                    return xSalary.CompareTo(ySalary);
                default:
                    break;

            }
            return x.Id.CompareTo(y.Id);
        }
    }
}
