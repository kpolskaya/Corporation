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
   
        public Administration Board { get; set; }

        public Repository(int MaxChildren, int MaxDepth, int MaxStaff)
        {

            CreateRandomCorp(MaxChildren, MaxDepth, MaxStaff);
        }

        public void CreateRandomCorp(int maxChildren, int maxDepth, int maxStaff)
        {
            this.Board = new Administration("Virtual Times Entertainment");
            this.Board.AddEmployee(new Boss("Лев", "Мышкин", 27, Level.Director, this.Board)); 
            this.Board.AddEmployee(new Boss("Ипполит", "Терентьев", 20, Level.Deputy, this.Board));
            if (maxDepth > 0)
                this.Board.CreateRandomChildren(maxChildren, maxDepth, maxStaff, FirstTier);
        }

        public void Save(string path)
        {
            
            string jsonString = JsonConvert.SerializeObject(Board, Formatting.Indented); 

            File.WriteAllText(path, jsonString, Encoding.UTF8);
        }

        public Repository()
        {
            this.Board = new Administration("Новый департамент");
        }

         public void Load(string path)
         {
            string jsonString = File.ReadAllText(path, Encoding.UTF8);
            JObject o = JObject.Parse(jsonString);

            this.Board = new Administration(o.Value<uint>("Id"), o.Value<string>("Name"));

            IList<JToken> panel = o["Staff"].Children().ToList();
            foreach (var item in panel)
            {
                this.Board.AddEmployee(new Boss(item.Value<uint>("Id"), //если в администрации будут не только начальники - это не сработает!
                    item.Value<string>("FirstName"), 
                    item.Value<string>("LastName"),
                    item.Value<uint>("Age"),
                    (Level)item.Value<byte>("Position"),
                    this.Board));
            }

            IList<JToken> grandChildren = o["Children"].Children().ToList();
            this.Board.RestoreChildren(grandChildren);
         }
    }
    }
