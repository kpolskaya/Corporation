using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


namespace Corporation
{
    class Repository
    {
        static string DbPath;
       
        static readonly int FirstTier;

        static Repository()
        {
            DbPath = @"db.json";
            FirstTier = 1;
        }
   
        public Department Board { get; set; }

        public Repository(int MaxChildren, int MaxDepth, int MaxStaff)
        {
            
            this.Board = new Department("Дирекция");
            CreateRandomCorp(MaxChildren, MaxDepth, MaxStaff);
        }

        private void CreateRandomCorp(int maxChildren, int maxDepth, int maxStaff)
        {
            this.Board.Panel.Add(new Boss("Лев", "Мышкин", Level.CEO, this.Board, 27));
            this.Board.Panel.Add(new Boss("Ипполит", "Терентьев", Level.CTO, this.Board, 20));
            if (maxDepth > 0)
                this.Board.CreateRandomChildren(maxChildren, maxDepth, maxStaff, FirstTier);
        }

        public void SerializeDb()
        {
            string jsonString = "";

            jsonString += JsonConvert.SerializeObject(Board, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });


            File.WriteAllText(DbPath, jsonString, Encoding.UTF8);
            
        }

        public Repository()
        {
            string jsonString = File.ReadAllText(DbPath, Encoding.UTF8);
            JObject o = JObject.Parse(jsonString);

            this.Board = new Department(o.Value<uint>("Id"), o.Value<string>("Name"));

            IList<JToken> panel = o["Panel"].Children().ToList();
            foreach (var item in panel) 
            {
                this.Board.Panel.Add(new Boss(item.Value<uint>("Id"), item.Value<string>("FirstName"), item.Value<string>("LastName"), 
                    (Level)item.Value<byte>("Position"), this.Board, item.Value<uint>("Age")));
            }

            IList<JToken> grandChildren = o["Children"].Children().ToList();
            this.Board.RestoreChildren(grandChildren);

        }

    }
    }
