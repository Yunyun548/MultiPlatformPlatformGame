using System;

namespace MultiplatformPlatformGame.Generation
{
	public class BlockComponent
	{
		public BlockComponent (Component c, Block b, int x, int y)
		{
			this.Component = c;
			this.Block = b;
			this.X = x;
			this.Y = y;
		}

		public Component Component { get; set; }
		public Block Block { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}
}

