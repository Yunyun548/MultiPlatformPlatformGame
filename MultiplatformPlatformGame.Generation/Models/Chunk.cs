using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplatformPlatformGame.Generation.Models
{
    public class Chunk
    {
        public Block[,] Blocks { get; set; }

        public int StartBlock { get; set; }
        public Point? StartComponent { get; set; }

        public int EndBlock { get; set; }
        public IEnumerable<int> EndComponents { get; set; }
    }
}
