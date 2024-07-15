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

namespace Test
{
    internal class WFCNode
    {   /*
        // The int that contains the socket information.
        private UInt32 _socketData;

        // This defines how the WFCNode is packed.
        private bool _WFCNodeType;

        public WFCNode(bool WFCNodeType)
        {
            _WFCNodeType = WFCNodeType;
            _socketData = 0b_0000_0000_0000_0000_0000_0000_0000_0000;
        }

        public void Pack(int up, int down, int right, int left)
        {
            if (_WFCNodeType) { throw new Exception("Missing diagonals! Use the other Pack() method."); }

            _socketData = (UInt32)left + ((UInt32)right << 4) + ((UInt32)down << 8) + ((UInt32)up << 12);
        }

        public void Pack(int topRight, int topLeft, int bottomRight, int bottomLeft, int up, int down, int right, int left)
        {
            if (!_WFCNodeType) { throw new Exception("socketPerGridSquare == 4 doesn't support diagonals! Use the other Pack() method."); }

            _socketData = (UInt32)left + ((UInt32)right << 4) + ((UInt32)down << 8) + ((UInt32)up << 12) + ((UInt32)bottomLeft << 16) + ((UInt32)bottomRight << 20) + ((UInt32)topLeft << 24) + ((UInt32)topRight << 28);
        }

        public void Pack(int value, int direction)
        {
            if (!_WFCNodeType)
            {
                if (direction == 0) { _socketData += (UInt32)value; }
                if (direction == 2) { _socketData += (UInt32)value >> 12; }
                if (direction == 4) { _socketData += (UInt32)value << 4; }
                if (direction == 6) { _socketData += (UInt32)value << 8; }
                else { throw new Exception($"Invalid direction {direction}!"); }
            }
            else
            {
                if (direction == 0) { _socketData += (UInt32)value; }
                if (direction == 1) { _socketData += (UInt32)value << 24; }
                if (direction == 2) { _socketData += (UInt32)value << 12; }
                if (direction == 3) { _socketData += (UInt32)value; }
                if (direction == 4) { _socketData += (UInt32)value << 4; }
                if (direction == 5) { _socketData += (UInt32)value; }
                if (direction == 6) { _socketData += (UInt32)value << 8; }
                if (direction == 7) { _socketData += (UInt32)value; }
                else { throw new Exception($"Invalid direction {direction}!"); }
            }
        }

        public int GetEntropy()
        {
            if (!_WFCNodeType)
            {
                return Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_0000_0000_0000_1111)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_0000_0000_1111_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_0000_1111_0000_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_1111_0000_0000_0000));
            }
            else
            {
                return Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_0000_0000_0000_1111)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_0000_0000_1111_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_0000_1111_0000_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_0000_1111_0000_0000_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_0000_1111_0000_0000_0000_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_0000_1111_0000_0000_0000_0000_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_0000_1111_0000_0000_0000_0000_0000_0000)) +
                       Convert.ToInt32(!Convert.ToBoolean(_socketData & 0b_1111_0000_0000_0000_0000_0000_0000_0000));
            }
        
        public int Left()
        {
            return (int)(_socketData & 0b_0000_0000_0000_0000_0000_0000_0000_1111);
        }
        public int Right()
        {
            return (int)(_socketData & 0b_0000_0000_0000_0000_0000_0000_1111_0000);
        }
        public int Down()
        {
            return (int)(_socketData & 0b_0000_0000_0000_0000_0000_1111_0000_0000);
        }
        public int Up()
        {
            return (int)(_socketData & 0b_0000_0000_0000_0000_1111_0000_0000_0000);
        }
        public int BottomLeft()
        {
            if (!_WFCNodeType) { throw new Exception("socketPerGridSquare == 4 doesn't support diagonals! Trying to access them anyways would return 0!");  }
            return (int)(_socketData & 0b_0000_0000_0000_1111_0000_0000_0000_0000);
        }
        public int BottomRight()
        {
            if (!_WFCNodeType) { throw new Exception("socketPerGridSquare == 4 doesn't support diagonals! Trying to access them anyways would return 0!"); }
            return (int)(_socketData & 0b_0000_0000_1111_0000_0000_0000_0000_0000);
        }
        public int TopLeft()
        {
            if (!_WFCNodeType) { throw new Exception("socketPerGridSquare == 4 doesn't support diagonals! Trying to access them anyways would return 0!"); }
            return (int)(_socketData & 0b_0000_1111_0000_0000_0000_0000_0000_0000);
        }
        public int TopRight()
        {
            if (!_WFCNodeType) { throw new Exception("socketPerGridSquare == 4 doesn't support diagonals! Trying to access them anyways would return 0!"); }
            return (int)(_socketData & 0b_1111_0000_0000_0000_0000_0000_0000_0000);
        }
        */
        private readonly bool _WFCNodeType;

        public string Name { get; private set; }

        public int Left { get; private set; }
        public int TopLeft { get; private set; }
        public int Top { get; private set; }
        public int TopRight { get; private set; }
        public int Right { get; private set; }
        public int BottomRight { get; private set; }
        public int Bottom { get; private set; }
        public int BottomLeft { get; private set; }

        public WFCNode(bool WFCNodeType)
        {
            _WFCNodeType = WFCNodeType;
            Name = "uncollapsed";
        }

        public WFCNode(int left, int topLeft, int top, int topRight, int right, int bottomRight, int bottom, int bottomLeft, string name)
        {
            _WFCNodeType = true;

            Left = left;
            TopLeft = topLeft;
            TopRight = topRight;
            Top = top;  
            Right = right;
            Bottom = bottom;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
            Name = name;
        }


        public void Pack(int value, int direction)
        {
            if (direction == 0 && Left == 0) { Left = value; }
            else if (direction == 1 && TopLeft == 0) { TopLeft = value; }
            else if (direction == 2 && Top == 0) { Top = value; }
            else if (direction == 3 && TopRight == 0) { TopRight = value; }
            else if (direction == 4 && Right == 0) { Right = value; }
            else if (direction == 5 && BottomRight == 0) { BottomRight = value; }
            else if (direction == 6 && Bottom == 0) { Bottom = value; }
            else if (direction == 7 && BottomLeft == 0) { BottomLeft = value; }
            //else { throw new Exception("Error"); }
        }

        public int GetSocket(int dir)
        {
            if (dir == 0) { return Left; }
            if (dir == 1) { return TopLeft; }
            if (dir == 2) { return Top; }
            if (dir == 3) { return TopRight; }
            if (dir == 4) { return Right; }
            if (dir == 5) { return BottomRight; }
            if (dir == 6) { return Bottom; }
            if (dir == 7) { return BottomLeft; }
            else { return -1; }
        }

        public int GetEntropy()
        {
            int entropy = 0;

            for (int i=0; i<8; i++)
            {
                if (GetSocket(i) <= 0) { entropy++; }
            }

            return entropy;
        }

        public void Print()
        {
            Debug.WriteLine(Left.ToString() + " " + TopLeft.ToString() + " " 
                + Top.ToString() + " " + TopRight.ToString() + " " 
                + Right.ToString() + " " + BottomRight.ToString() + " " 
                + Bottom.ToString() + " " + BottomLeft.ToString());
        }

        public static bool operator ==(WFCNode me, WFCNode other)
        {
            if (me.Left == other.Left && me.TopLeft == other.TopLeft && me.Top == other.Top && me.TopRight == other.TopRight && me.Right == other.Right && me.Bottom == other.Bottom && me.BottomLeft == other.BottomLeft && me.BottomRight == other.BottomRight) {  return true; }
            return false;
        }
        public static bool operator !=(WFCNode me, WFCNode other)
        {
            //if (me.Left == other.Left && me.TopLeft == other.TopLeft && me.Top == other.Top && me.TopRight == other.TopRight && me.Right == other.Right && me.Bottom == other.Bottom && me.BottomLeft == other.BottomLeft && me.BottomRight == other.BottomRight) { return false; }
            return true;
        }

    }

    internal class WFCNodes
    {
        internal WFCNodes() {}
        // DynamicContentManager.Instance.Load<Texture2D>("uncollapsed")

        public static List<WFCNode> nodes = new()
        {
            new WFCNode(0, 0, 0, 0, 0, 0, 0, 0, "uncollapsed"),
            new WFCNode(0, 0, 0, 0, 0, 0, 0, 0, "void"),
            new WFCNode(-1, -1, -1, -1, -1, -1, -1, -1, "invalid"),
            new WFCNode(1, 1, 1, 1, 2, 2, 2, 2, "tile000"),
            new WFCNode(2, 2, 1, 1, 2, 2, 2, 2, "tile001"),
            new WFCNode(2, 2, 1, 1, 1, 1, 2, 2, "tile002"), 
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile003"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile004"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile005"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile006"), 
            new WFCNode(1, 1, 2, 2, 2, 2, 2, 2, "tile007"),
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile008"), 
            new WFCNode(2, 2, 2, 2, 1, 1, 2, 2, "tile009"), 
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile010"), 
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile011"),
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile012"),
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile013"),
            new WFCNode(1, 1, 2, 2, 2, 2, 1, 1, "tile014"), 
            new WFCNode(2, 2, 2, 2, 2, 2, 1, 1, "tile015"), 
            new WFCNode(2, 2, 2, 2, 1, 1, 1, 1, "tile016"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile017"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile018"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile019"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile020"),
        };

        public static WFCNode Uncollapsed = nodes[0];
        public static WFCNode Invalid = nodes[2];

    }
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

        // This defines how the WFCNode is packed. False is 4*4 bits, True is 4*8 bits.
        readonly private bool _WFCNodeType;

        private List<Point> _lowestEntropy;

        private int _lowestEntropyValue;

        private Point _startPoint;



        readonly private Random _random;



        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <param name="grid">A newly initialized grid, defined however you want.</param>
        /// <param name="isGridSquareCollapsable">A function defining if the grid square at an index is collapsable, or if it should be seen as a void.</param>
        /// <param name="getSize">This gets the size of the grid. It's important this is a function, not an int, that way if the size changes it gets updated here.</param>
        /// <param name="socketsPerGridSquare">The sockets each Grid Square has, either 4 (up, down, right, left) or 8 (diagonals too).</param>
        public WaveFunctionCollapse(Grid grid, Func<Point, bool> isGridSquareCollapsable, Func<Point> getSize, Func<Point, WFCNode> indexer, Func<Point, WFCNode, bool, bool> setter, int socketsPerGridSquare = 8, int socketTypes = 8)
        {
            _grid = grid;
            _isGridSquareCollapsable = isGridSquareCollapsable;
            _getSize = getSize;
            _indexer = indexer;
            _setter = setter;
            _WFCNodeType = Convert.ToBoolean((socketsPerGridSquare / 4) - 1);

            _random = new ();
            _lowestEntropy  = new List<Point>();
            _lowestEntropyValue = 8;

            Initialize();            
        }

        private void Initialize()
        {
            foreach (Point point in Looper())
            {
                WFCNode node = new (_WFCNodeType);
                _setter(point, node, true);
                _grid.SetPathable(point, true);
            }

            _startPoint = new Point(_random.Next(0, _getSize().X), _random.Next(0, _getSize().Y));

            /*
            int x = 0;
            while (!_isGridSquareCollapsable(_startPoint))
            {
                _startPoint = new Point(_random.Next(0, _getSize().X), _random.Next(0, _getSize().Y));
                x++;

                if (x > 1000) { throw new Exception("ouch"); }
            
            */

            WFCNode startNode = _indexer(_startPoint);

            for (int i = 0; i < 8; i++)
            {
                startNode.Pack(2, i);
            }
            _lowestEntropy.Add(_startPoint);
            _lowestEntropyValue = 0;

            // _startPoint gets assigned all 2s in it's sockets
            for (int i=0; i<2000; i++) { Collapse(); }
        }

        public void Collapse()
        {
            if (_lowestEntropy.Count <= 0) { throw new Exception("woag"); }
            int i = _random.Next(0, _lowestEntropy.Count);

            Point nodePoint = _lowestEntropy[i];
            WFCNode node = GetRandomOption(nodePoint);
            _lowestEntropy.RemoveAt(i);

            _setter(nodePoint, node, false);
            UpdateNeighbors(nodePoint);

        }

        private void UpdateNeighbors(Point point)
        {
            WFCNode node = _indexer(point);
            //Debug.WriteLine("   Point: ");
            //node.Print();

            foreach (Point3 neighbor in GetNeighbors(point))
            {
                Point neighborPos = new(neighbor.X, neighbor.Y);
                //Debug.WriteLine(_grid.IsPathable(neighborPos) + " location: " + neighborPos.ToString() + " direction: " + neighbor.Z.ToString());

                if (_indexer(neighborPos) != null && _isGridSquareCollapsable(neighborPos))
                {
                    _indexer(neighborPos).Pack(node.GetSocket(neighbor.Z), MirrorSocket(neighbor.Z));
                    _indexer(neighborPos).Pack(node.GetSocket(neighbor.Z + 1), MirrorSocket(neighbor.Z + 1));
                    AddToEntropyList(neighborPos, node.GetEntropy());
                }
            }

        }

        public void AddToEntropyList(Point point, int entropy)
        {
            //Debug.WriteLine(point.ToString() + entropy.ToString());
            if (entropy > _lowestEntropyValue) { return; }
            
            if (entropy == _lowestEntropyValue) { _lowestEntropy.Add(point); return; }

            _lowestEntropy.Clear();
            _lowestEntropy.Add(point);
            _lowestEntropyValue = entropy;
        }
        private WFCNode GetRandomOption(Point point)
        {
            WFCNode self = _indexer(point);
            int bestScore = 0;
            WFCNode bestNode = WFCNodes.Invalid;

            foreach (WFCNode other in WFCNodes.nodes)
            {
                int score = -1;

                for (int i = 0; i < 8; i++)
                {
                    if (self.GetSocket(i) == other.GetSocket(i)) { score++; continue; }
                    if (self.GetSocket(i) != 0) { score = -10; }
                }

                if ((other.Name == "tile003" || other.Name == "tile008") && _random.Next(0, 4) > 0) { score += 1; }

                if (_random.Next(0, 2) == 0)
                {
                    if (score > bestScore) { bestScore = score; bestNode = other; }
                }
                else
                {
                    if (score >= bestScore) { bestScore = score; bestNode = other; }
                }
            }
            if (bestNode == WFCNodes.Invalid) { Debug.WriteLine("No matching node found!"); }
            //self.Print();
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
            foreach (WFCNode node in WFCNodes.nodes)
            {
                yield return node.Name;
            }
        }

    }
}
