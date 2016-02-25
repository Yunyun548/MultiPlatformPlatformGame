using System;

namespace MultiplatformPlatformGame.Generation
{
	public class Block
	{
		public Block ()
		{
		}

		public int Id {get;set;}
		public String Name {get;set;}
		public int[] Matrix {get;set;}
		public Component[] ComponentMatrix {get;set;}
	}
}

