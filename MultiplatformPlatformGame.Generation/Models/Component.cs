using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MultiplatformPlatformGame.Generation.Models
{
    [DataContract]
    public class Component
    {
        [DataMember(Name = "id")]
        public int ComponentId { get; set; }
        [DataMember(Name = "name")]
        public String Name { get; set; }
        [DataMember(Name = "texture")]
        public String Texture { get; set; }
        [DataMember(Name = "physics")]
        public Properties Physics { get; set; }
    }

    [DataContract]
    public class Properties
    {
        [DataMember(Name = "solid")]
        public bool Solid { get; set; }
    }
}
