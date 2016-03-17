using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiPlatfomPlatformGame.WebEditorASP.Models
{
    public class Component
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public String Name { get; set; }
        [JsonProperty("TexturePath")]
        public String TexturePath { get; set; }
        [JsonProperty("Physics")]
        public Physic Physics { get; set; }

        public Component()
        {
            this.Physics = new Physic();
        }

        public Component(int id, string name, string texturePath, string physics)
        {
            this.Id = id;
            this.Name = name;
            this.TexturePath = texturePath;
            this.Physics = JsonConvert.DeserializeObject<Physic>(physics);
        }

        public string Serialize()
        {           
            return JsonConvert.SerializeObject(this);
        } 
    }
}