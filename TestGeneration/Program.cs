using System;
using MultiplatformPlatformGame.Generation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace TestGeneration
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Generator g = new Generator();
            g.AddComponent(@"../../test-files/empty-component.json");
            g.AddComponent(@"../../test-files/wall-component.json");
            g.AddBlock(@"../../test-files/corridor-block.json");
            g.AddBlock(@"../../test-files/well-block.json");
            g.AddBlock(@"../../test-files/plus-block.json");
            g.AddBlock(@"../../test-files/up-right-l-block.json");
            g.AddBlock(@"../../test-files/down-right-l-block.json");
            g.AddBlock(@"../../test-files/full-block.json");
            g.AddBlock(@"../../test-files/empty-block.json");

            g.AnalyseBlocks();
            var layout = g.GenerateLayout(15, 5);
            var level = g.AttributeBlocks(layout);
            foreach (Block b in g.GetBlocks()) {
                BlockPrettyPrint (b);
            }

            Console.WriteLine ("-----");

            Console.WriteLine(GetDisplayableLevel(layout));

            Console.WriteLine ("-----");

            LevelPrettyPrint(level);

            Console.Read();
        }

        static string GetDisplayableLevel(byte[,] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int line = 0; line < array.GetLength(0); line++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int column = 0; column < array.GetLength(1); column++)
                    {
                        Opening currentOpening = (Opening)array[line, column];
                        if (currentOpening == Opening.None)
                        {
                            sb.Append("   ");
                        }
                        else if (i == 0)
                        {
                            if ((currentOpening & Opening.Up) == Opening.Up)
                            {
                                sb.Append(" ^ ");
                            }
                            else {
                                sb.Append("   ");
                            }
                        }
                        else if (i == 1)
                        {
                            if ((currentOpening & Opening.Left) == Opening.Left)
                            {
                                sb.Append("<");
                            }
                            else
                            {
                                sb.Append(" ");
                            }

                            sb.Append("X");

                            if ((currentOpening & Opening.Right) == Opening.Right)
                            {
                                sb.Append(">");
                            }
                            else
                            {
                                sb.Append(" ");
                            }
                        }
                        else
                        {
                            if ((currentOpening & Opening.Down) == Opening.Down)
                            {
                                sb.Append(" v ");
                            }
                            else
                            {
                                sb.Append("   ");
                            }
                        }

                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
            
        public static void BlockPrettyPrint(Block block)
        {
            foreach (List<BlockComponent> l in block.ComponentMatrix) {
                foreach (BlockComponent bc in l) {
                    if (bc.Component.Solid) {
                        Console.Write ('X');
                    } else {
                        Console.Write (' ');
                    }
                }
                Console.WriteLine ();
            }
        }

        public static void LevelPrettyPrint(List<List<Block>> level)
        {
            int blockSize = level [0] [0].BlockSize;

            foreach (List<Block> bl in level) {
                for (int i = 0; i < blockSize; i++) {
                    foreach (Block b in bl) {
                        foreach (BlockComponent bc in b.ComponentMatrix[i]) {
                            if (bc.Component.Solid) {
                                Console.Write ('X');
                            } else {
                                Console.Write (' ');
                            }
                        }
                    }
                    Console.WriteLine ();
                }
            }
        }
    }
}
