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

            g.AnalyseBlocks();
            var test = g.GenerateChunck(15, 5);

            Console.WriteLine(GetDisplayableLevel(test));
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
    }
}
