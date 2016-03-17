using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;

namespace MultiPlatfomPlatformGame.WebEditorASP.Models
{
    public class Block
    {
        public int Id { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("components")]
        public string Components { get; set; }

        public Block()
        {
                
        }

        public Block BlockDeserilizer(string jsonData)
        {
           
           // Block newBlock = new JavaScriptSerializer().Deserialize<Block>(jsonData);
            Block newBlock = JsonConvert.DeserializeObject<Block>(jsonData);
            return newBlock;
        }
    }
}