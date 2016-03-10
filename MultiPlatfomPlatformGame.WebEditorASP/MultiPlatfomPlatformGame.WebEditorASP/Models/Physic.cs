using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiPlatfomPlatformGame.WebEditorASP.Models
{
    public class Physic
    {
        public Physic() {
            Solid = false;
            Destructible = false;
        }

        [JsonProperty("Solid")]
        public bool Solid { get; set; }
        [JsonProperty("Destructible")]
        public bool Destructible { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}