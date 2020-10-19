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

        public BossLevel MinBossLevel { get { return this.minBossLevel; } }

        public bool HeadIsVacant { get { return this.headIsVacant; } }
        
        public bool DeputyIsVacant { get { return this.deputyIsVacant; } }

        public ObservableCollection<Department> Childs { get; set; }

        public ObservableCollection<Employee> Panel { get; set; }

        public Department(string Name, BossLevel MinBossLevel)
        {
            this.Name = Name;
            this.id = Department.NextId();
            this.minBossLevel = MinBossLevel;
            this.headIsVacant = true;
            this.deputyIsVacant = this.minBossLevel == 0 ? false : true;
            this.Childs = new ObservableCollection<Department>();
            this.Panel = new ObservableCollection<Employee>();
        }
        


        //public void AddDepartmentToChilds(uint ChildId)
        //{
        //    this.childs.Add(ChildId);
        //}


        //public void AddEmploeeToPanel(uint EmId)
        //{
        //    this.panel.Add(EmId);
        //}


        /// <summary>
        /// Считает всю зарплату департамента и всех дочерних департаментов
        /// </summary>
        /// <returns></returns>
        public decimal TotalSalary()
        {
            decimal total = this.Salary(); 
            if (this.Childs.Count > 0)
            {
                foreach (var item in Childs)
                {
                    total += item.TotalSalary();
                }
            }
            return total;
        }

       /// <summary>
       /// Считает, какая зарплата должна быть установлена начальнику исходя из зарплаты подчиненных по всей иерархии
       /// </summary>
       /// <param name="lvl"></param>
       /// <returns></returns>
        public decimal BossSalary( BossLevel lvl)
        {
            decimal salary;
            salary = (TotalSalary() - SalaryOfEqualOrHigherLvl(lvl)) * 0.15m; //для операций с decimal, чтобы не было проблем с типами, нужно приводить все явно!!!

            return salary > 1300m ? salary : 1300m;
        }


        /// <summary>
        /// Вся зарплата только этого департамента
        /// </summary>
        /// <returns></returns>
        private decimal Salary()
        {
            decimal salary = 0;
            foreach (var item in this.Panel)
            {
                salary += item.Salary();
            }
            return salary;
        }

        /// <summary>
        /// Зарплата начальников департамента равного уровня или выше
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        private decimal SalaryOfEqualOrHigherLvl(BossLevel lvl) // 0 -высший уровень, 1 - ниже и так далее
        {
            decimal salary = 0;
            foreach (var item in this.Panel)
            {
                if (item.GetType() == typeof(Boss) && ((Boss)item).Lvl <= lvl)
                   
                    salary += item.Salary();
            }

            return salary;
        }

        private uint id;
        //private List<Department> childs;
        //private List<Employee> panel;
        private BossLevel minBossLevel;
        private bool headIsVacant;
        private bool deputyIsVacant;
        
    }
}
