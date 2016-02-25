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

        public Generator()
        {
            this._r = new Random(DateTime.Now.Millisecond);
        }

        private List<Component> components;
        private List<Block> blocks;

        public void AddComponent(String filePath)
        {
            Component c = JsonConvert.DeserializeObject<Component>(System.IO.File.ReadAllText(filePath));
            components.Add(c);
        }

        public void AddBlock(String filePath)
        {
            Block b = JsonConvert.DeserializeObject<Block>(System.IO.File.ReadAllText(filePath));
            blocks.Add(b);
        }

        public void AnalyseBlocks()
        {
            foreach (Block b in blocks)
            {
                b.BindComponents(components);
                AnalyseBlock(b);
            }
        }

        protected void AnalyseBlock(Block b)
        {
        }

        public void GenerateChunck()
        {
            List<Room> rooms = new List<Room>();

            // Point de départ
            Position start = new Position(0, 0);
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
                    int topRight = rooms.Max(ro => ro.Position.Column);
                    currentRoom = rooms.First(ro => ro.Position.Column == topRight);
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
                Position newPosition = new Position(currentRoom.Position.Line, currentRoom.Position.Column);
                switch (direction)
                {
                    case Opening.Up:
                        newPosition.Line -= 1;
                        oppositeDirection = Opening.Down;
                        break;
                    case Opening.Right:
                        newPosition.Column += 1;
                        oppositeDirection = Opening.Left;
                        break;
                    case Opening.Down:
                        newPosition.Line += 1;
                        oppositeDirection = Opening.Up;
                        break;
                    case Opening.Left:
                        newPosition.Column -= 1;
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
            public Position Position { get; set; }
        }

        class Position
        {
            public int Line { get; set; }
            public int Column { get; set; }
            public Position(int line, int column)
            {
                Line = line;
                Column = column;
            }
        }
    }
}

