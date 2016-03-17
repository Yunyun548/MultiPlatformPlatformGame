using System;
using MultiplatformPlatformGame.Generation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using MultiplatformPlatformGame.Generation.Models;

namespace TestGeneration
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Generator g = new Generator(4, 4);
            g.AddComponent(@"../../test-files/empty-component.json");
            g.AddComponent(@"../../test-files/wall-component.json");
            g.AddBlock(@"../../test-files/corridor-block.json");
            g.AddBlock(@"../../test-files/well-block.json");
            g.AddBlock(@"../../test-files/plus-block.json");
            g.AddBlock(@"../../test-files/up-right-l-block.json");
            g.AddBlock(@"../../test-files/down-right-l-block.json");
            g.AddBlock(@"../../test-files/full-block.json");
            g.AddBlock(@"../../test-files/empty-block.json");

            TestChunkGeneration(g, 15, 5, 4, 4, 1);

            Console.Read();
        }

        public static void TestChunkGeneration(Generator gen, int chunkWidth, int chunkHeight, int blockWidth, int blockHeight, int chunkNb)
        {
            Chunk lastChunk = null;
            for (int i = 0; i < chunkNb; i++)
            {
                GeneratorOptions op = new GeneratorOptions
                {
                    ChunkHeight = chunkHeight,
                    ChunkWidth = chunkWidth,
                    LastChunk = lastChunk,
                    OpenEnd = i < chunkNb - 1
                };
                
                bool speed = false;
                var chunk = gen.GenerateChunk(op, str =>
                {
                    Console.WriteLine(str);
                    if (!speed)
                    {
                        var key = Console.ReadKey(true);
                        if (key.KeyChar == 'q')
                            speed = true;
                    }
                });

                //var chunk = gen.GenerateChunk(op);
                //Console.WriteLine(gen.GetDisplayableChunk(chunk));

                lastChunk = chunk;
            }
        }
    }
}
