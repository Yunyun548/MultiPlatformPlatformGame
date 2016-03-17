using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplatformPlatformGame.Generation.Models
{
    public class PathModel
    {
        public Opening[,] Openings { get; set; }
        public int StartLine { get; set; }
        public bool OpenStart { get; set; }
        public int EndLine { get; set; }
        public bool OpenEnd { get; set; }
    }
}
