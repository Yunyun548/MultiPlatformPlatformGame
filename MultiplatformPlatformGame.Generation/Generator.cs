using MultiplatformPlatformGame.Generation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MultiplatformPlatformGame.Generation
{
    public class Generator
    {
        private readonly Random _r;
        private readonly int _blockHeight;
        private readonly int _blockWidth;

        private List<Component> _components;
        private List<Block> _userBlocks;

        private IEnumerable<Block> _generatedBlocks;
        private Dictionary<BlockOpening, Block> _allOpenings;
        private IEnumerable<Block> _closedBlocks;

        // TODO : binder les composants chargés par l'user
        public Generator(int blockHeight, int blockWidth)
        {
            this._r = new Random(DateTime.Now.Millisecond);
            this._blockHeight = blockHeight;
            this._blockWidth = blockWidth;

            this._userBlocks = new List<Block>();
            this._components = new List<Component>();
        }

        // génération de tous les blocs nécessaires et analyse de blocks
        private IEnumerable<Block> GenerateAllBlocks()
        {
            List<Block> blocks = new List<Block>();

            // TODO : utiliser des composants chargés par l'utilisateur
            var solidComponent = this._components.FirstOrDefault(c => c.Physics.Solid == true);
            var emptyComponent = this._components.FirstOrDefault(c => c.Physics.Solid == false);
            if (solidComponent == null || emptyComponent == null)
                throw new Exception("Vous devez charger au moins un composant solide et un composant vide avant de lancer la génération d'un chunk.");

            int theBase = 2;
            // boucle itérée sur toutes les combinaisons possibles
            for (int i = 0; i < Math.Pow(theBase, this._blockWidth * this._blockHeight); i++)
            {
                Component[,] components = new Component[this._blockHeight, this._blockWidth];
                for (int line = 0; line < this._blockHeight; line++)
                {
                    for (int column = 0; column < this._blockWidth; column++)
                    {
                        components[line, column] = emptyComponent;
                    }
                }

                int rest = i;
                while (rest > 0)
                {
                    int pow = 0;
                    int tmp = rest;
                    while (tmp >= theBase)
                    {
                        tmp /= theBase;
                        pow += 1;
                    }

                    int value = 1;
                    for (int j = 0; j < pow; j++)
                    {
                        value *= theBase;
                    }

                    int powCount = Math.DivRem(rest, value, out rest);

                    // retrouver l'index dans le tableau
                    int line = pow / this._blockWidth;
                    int column = pow % this._blockWidth;
                    if (powCount == 1)
                        components[line, column] = solidComponent;
                }

                blocks.Add(new Block
                {
                    BlockId = i,
                    Components = components
                });
            }
            return blocks;
        }
        private IEnumerable<BlockOpening> AnalyseBlock(Block block)
        {
            List<BlockOpening> openings = new List<BlockOpening>();

            // group all adjacent non solid components
            for (int line = 0; line < this._blockHeight; line++)
            {
                for (int column = 0; column < this._blockWidth; column++)
                {
                    if (block.Components[line, column].Physics.Solid == false)
                    {
                        BlockOpening newOpening = new BlockOpening();
                        newOpening.Points.Add(new Point(line, column));

                        // on verifie les ouvertures
                        newOpening.OpeningType = Opening.None;
                        if (line == 0)
                            newOpening.OpeningType |= Opening.Up;
                        if (column == 0)
                            newOpening.OpeningType |= Opening.Left;
                        if (line == this._blockHeight - 1)
                            newOpening.OpeningType |= Opening.Down;
                        if (column == this._blockWidth - 1)
                            newOpening.OpeningType |= Opening.Right;

                        List<BlockOpening> adjacentOpenings = new List<BlockOpening>();
                        foreach (var opening in openings)
                        {
                            foreach (var p in opening.Points)
                            {
                                if ((p.X == line && (p.Y == column + 1 || p.Y == column - 1))
                                    || (p.Y == column && (p.X == line + 1 || p.X == line - 1)))
                                {
                                    adjacentOpenings.Add(opening);
                                    break;
                                }
                            }
                        }

                        if (adjacentOpenings.Count > 0)
                        {
                            foreach (var opening in adjacentOpenings)
                            {
                                newOpening.Points.AddRange(opening.Points);
                                newOpening.OpeningType |= opening.OpeningType;
                                openings.Remove(opening);
                            }
                        }
                        openings.Add(newOpening);
                    }
                }
            }

            return openings;
        }

        /// <summary>
        /// Retourne un chunk avec les id des blocks le point de départ et le point d'arrivée
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public Chunk GenerateChunk(GeneratorOptions options, Action<string> demoMode = null)
        {
            Chunk chunk = new Chunk();

            // Génération du path
            PathModel path = this.GeneratePath(options, demoMode);

            chunk.StartBlock = path.StartLine;
            chunk.EndBlock = path.EndLine;
            IEnumerable<int> lastChunkEndComponents = null;
            if (options.LastChunk != null)
                lastChunkEndComponents = options.LastChunk.EndComponents;

            if (this._generatedBlocks == null)
            {
                var blocks = this.GenerateAllBlocks();

                foreach (var block in blocks)
                {
                    block.Openings.AddRange(this.AnalyseBlock(block));
                }

                List<Block> validBlocks = new List<Block>();
                List<Block> closedBlocks = new List<Block>();
                foreach (var block in blocks)
                {
                    // tout fonctionne uniquement avec des blocs qui ont au plus une ouverture
                    if (block.Openings.Count == 1)
                    {
                        if (block.Openings.First().OpeningType == Opening.None)
                            closedBlocks.Add(block);
                        else
                            validBlocks.Add(block);
                    }
                    else if (block.Openings.Count == 0)
                    {
                        closedBlocks.Add(block);
                    }
                }

                this._generatedBlocks = validBlocks;
                this._closedBlocks = closedBlocks;
            }

            var addModel = this.AddBlocksOnPath(path, lastChunkEndComponents, demoMode);

            chunk.Blocks = addModel.Blocks;
            if (options.LastChunk == null)
                chunk.StartComponent = addModel.StartPoint;

            if (options.OpenEnd)
                chunk.EndComponents = addModel.EndPoints;

            if (demoMode !=null)
            {
                demoMode(this.GetDisplayablePath(path.Openings));
                demoMode(this.GetDisplayableChunk(chunk));
            }

            return chunk;
        }

        // génértion du path
        private PathModel GeneratePath(GeneratorOptions options, Action<string> demoMode)
        {
            PathModel result = new PathModel();

            // Le chunk est initialisé avec toutes les valeurs par défaut (0 -> aucune ouverture)
            // x représente les lignes, y les colonnes
            Opening[,] path = new Opening[options.ChunkHeight, options.ChunkWidth];

            // Liste des coordonnées à traiter
            Queue<Point> toTreat = new Queue<Point>();

            int startLine = this._r.Next(options.ChunkHeight);
            if (options.LastChunk != null)
            {
                startLine = options.LastChunk.EndBlock;
                path[startLine, 0] |= Opening.Left;
                result.StartLine = startLine;
                result.OpenStart = true;
            }
            else
            {
                result.StartLine = startLine;
                result.OpenStart = false;
            }

            // On commence avec une position à gauche
            toTreat.Enqueue(new Point(startLine, 0));

            // Génération des pièces
            bool endReached = false;
            // Tant qu'on a des pièces à traiter ou qu'on n'a pas atteint la fin
            while (toTreat.Count > 0 || !endReached)
            {
                Point currentCoord;
                if (toTreat.Count > 0)
                {
                    currentCoord = toTreat.Dequeue();
                }
                else
                {
                    int column = this.FindRightColumn(path);
                    currentCoord = new Point(this.GetRandomLineInColumn(path, column), column);
                }

                // analyse de la faisabilité du tirage et initialisation des case adjacentes
                // 4 tests minimum
                this.CheckOpening(path, currentCoord, Opening.Up, toTreat);
                this.CheckOpening(path, currentCoord, Opening.Right, toTreat);
                this.CheckOpening(path, currentCoord, Opening.Down, toTreat);
                this.CheckOpening(path, currentCoord, Opening.Left, toTreat);

                if (currentCoord.Y == options.ChunkWidth - 1)
                    endReached = true;

                if (demoMode != null)
                    demoMode(this.GetDisplayablePath(path, currentCoord));
            }

            int endDepth = this.GetRandomLineInColumn(path, options.ChunkWidth - 1);
            result.EndLine = endDepth;
            if (options.OpenEnd)
            {
                path[endDepth, options.ChunkWidth - 1] |= Opening.Right;
                result.OpenEnd = true;
            }

            result.Openings = path;

            if (demoMode != null)
                demoMode(this.GetDisplayablePath(path));

            return result;
        }
        private void CheckOpening(Opening[,] array, Point currentCoord, Opening direction, Queue<Point> toTreat)
        {
            Opening oppositeOpening;
            bool isValidPosition;
            Point adjacentCoord;
            int creationRate;
            switch (direction)
            {
                case Opening.Up:
                    oppositeOpening = Opening.Down;
                    isValidPosition = currentCoord.X > 0;
                    adjacentCoord = new Point(currentCoord.X - 1, currentCoord.Y);
                    creationRate = 33;
                    break;
                case Opening.Right:
                    oppositeOpening = Opening.Left;
                    isValidPosition = currentCoord.Y < array.GetLength(1) - 1;
                    adjacentCoord = new Point(currentCoord.X, currentCoord.Y + 1);
                    creationRate = 50;
                    break;
                case Opening.Down:
                    oppositeOpening = Opening.Up;
                    isValidPosition = currentCoord.X < array.GetLength(0) - 1;
                    adjacentCoord = new Point(currentCoord.X + 1, currentCoord.Y);
                    creationRate = 33;
                    break;
                case Opening.Left:
                    oppositeOpening = Opening.Right;
                    isValidPosition = currentCoord.Y > 0;
                    adjacentCoord = new Point(currentCoord.X, currentCoord.Y - 1);
                    creationRate = 50;
                    break;
                default:
                    throw new Exception("Paramètre direction invalide");
            }

            // on peut créer une case dans cette direction
            if (isValidPosition && this._r.Next(100) < creationRate)
            {
                if (array[adjacentCoord.X, adjacentCoord.Y] == Opening.None)
                {
                    toTreat.Enqueue(adjacentCoord);
                }
                array[adjacentCoord.X, adjacentCoord.Y] |= oppositeOpening;
                array[currentCoord.X, currentCoord.Y] |= direction;
            }
        }
        private int FindRightColumn(Opening[,] array)
        {
            for (int column = array.GetLength(1) - 1; column >= 0; column--)
            {
                for (int line = array.GetLength(0) - 1; line >= 0; line--)
                {
                    if (array[line, column] != Opening.None)
                    {
                        return column;
                    }
                }
            }
            return 0;
        }
        private int GetRandomLineInColumn(Opening[,] array, int column)
        {
            List<int> validLines = new List<int>();
            for (int line = 0; line < array.GetLength(0); line++)
            {
                if (array[line, column] != Opening.None)
                {
                    validLines.Add(line);
                }
            }
            if (validLines.Any())
                return validLines.ElementAt(this._r.Next(validLines.Count));
            else
                return 0;
        }

        // ajout des blocks sur le path
        private AddBlock AddBlocksOnPath(PathModel path, IEnumerable<int> lastChunkEndCoponents, Action<string> demoMode)
        {
            AddBlock result = new AddBlock();

            int lineCount = path.Openings.GetLength(0);
            int columnCount = path.Openings.GetLength(1);

            // pour le retour avec les vrais blocs
            result.Blocks = new Block[lineCount, columnCount];

            // pour le calcul
            BlockOpening[,] matrix = new BlockOpening[lineCount, columnCount];

            if (this._allOpenings == null)
            {
                this._allOpenings = new Dictionary<BlockOpening, Block>();
                foreach (var block in this._generatedBlocks)
                {
                    foreach (var opening in block.Openings)
                    {
                        this._allOpenings.Add(opening, block);
                    }
                }
            }
            IEnumerable<BlockOpening> openings = this._allOpenings.Keys;

            // on remplit de haut en bas et de gauche à droite, on peut regarder seulement en haut et à gauche
            // pour vérifier les contraintes
            for (int line = 0; line < lineCount; line++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    Opening pathOpening = path.Openings[line, column];
                    if (pathOpening == Opening.None)
                    {
                        var selectedBlock = this._closedBlocks.ElementAt(this._r.Next(this._closedBlocks.Count()));
                        result.Blocks[line, column] = selectedBlock;
                        matrix[line, column] = new BlockOpening
                        {
                            OpeningType = Opening.None
                        };
                    }
                    else
                    {
                        IEnumerable<int> validY = null;
                        if ((pathOpening & Opening.Up) == Opening.Up)
                        {
                            // on est sur d'avoir un bloc en haut
                            var opening = matrix[line - 1, column];
                            List<int> validPoints = new List<int>();
                            foreach (var p in opening.Points)
                            {
                                if (p.X == this._blockHeight - 1)
                                    validPoints.Add(p.Y);
                            }
                            validY = validPoints;
                        }

                        IEnumerable<int> validX = null;
                        if ((pathOpening & Opening.Left) == Opening.Left)
                        {
                            if (column == 0)
                            {
                                // on est sur d'etre au début
                                validX = lastChunkEndCoponents;
                            }
                            else
                            {
                                var opening = matrix[line, column - 1];
                                List<int> validPoints = new List<int>();
                                foreach (var p in opening.Points)
                                {
                                    if (p.Y == this._blockWidth - 1)
                                        validPoints.Add(p.X);
                                }
                                validX = validPoints;
                            }
                        }

                        Opening constraint = Opening.None;
                        if (line == 0)
                        {
                            constraint |= Opening.Up;
                        }

                        if (line == lineCount - 1)
                        {
                            constraint |= Opening.Down;
                        }

                        if (column == 0 && !(path.OpenStart && path.StartLine == line))
                        {
                            constraint |= Opening.Left;
                        }

                        if (column == columnCount - 1 && !(path.OpenEnd && path.EndLine == line))
                        {
                            constraint |= Opening.Right;
                        }

                        var matchingOpenings = openings.Where(o => (o.OpeningType & pathOpening) == pathOpening
                            && (o.OpeningType & ~constraint) == o.OpeningType
                            && (validX == null || o.Points.Any(p => p.Y == 0 && validX.Any(x => x == p.X)))
                            && (validY == null || o.Points.Any(p => p.X == 0 && validY.Any(y => y == p.Y)))
                            );

                        // TODO : gérer avec une meilleure gestion des contraintes
                        // regarder le path des éléments en bas et à droite
                        if (!matchingOpenings.Any())
                            matchingOpenings = openings.Where(o => (o.OpeningType & pathOpening) == pathOpening
                                && (validX == null || o.Points.Any(p => p.Y == 0 && validX.Any(x => x == p.X)))
                                && (validY == null || o.Points.Any(p => p.X == 0 && validY.Any(y => y == p.Y)))
                            );

                        var randomOpening = matchingOpenings.ElementAt(this._r.Next(matchingOpenings.Count()));

                        if ((pathOpening & Opening.Right) == Opening.Right)
                        {
                            if (column == columnCount - 1)
                            {
                                result.EndPoints = randomOpening.Points.Where(p => p.Y == this._blockWidth - 1).Select(p => p.X);
                            }
                        }

                        if (!path.OpenStart && column == 0 && line == path.StartLine)
                        {
                            result.StartPoint = randomOpening.Points.ElementAt(this._r.Next(randomOpening.Points.Count));
                        }

                        matrix[line, column] = randomOpening;
                        var selectedBlock = this._allOpenings[randomOpening];
                        result.Blocks[line, column] = selectedBlock;
                    }
                    if (demoMode != null)
                        demoMode(this.GetDisplayableAddBlock(result));
                }
            }

            return result;
        }

        // affichage
        public string GetDisplayableChunk(Chunk chunk)
        {
            return this.GetDisplayableEndLevel(chunk.Blocks, chunk.StartBlock, chunk.StartComponent);
        }

        private string GetDisplayableAddBlock(AddBlock addBlock)
        {
            var blocks = addBlock.Blocks;
            return this.GetDisplayableEndLevel(blocks);
        }

        private string GetDisplayableBlock(Block block)
        {
            StringBuilder sb = new StringBuilder();
            for (int line = 0; line < this._blockHeight; line++)
            {
                for (int column = 0; column < this._blockWidth; column++)
                {
                    sb.Append(block.Components[line, column].Physics.Solid ? "X" : " ");
                }
                sb.AppendLine();
            }

            foreach (var opening in block.Openings)
            {
                sb.AppendLine(opening.OpeningType.ToString());
                foreach (var point in opening.Points)
                {
                    sb.Append(point.ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private string GetDisplayablePath(Opening[,] path, Point? current = null)
        {
            StringBuilder sb = new StringBuilder();
            for (int line = 0; line < path.GetLength(0); line++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int column = 0; column < path.GetLength(1); column++)
                    {
                        Opening currentOpening = path[line, column];
                        if (currentOpening == Opening.None)
                        {
                            sb.Append("XXX");
                        }
                        else if (i == 0)
                        {
                            if ((currentOpening & Opening.Up) == Opening.Up)
                            {
                                sb.Append("X X");
                            }
                            else {
                                sb.Append("XXX");
                            }
                        }
                        else if (i == 1)
                        {
                            if ((currentOpening & Opening.Left) == Opening.Left)
                            {
                                sb.Append(" ");
                            }
                            else
                            {
                                sb.Append("X");
                            }
                            if (current.HasValue && current.Value.X == line && current.Value.Y == column)
                            {
                                sb.Append(".");
                            }
                            else
                            {
                                sb.Append(" ");
                            }
                            if ((currentOpening & Opening.Right) == Opening.Right)
                            {
                                sb.Append(" ");
                            }
                            else
                            {
                                sb.Append("X");
                            }
                        }
                        else
                        {
                            if ((currentOpening & Opening.Down) == Opening.Down)
                            {
                                sb.Append("X X");
                            }
                            else
                            {
                                sb.Append("XXX");
                            }
                        }

                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        private string GetDisplayableEndLevel(Block[,] array, int? startLine = null, Point? start = null)
        {
            StringBuilder sb = new StringBuilder();
            int lineCount = array.GetLength(0);
            int columnCount = array.GetLength(1);

            for (int line = 0; line < lineCount; line++)
            {
                for (int height = 0; height < this._blockHeight; height++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        for (int width = 0; width < this._blockWidth; width++)
                        {
                            var block = array[line, column];
                            if(block == null)
                            {
                                sb.Append("X");
                            }
                            else
                            {
                                bool solid = block.Components[height, width].Physics.Solid;
                                if (!solid)
                                {
                                    if (startLine.HasValue && startLine.Value == line
                                        && column == 0
                                        && start.HasValue && start.Value.X == height && start.Value.Y == width)
                                        sb.Append("o");
                                    else
                                        sb.Append(" ");
                                }
                                else
                                    sb.Append("X");
                            }                            
                        }
                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        // ajout des objets au générateur
        public void AddComponent(String filePath)
        {
            string json = System.IO.File.ReadAllText(filePath);
            Component c = this.Deserialize<Component>(json);
            this._components.Add(c);
        }
        public void AddBlock(String filePath)
        {
            string json = System.IO.File.ReadAllText(filePath);
            Block b = this.Deserialize<Block>(json);
            this._userBlocks.Add(b);
        }

        // sérialisation des objets
        private string Serialize<T>(T parameter)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            string json = null;
            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                ser.WriteObject(ms, parameter);
                ms.Position = 0;
                json = sr.ReadToEnd();
            }
            return json;
        }
        private T Deserialize<T>(string json)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            T deserialized;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                deserialized = (T)ser.ReadObject(ms);
            }
            return deserialized;
        }
    }

    [Flags]
    public enum Opening : byte
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
        All = 15
    }
}

