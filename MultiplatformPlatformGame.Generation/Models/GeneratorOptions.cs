using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplatformPlatformGame.Generation.Models
{
    public class GeneratorOptions
    {
        public int ChunkWidth { get; set; }
        public int ChunkHeight { get; set; }
        public bool OpenEnd { get; set; }
        public Chunk LastChunk { get; set; }
    }
}
