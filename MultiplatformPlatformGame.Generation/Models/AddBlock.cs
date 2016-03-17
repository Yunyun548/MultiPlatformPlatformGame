using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplatformPlatformGame.Generation.Models
{
    public class AddBlock
    {
        public Block[,] Blocks { get; set; }
        public Point? StartPoint { get; set; }
        public IEnumerable<int> EndPoints { get; set; }
    }
}
