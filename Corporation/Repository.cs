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
    public class Repository
    {
        static string DefaultPath;
       
        static readonly int FirstTier;

        static Repository()
        {
            DefaultPath = @"db.json";
            FirstTier = 1;
        }
   
        public Department Board { get; set; }

        public Repository(int MaxChildren, int MaxDepth, int MaxStaff)
        {

            CreateRandomCorp(MaxChildren, MaxDepth, MaxStaff);
        }

        

        public void CreateRandomCorp(int maxChildren, int maxDepth, int maxStaff)
        {
            this.Board = new Department("Virtual Times Entertainment");
            this.Board.AddEmployee(new Boss("Лев", "Мышкин", 27, Level.CEO, this.Board)); // Как запретить добавлять кого угодно без валидации?
            this.Board.AddEmployee(new Boss("Ипполит", "Терентьев", 20, Level.CTO, this.Board));
            if (maxDepth > 0)
                this.Board.CreateRandomChildren(maxChildren, maxDepth, maxStaff, FirstTier);
        }

        public void SerializeDb(string path)
        {
            
            string jsonString = JsonConvert.SerializeObject(Board, Formatting.Indented); 

            File.WriteAllText(path, jsonString, Encoding.UTF8);
        }

        public Repository()
        {
            this.Board = new Department("Новый департамент");
        }

         public void Load(string path)
         {
            string jsonString = File.ReadAllText(path, Encoding.UTF8);
            JObject o = JObject.Parse(jsonString);

            this.Board = new Department(o.Value<uint>("Id"), o.Value<string>("Name"));

            IList<JToken> panel = o["Staff"].Children().ToList();
            foreach (var item in panel)
            {
                this.Board.AddEmployee(new Boss(item.Value<uint>("Id"), item.Value<string>("FirstName"), item.Value<string>("LastName"),
                    item.Value<uint>("Age"),
                    (Level)item.Value<byte>("Position"),
                    this.Board));
            }

            IList<JToken> grandChildren = o["Children"].Children().ToList();
            this.Board.RestoreChildren(grandChildren);
         }
    }
    }
