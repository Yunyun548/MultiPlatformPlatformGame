using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace MultiplatformPlatformGame.Generation.Models
{
    [DataContract]
    public class Block
    {
        [DataMember(Name = "id")]
        public int BlockId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "matrix")]
        public int[] ComponentIds { get; set; }

        // valeurs pour le placement
        public Component[,] Components { get; set; }
        public List<BlockOpening> Openings { get; set; }
        public Block()
        {
            Openings = new List<BlockOpening>();
        }

        public void BindComponents(int blockWidth, int blockHeight, IEnumerable<Component> components)
        {
            this.Components = new Component[blockHeight, blockWidth];
            for (int line = 0; line < blockHeight; line++)
            {
                for (int column = 0; column < blockWidth; column++)
                {
                    int index = column * blockWidth + line;
                    Component component = components.SingleOrDefault(c => c.ComponentId == this.ComponentIds[index]);

                    if (component == null)
                        throw new Exception(string.Format("Unable to find the component {0}", this.ComponentIds[index]));

                    this.Components[line, column] = component;
                }
            }
        }
    }

    public class BlockOpening
    {
        public Opening OpeningType { get; set; }
        public List<Point> Points { get; set; }
        public BlockOpening()
        {
            Points = new List<Point>();
        }
    }
}