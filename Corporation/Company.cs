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
    /// <summary>
    /// Класс для работы со всей компанией
    /// </summary>
    public class Company
    {
        //Департамент самого верхнего уровня  
        public Department Board { get; set; }

        public Company()
        {
            this.Board = new Administration("Новый департамент");
        }

        /// <summary>
        /// Создает новую компанию с сотрудниками случайным образом 
        /// </summary>
        /// <param name="maxChildren">Максимальное количество дочерних департаментов на первом уровне</param>
        /// <param name="maxDepth">Максимальная вложенность департаментов</param>
        /// <param name="maxStaff">Максимальное количество сотрудников в каждом департаменте (не считая начальников)</param>
        public void CreateRandom(int maxChildren, int maxDepth, int maxStaff)
        {
            this.Board = new Administration("Virtual Times Entertainment");
            
            this.Board.RecruitPerson(RandomPerson.Next(31, 70), Level.Director); 
            this.Board.RecruitPerson(RandomPerson.Next(31, 70), Level.Deputy);
            if (maxDepth > 0)
                this.Board.CreateRandom(maxChildren, maxDepth, maxStaff);
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
