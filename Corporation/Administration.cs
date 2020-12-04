using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
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

        protected override string NameChild(int suffix)
        {
            return $"Отдел {suffix}.";
        }

        protected override bool PositionAllowed(Level lvl)
        {
            return lvl > Level.Product_Manager;
        }

    }
}
