using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corporation;

namespace WpfCorp.ViewModel
{
    /// <summary>
    /// Дополнительная обертка для департамента - нужна для специфических методов на уровне корпорации,
    /// также позволит работать с данными, если на верхнем уровне будет не один, а несколько департаментов
    /// (можно будет создать массив объектов - представлений департаментов и передать его как корневой элемент)
    /// </summary>
    public class CorporationViewModel
    {
        DepartmentViewModel board;

        public CorporationViewModel(Department Board)
        {
            this.board = new DepartmentViewModel (Board);
        }

        DepartmentViewModel Board { get { return board; } }

    }
}
