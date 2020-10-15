using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Department
    {
       //------------------------------         
        static uint lastId;
        

        static Department()
        {
            lastId = 0;
        }

        

        static uint NextId()
        {
            return ++lastId;
        }
        //-------------------------------
        
        public enum BossLevel : byte
        {
            Head = 0,
            Deputy = 1
        }


        public string Name { get; }
        public uint Id { get { return id; } }

        public BossLevel Level { get { return this.level; } }

        public List<Department> Childs { get { return this.childs; } }

        public List<Employee> Panel { get { return this.panel; } }

        public Department(string Name, BossLevel Level)
        {
            this.Name = Name;
            this.id = Department.NextId();
            this.level = Level;
            this.headIsVacant = true;
            this.deputyIsVacant = this.level == 0 ? true : false;
            this.childs = new List<Department>();
            this.panel = new List<Employee>();
        }
        


        //public void AddDepartmentToChilds(uint ChildId)
        //{
        //    this.childs.Add(ChildId);
        //}


        //public void AddEmploeeToPanel(uint EmId)
        //{
        //    this.panel.Add(EmId);
        //}

        public decimal TotalSalary()
        {
            decimal total = this.Salary(); 
            if (this.childs.Count > 0)
            {
                foreach (var item in childs)
                {
                    total += item.TotalSalary();
                }
            }
            return total;
        }

       
        public decimal BossSalary( BossLevel lvl)
        {
            decimal salary;
            salary = (TotalSalary() - SalaryOfEqualOrHigherLvl(lvl)) * 0.15m; //для операций с decimal, чтобы не было проблем с типами, нужно приводить все явно!!!

            return salary > 1300m ? salary : 1300m;
        }

        private decimal Salary()
        {
            decimal salary = 0;
            foreach (var item in panel)
            {
                salary += item.Salary;
            }
            return salary;
        }


        private decimal SalaryOfEqualOrHigherLvl(BossLevel lvl) // 0 -высший уровень, 1 - ниже и так далее
        {
            decimal salary = 0;
            foreach (var item in panel)
            {
                if (item.GetType() == typeof(Boss) && ((Boss)item).Lvl <= lvl)
                   
                    salary += item.Salary;
            }

            return salary;
        }

        private uint id;
        private List<Department> childs;
        private List<Employee> panel;
        private BossLevel level;
        private bool headIsVacant;
        private bool deputyIsVacant;
        
    }
}
