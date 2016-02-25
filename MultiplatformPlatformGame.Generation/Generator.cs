using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MultiplatformPlatformGame.Generation
{
	public class Generator
	{
		public Generator ()
		{
			components = new List<Component> ();
			blocks = new List<Block> ();
		}

		private List<Component> components;
		private List<Block> blocks;

		public void AddComponent(String filePath)
		{
			Component c = JsonConvert.DeserializeObject<Component> (System.IO.File.ReadAllText(filePath));
			components.Add (c);
		}

		public void AddBlock(String filePath)
		{
			Block b = JsonConvert.DeserializeObject<Block> (System.IO.File.ReadAllText(filePath));
			blocks.Add (b);
		}

		public void AnalyseBlocks()
		{
			foreach (Block b in blocks) {
				b.BindComponents (components);
				AnalyseBlock (b);
			}
		}

		protected void AnalyseBlock(Block b)
		{
			foreach (BlockComponent c in BorderNonSolidComponents(b)) {
			}
		}

		protected IEnumerable<BlockComponent> BorderComponents(Block b)
		{
			for (int i = 0; i < b.BlockSize; i++) {
				if (i == 0 || i == b.BlockSize) {
					foreach (BlockComponent c in b.ComponentMatrix[i]) {
						yield return c;
					}
				} else {
					yield return b.ComponentMatrix [i] [0];
					yield return b.ComponentMatrix [i] [b.BlockSize - 1];
				}
			}
		}

		protected IEnumerable<BlockComponent> BorderNonSolidComponents(Block b)
		{
			foreach (BlockComponent c in BorderComponents(b)) {
				if (!c.Component.Solid) {
					yield return c;
				}
			}
		}

		public void GenerateChunck()
		{
		}
	}
}

