using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplatformPlatformGame.Generation
{
    public class Component
    {
		public Component()
		{
		}

		public int Id { get; set; }
		public String Name { get; set; }
		public String Texture { get; set; }
		public Object Physics { get; set; }
    }
}
