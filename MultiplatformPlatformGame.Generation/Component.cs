using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MultiplatformPlatformGame.Generation
{
    [DataContract]
    public class Component
	{
        [DataContract]
		public class Properties
		{
			public Properties()
			{
			}
            [DataMember(Name = "solid")]
			public bool Solid { get; set; }
		}

		public Component()
		{
		}
        [DataMember(Name ="id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public String Name { get; set; }
        [DataMember(Name = "texture")]
        public String Texture { get; set; }
        [DataMember(Name = "physics")]
        public Properties Physics { get; set; }

        public bool Solid {
			get { return Physics.Solid; }
			set { Physics.Solid = value; }
		}
    }
}
