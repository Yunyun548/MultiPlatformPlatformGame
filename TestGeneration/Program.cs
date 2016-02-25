using System;
using MultiplatformPlatformGame.Generation;
using Newtonsoft.Json;

namespace TestGeneration
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Generator g = new Generator ();
			g.AddComponent (@"../../test-files/component.json");
			g.AddBlock (@"../../test-files/block.json");

			g.AnalyseBlocks ();
			g.GenerateChunck ();
		}
	}
}
