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
    public class Company
    {
          
        public Department Board { get; set; }

        public Company()
        {
            this.Board = new Administration("Новый департамент");
        }

        public Company(int MaxChildren, int MaxDepth, int MaxStaff) //удалить
        {

            CreateRandomCorp(MaxChildren, MaxDepth, MaxStaff);
        }

        public void CreateRandomCorp(int maxChildren, int maxDepth, int maxStaff)
        {
            this.Board = new Administration("Virtual Times Entertainment");
            this.Board.RecruitPerson(new Person("Лев", "Мышкин", 27), Level.Director); 
            this.Board.RecruitPerson(new Person("Ипполит", "Терентьев", 20), Level.Deputy);
            if (maxDepth > 0)
                this.Board.CreateRandomChildren(maxChildren, maxDepth, maxStaff);
        }

        public void Save(string path)
        {
            
            string jsonString = JsonConvert.SerializeObject(Board, Formatting.Indented); 

            File.WriteAllText(path, jsonString, Encoding.UTF8); 
        }

        public void Load(string path)
        {
            string jsonString = File.ReadAllText(path, Encoding.UTF8);
            JObject o = JObject.Parse(jsonString);

            this.Board = new Administration(o.Value<uint>("Id"), o.Value<string>("Name"));

            IList<JToken> panel = o["Staff"].Children().ToList();
            IList<JToken> descendants = o["Children"].Children().ToList();
            this.Board.Restore(panel, descendants);
        }
                
    }
}
