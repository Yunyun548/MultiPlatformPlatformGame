using System;
using System.Collections.Generic;

namespace MultiplatformPlatformGame.Generation
{
    public class Component
	{
		public class Properties
		{
			public Properties()
			{
			}

			public bool Solid { get; set; }
		}

		public Component()
		{
		}

		public int Id { get; set; }
		public String Name { get; set; }
		public String Texture { get; set; }
		public Properties Physics { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public bool Solid {
			get { return Physics.Solid; }
			set { Physics.Solid = value; }
		}
    }
}
