using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
            maxChilds = 10;    //10;
            maxDepth = 4;  //4;
            maxStaff = 8;  //6;
        }
   
        public Department Board { get; set; }

        public Repository()
        {
            this.Board = new Department("Дирекция");
            CreateRandomCorp();
        }

        private void CreateRandomCorp()
        {
            this.Board.Panel.Add(new Boss("Лев", "Мышкин", Level.CEO, this.Board, 27));
            this.Board.Panel.Add(new Boss("Ипполит", "Терентьев", Level.CTO, this.Board, 20));
            if (maxDepth > 0)
                this.Board.CreateRandomChilds(maxChilds, maxDepth, maxStaff, 1);
        }

        public void SerializeDb()
        {
            string jsonString = "";

            jsonString += JsonConvert.SerializeObject(Board, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            //JObject o = JObject.FromObject(Board);
            //jsonString += o.ToString();
            Console.WriteLine(jsonString);
        }

                      
    }
    }
