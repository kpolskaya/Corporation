using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation
{
    class Repository
    {
        static string DbPath;
        //static Random Randomize;
        static int maxChilds;
        static int maxDepth;
        static int maxStaff;

        static Repository()
        {
            DbPath = @"db.xml";
            //Randomize = new Random();
            maxChilds = 10;
            maxDepth = 4;
            maxStaff = 6;
        }
   
        public Department Board { get; set; }

        public Repository()
        {
            this.Board = new Department("Дирекция", BossLevel.Deputy);
            CreateRandomCorp();
        }

        private void CreateRandomCorp()
        {
            this.Board.Panel.Add(new Boss("Лев", "Мышкин", "Генеральный директор", this.Board, 27, BossLevel.Head));
            this.Board.Panel.Add(new Boss("Ипполит", "Терентьев", "Заместитель директора", this.Board, 20, BossLevel.Deputy));
            this.Board.CreateRandomChilds(maxChilds, maxDepth, maxStaff, 1);
        }



    }
    }
