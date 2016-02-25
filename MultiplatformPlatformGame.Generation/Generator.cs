using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MultiplatformPlatformGame.Generation
{
	public class Generator
	{
		public Generator ()
		{
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
			foreach(Block b in blocks) {
				blocks.BindComponents (components);
				AnalyseBlock (b);
			}
		}

		protected void AnalyseBlock(Block b)
		{
		}

		public void GenerateChunck()
		{
		}
	}
}

