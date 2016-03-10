using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiPlatfomPlatformGame.WebEditorASP.Models
{
    public class Block
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Component[] Components { get; set; }

        public Block()
        {
                
        }
    }
}