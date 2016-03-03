using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace MultiplatformPlatformGame.Generation
{
	public class Generator
	{
        // Constantes pour la configuration (déplacer dans configuration)
        private const int ChunkSize = 100;
        private const int JoinChance = 30;
        private const int VerticalChance = 25;
        private const int HorizontalChance = 75;

        private readonly Random _r;

		private List<Component> components;
		private List<Block> blocks;

		public Generator ()
		{
            this._r = new Random(DateTime.Now.Millisecond);
			components = new List<Component> ();
			blocks = new List<Block> ();
		}

		public void AddComponent(String filePath)
		{
			Component c = JsonConvert.DeserializeObject<Component> (System.IO.File.ReadAllText(filePath));
			components.Add (c);
		}

		public void AddBlock(String filePath)
		{
			Block b = JsonConvert.DeserializeObject<Block> (System.IO.File.ReadAllText(filePath));
			blocks.Add (b);
		}

		public List<Block> GetBlocks()
		{
			return blocks;
		}

		public void AnalyseBlocks()
		{
			foreach (Block b in blocks) {
				b.BindComponents (components);
				AnalyseBlock (b);
			}
		}

		protected void AnalyseBlock(Block b)
		{
			foreach (BlockComponent c in BorderNonSolidComponents(b)) {
				
			}
		}

		protected IEnumerable<BlockComponent> BorderComponents(Block b)
		{
			for (int i = 0; i < b.BlockSize; i++) {
				if (i == 0 || i == b.BlockSize) {
					foreach (BlockComponent c in b.ComponentMatrix[i]) {
						yield return c;
					}
				} else {
					yield return b.ComponentMatrix [i] [0];
					yield return b.ComponentMatrix [i] [b.BlockSize - 1];
				}
			}
		}

		protected IEnumerable<BlockComponent> BorderNonSolidComponents(Block b)
		{
			foreach (BlockComponent c in BorderComponents(b)) {
				if (!c.Component.Solid) {
					yield return c;
				}
			}
		}

        public void GenerateChunck()
        {
            List<Room> rooms = new List<Room>();

            // Point de départ
            Point start = new Point(0, 0);
            Room startRoom = new Room
            {
                Position = start,
                OpenWalls = Opening.None
            };
            rooms.Add(startRoom);


            // Génération des autres pièces
            int cmpt = 0;
            while (rooms.Count < Generator.ChunkSize)
            {
                Room currentRoom;
                if (cmpt >= rooms.Count)
                {
                    int topRight = rooms.Max(ro => ro.Position.X);
                    currentRoom = rooms.First(ro => ro.Position.X == topRight);
                    currentRoom.OpenWalls = Opening.All;
                }
                else
                {
                    currentRoom = rooms.ElementAt(cmpt);
                    currentRoom.OpenWalls |=
                        (this._r.Next(100) < VerticalChance ? Opening.Up : Opening.None) |
                        (this._r.Next(100) < HorizontalChance ? Opening.Right : Opening.None) |
                        (this._r.Next(100) < VerticalChance ? Opening.Down : Opening.None) |
                        (this._r.Next(100) < HorizontalChance ? Opening.Left : Opening.None);
                    cmpt += 1;
                }

                this.CreateAdjacentRoom(currentRoom, rooms, Opening.Up);
                this.CreateAdjacentRoom(currentRoom, rooms, Opening.Right);
                this.CreateAdjacentRoom(currentRoom, rooms, Opening.Down);
                this.CreateAdjacentRoom(currentRoom, rooms, Opening.Left);
            }

            // TODO : retourner un modèle exploitable
        }

        private void CreateAdjacentRoom(Room currentRoom, List<Room> rooms, Opening direction)
        {
            if ((currentRoom.OpenWalls & direction) == direction)
            {
                Opening oppositeDirection;
                Point newPosition = new Point(currentRoom.Position.X, currentRoom.Position.Y);
                switch (direction)
                {
                    case Opening.Up:
                        newPosition.Y -= 1;
                        oppositeDirection = Opening.Down;
                        break;
                    case Opening.Right:
                        newPosition.X += 1;
                        oppositeDirection = Opening.Left;
                        break;
                    case Opening.Down:
                        newPosition.Y += 1;
                        oppositeDirection = Opening.Up;
                        break;
                    case Opening.Left:
                        newPosition.X -= 1;
                        oppositeDirection = Opening.Right;
                        break;
                    default:
                        throw new Exception(string.Format("Attention au paramètre direction. {0}", direction));
                }

                Room oldRoom = rooms.FirstOrDefault(ro => ro.Position == newPosition);
                if (oldRoom != null)
                {
                    if (this._r.Next(100) < JoinChance)
                    {
                        oldRoom.OpenWalls |= oppositeDirection;
                    }
                    else if ((oldRoom.OpenWalls & oppositeDirection) != oppositeDirection)
                    {
                        currentRoom.OpenWalls &= (Opening.All & ~direction);
                    }

                }
                else
                {
                    Room newRoom = new Room()
                    {
                        Position = newPosition,
                        OpenWalls = oppositeDirection
                    };
                    rooms.Add(newRoom);
                }
            }
        }

        [Flags]
        enum Opening
        {
            Up = 0x0001,
            Right = 0x0010,
            Down = 0x0100,
            Left = 0x1000,
            All = 0x1111,
            None = 0x0000
        }

        class Room
        {
            public Opening OpenWalls { get; set; }
            public Point Position { get; set; }
        }
    }
}

