using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Test.DynamicContentManagement;
using System.Diagnostics;
using System.Collections.Generic;
using Test.Helper_classes;
using System.Linq;
using Test.InputMangement;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Test
{
    // ##################################################################################################
    // ##########################################   NODE   ##############################################
    // ##################################################################################################

    internal class WFCNode
    { 

        public string Name { get; private set; }

        public short[] Sockets { get; private set; }

        public WFCNode()
        {
            Name = "uncollapsed";
            Sockets = new short[8];
        }

        public WFCNode(int left, int topLeft, int top, int topRight, int right, int bottomRight, int bottom, int bottomLeft, string name)
        {
            Sockets = new short[8];
            Sockets[0] = (short)left;
            Sockets[1] = (short)topLeft;
            Sockets[2] = (short)topRight;
            Sockets[3] = (short)top;
            Sockets[4] = (short)right;
            Sockets[5] = (short)bottom;
            Sockets[6] = (short)bottomLeft;
            Sockets[7] = (short)bottomRight;
            Name = name;
        }

        /// <summary>
        /// Change a Socket's value.
        /// </summary>
        /// <param name="value"> The value that will be set to</param>
        /// <param name="direction"> The Socket that will be changed. 0 is left, 1 is top left, and continues clockwise.</param>
        public void Pack(int value, int direction)
        {
            if (value < -1) { throw new ArgumentOutOfRangeException("value", "Value must be >= -1. Use -1 for invalid."); }
            if (direction < 0 || direction > 7) { throw new ArgumentOutOfRangeException("direction", "Direction must be between 0 and 7."); }
            Sockets[direction] = (short)value;
        }

        // Use this to directly get a Socket's value.
        public int this[int key]
        {
            get => Sockets[key];
            set => throw new InvalidOperationException("Use .Pack(int value, int direction) to assign a value to a Socket.");
        }

        // This method calculates how many Sockets are filled.
        public int GetEntropy()
        {
            int entropy = 0;

            for (int i=0; i<8; i++)
            {
                if (Sockets[i] <= 0) { entropy++; }
            }

            return entropy;
        }
        
        // This method returns the most common tile in this nodes' Sockets.
        public int GetTileType()
        {
            int[] typeCount = new int[8];
            for (int i=0; i<8; i++)
            {
                if (Sockets[i] < 0) { continue; }
                typeCount[Sockets[i]] += 1;
            }
            int best = 0;
            int bestScore = 0;
            for (int i=0; i<8; i++)
            {
                if (typeCount[i] >= bestScore)
                {
                    best = i;
                    bestScore = typeCount[i];
                }
            }
            return best;
        }

        
        // Print the individual Socket value to the Debug Console.
        public void Print()
        {
            Debug.WriteLine(Sockets[0].ToString());
        }


        // Overload of the Equals operator to properly compare and not use hashing.
        public static bool operator ==(WFCNode me, WFCNode other)
        {
            if (ReferenceEquals(other, null)) { return false; }
            for (int i = 0; i < 8; i++)
            {
                if (me[i] != other[i]) { return false; }
            }
            return true;
        }
        public static bool operator !=(WFCNode me, WFCNode other)
        {
            if (ReferenceEquals(other, null)) { return true; }
            if (me == other) { return false; } return true;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            throw new NotImplementedException();
        }
        public override int GetHashCode()
        {
            long hc = Enumerable.Range(0, 8)
                .Select(i => (long)Sockets[i] << (i * 16))
                .Aggregate(0L, (acc, x) => (acc + x) % Int32.MaxValue);

            return (int)hc;
        }
    }

    // ##################################################################################################
    // ####################################   THE ALGORITHM   ###########################################
    // ##################################################################################################

    internal class WaveFunctionCollapse
    {
        // The grid.
        readonly private Grid _grid;

        // This tells the class if the grid Square is to be collapsed.
        readonly private Func<Point, bool> _isGridSquareCollapsable;

        readonly private Func<Point, WFCNode> _indexer;

        readonly private Func<Point, WFCNode, bool, bool> _setter;

        // The function to get the size of the grid.
        readonly private Func<Point> _getSize;

        private Dictionary<int, List<Point>> _entropy;

        private int _lowestEntropy;

        private Point _startPoint;

        readonly private Random _random;

        readonly WFCNodes _nodes;



        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <param name="grid">A newly initialized grid, defined however you want.</param>
        /// <param name="isGridSquareCollapsable">A function defining if the grid square at an index is collapsable, or if it should be seen as a void.</param>
        /// <param name="getSize">This gets the size of the grid. It's important this is a function, not an int, that way if the size changes it gets updated here.</param>
        /// <param name="socketsPerGridSquare">The sockets each Grid Square has, either 4 (up, down, right, left) or 8 (diagonals too).</param>
        public WaveFunctionCollapse(Grid grid, Func<Point, bool> isGridSquareCollapsable, Func<Point> getSize, Func<Point, WFCNode> indexer, Func<Point, WFCNode, bool, bool> setter, WFCNodes nodes)
        {
            _grid = grid;
            _isGridSquareCollapsable = isGridSquareCollapsable;
            _getSize = getSize;
            _indexer = indexer;
            _setter = setter;
            _nodes = nodes;

            _random = new ();
            _entropy  = new Dictionary<int, List<Point>>();
            _lowestEntropy = 8;

            Initialize();            
        }

        // Initializes the WFC and starts the collapse.
        private void Initialize()
        {
            WFCNode startNode;

            // Assure that all Grid points are collapsable.
            foreach (Point point in Looper())
            {
                WFCNode node = new ();
                _setter(point, node, true);
                _grid.SetPathable(point, true);
            }

            // Set the start point to the bottom right corner.
            _startPoint = new Point(_getSize().X - 1, _getSize().Y - 1);

            // Initialize the entropy list.
            for (int i = 0; i < 9; i++)
            {
                _entropy.Add(i, new List<Point>());
            }

            // Get the start node and pack it with a starting Tile.
            _setter(_startPoint, _nodes[3], false);
            
            _entropy[8].Add(_startPoint);

            while (Collapse()) { }
        }

        // This collapses the grid until every grid square is collapsed.
        public bool Collapse()
        {
            int i = _lowestEntropy;
            while (_entropy[i].Count == 0)
            {
                i++;
                if (i > 8) { return false; }
            }
            _lowestEntropy = i;

            Point nodePoint = _entropy[_lowestEntropy].First();
            if (_indexer(nodePoint).GetEntropy() == 0) { _entropy[_lowestEntropy].Remove(nodePoint); }

            _entropy[_lowestEntropy].Remove(nodePoint);

            WFCNode node = GetRandomOption(nodePoint);

            _setter(nodePoint, node, false);
            UpdateNeighbors(nodePoint);

            return true;
        }

        private void UpdateNeighbors(Point point)
        {
            WFCNode node = _indexer(point);

            foreach (Point3 neighbor in GetNeighbors(point))
            {
                Point neighborPos = new(neighbor.X, neighbor.Y);

                if (_indexer(neighborPos) != null && _isGridSquareCollapsable(neighborPos))
                {
                    _indexer(neighborPos).Pack(node[neighbor.Z], MirrorSocket(neighbor.Z));
                    _indexer(neighborPos).Pack(node[neighbor.Z + 1], MirrorSocket(neighbor.Z + 1));
                    AddToEntropyList(neighborPos, _indexer(neighborPos).GetEntropy());
                }
            }

        }

        public void AddToEntropyList(Point point, int entropy)
        {
            _entropy[entropy].Add(point); 
            
            if (entropy < _lowestEntropy) { _lowestEntropy = entropy; }
        }

        public bool UpdateEntropy(Point nodePoint)
        {
            _entropy[_lowestEntropy].Remove(nodePoint);
            return true;
        }

        private WFCNode GetRandomOption(Point point)
        {
            WFCNode self = _indexer(point);
            float bestScore = 0f;
            WFCNode bestNode = _nodes.Invalid;

            foreach (WFCNode other in _nodes.Nodes)
            {
                float score = -1f;

                for (int i = 0; i < 8; i++)
                {
                    if (self[i] == other[i]) { score++; continue; }
                    if (self[i] != 0) { score = -10f; }
                }

                // Make grass more than dirt
                if (other.GetTileType() == 1 && _random.Next(0,4) > 0) { score += .6f; }
                // Standard textures more than detail
                if ((other.Name == "tile003" || other.Name == "tile008") && _random.Next(0, 6) > 0) { score += .6f; }
                // Less dirt details
                if ((other.Name == "tile011" || other.Name == "tile012" || other.Name == "tile013") && _random.Next(0, 3) > 0) { score -= .1f; }
                // Less grass stones
                if ((other.Name == "tile018" || other.Name == "tile019" || other.Name == "tile020") && _random.Next(0, 6) > 0) { score -= .8f; }
                // More grass in the grass
                if ((other.Name == "tile004" || other.Name == "tile005" || other.Name == "tile006") && _random.Next(0, 6) > 4) { score += .5f; }

                // More leafy trees
                if ((other.Name == "tile0225" || other.Name == "tile0221" || other.Name == "tile0222") && _random.Next(0, 7) > 5) { score += .5f; }
                // Less weird branches
                if ((other.Name == "tile0224" || other.Name == "tile0223" || other.Name == "tile0226" || other.Name == "tile0227") && _random.Next(0, 7) > 1) { score -= .4f; }

                // trees that end
                if ((other.Name == "tile0229")) { score += 1.5f; }



                // If it's a 'emergency filler' tile
                if (other.Name == "tile0204" || other.Name == "tile0205" || other.Name == "tile0206" || other.Name == "tile0207" || other.Name == "tile0208" || other.Name == "tile0209") { score -= 0.9f; }

                if (_random.Next(0, 2) == 0)
                {
                    if (score > bestScore) { bestScore = (int)score; bestNode = other; }
                }
                else
                {

                }
                {
                    if (score >= bestScore) { bestScore = (int)score; bestNode = other; }
                }
            }
            
            
            return bestNode;
        }
        public IEnumerable<Point3> GetNeighbors(Point point)
        {
            yield return new Point3(point.X - 1, point.Y, 0);
            yield return new Point3(point.X, point.Y - 1, 2);
            yield return new Point3(point.X + 1, point.Y, 4);
            yield return new Point3(point.X, point.Y + 1, 6);
        }

        public static int MirrorSocket(int socket)
        {
            return socket % 2 == 0 ? (socket + 5) % 8 : (socket + 3) % 8;
        }

        /// <summary>
        /// This creates a short-hand way of accessing each point in the grid.
        /// </summary>
        /// <returns>An IEnumerable that yields each Point in the grid one by one.</returns>
        public IEnumerable<Point> Looper()
        {
            Point point = _getSize();

            for (int y = 0; y < point.Y; y++)
            {
                for (int x = 0; x < point.X; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Point point in Looper())
            {
                spriteBatch.Draw(ContentDictionary.TextureDict[_indexer(point).Name + ".png"], new Rectangle(point.X * 20, point.Y * 20, 20, 20), Color.White);
            }
        }

        public IEnumerable<string> TextureExporter()
        {
            foreach (WFCNode node in _nodes.Nodes)
            {
                yield return node.Name;
            }
        }

    }
}
