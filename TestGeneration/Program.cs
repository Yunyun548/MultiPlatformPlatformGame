using System;
using MultiplatformPlatformGame.Generation;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestGeneration
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Generator g = new Generator ();
			g.AddComponent (@"../../test-files/empty-component.json");
			g.AddComponent (@"../../test-files/wall-component.json");
			g.AddBlock (@"../../test-files/corridor-block.json");
			g.AddBlock (@"../../test-files/well-block.json");
			g.AddBlock (@"../../test-files/plus-block.json");
			g.AddBlock (@"../../test-files/up-right-l-block.json");
			g.AddBlock (@"../../test-files/down-right-l-block.json");

			g.AnalyseBlocks ();
			g.GenerateChunck ();
		}
	}
}
