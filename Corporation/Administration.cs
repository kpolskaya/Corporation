using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    /// <summary>
    /// Класс для административного департамента самого верхнего уровня
    /// </summary>
    public class Administration : Department
    {
        public Administration(string Name)
            : base(Name)
        {

        }

        public Administration(uint Id, string Name)
            : base(Id, Name)
        {

        }
        /// <summary>
        /// Создает наименование отдела с номером
        /// его порядковый номер
        /// </summary>
        /// <param name="suffix">Порядковый номер</param>
        /// <returns></returns>
        protected override string NameChild(int suffix)
        {
            return $"Отдел {suffix}.";
        }

        public override bool PositionAllowed(Level lvl)
        {
            return lvl > Level.Product_Manager;
        }

    }
}
