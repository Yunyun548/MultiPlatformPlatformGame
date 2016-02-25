using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public bool Solid {
			get { return Physics.Solid; }
			set { Physics.Solid = value; }
		}
    }
}
