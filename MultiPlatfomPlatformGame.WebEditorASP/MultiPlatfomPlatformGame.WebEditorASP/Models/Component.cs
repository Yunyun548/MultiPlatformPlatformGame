using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiPlatfomPlatformGame.WebEditorASP.Models
{
    public class Component
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String TexturePath { get; set; }
        public Properties Physics { get; set; }
        public Component()
        {

        }

        public Component(int id, string name, string texturePath, string physics)
        {
            this.Id = id;
            this.Name = name;
            this.TexturePath = texturePath;
            this.Physics = JsonConvert.DeserializeObject<Properties>(physics);
        }
    }
}