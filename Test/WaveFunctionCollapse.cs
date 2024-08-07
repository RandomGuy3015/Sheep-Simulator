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
            /*
            if (direction == 0 && Left == 0) { Left = value; }
            else if (direction == 1 && TopLeft == 0) { TopLeft = value; }
            else if (direction == 2 && Top == 0) { Top = value; }
            else if (direction == 3 && TopRight == 0) { TopRight = value; }
            else if (direction == 4 && Right == 0) { Right = value; }
            else if (direction == 5 && BottomRight == 0) { BottomRight = value; }
            else if (direction == 6 && Bottom == 0) { Bottom = value; }
            else if (direction == 7 && BottomLeft == 0) { BottomLeft = value; }
            */
            switch (direction)
            {
                case 0:
                    if (Left == 0) { Left = value; }
                    break;
                case 1:
                    if (TopLeft == 0) { TopLeft = value; }
                    break;
                case 2:
                    if (Top == 0) { Top = value; }
                    break;
                case 3:
                    if (TopRight == 0) { TopRight = value; }
                    break;
                case 4:
                    if (Right == 0) { Right = value; }
                    break;
                case 5:
                    if (BottomRight == 0) { BottomRight = value; }
                    break;
                case 6:
                    if (Bottom == 0) { Bottom = value; }
                    break;
                case 7:
                    if (BottomLeft == 0) { BottomLeft = value; }
                    break;
            }

            //else { throw new Exception("Error"); }
        }

        public int GetSocket(int dir)
        {
            /*
            if (dir == 0) { return Left; }
            if (dir == 1) { return TopLeft; }
            if (dir == 2) { return Top; }
            if (dir == 3) { return TopRight; }
            if (dir == 4) { return Right; }
            if (dir == 5) { return BottomRight; }
            if (dir == 6) { return Bottom; }
            if (dir == 7) { return BottomLeft; }
            else { return -1; }*/
            switch (dir)
            {
                case 0:
                    return Left;
                case 1:
                    return TopLeft;
                case 2:
                    return Top;
                case 3:
                    return TopRight;
                case 4:
                    return Right;
                case 5:
                    return BottomRight;
                case 6:
                    return Bottom;
                case 7:
                    return BottomLeft;
                default:
                    return -1;
            }
            
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
        //      0000_0000_0000_0000
        
        public int GetTileType()
        {
            int[] typeCount = new int[8];
            for (int i=0; i<8; i++)
            {
                if (GetSocket(i) < 0) { continue; }
                typeCount[GetSocket(i)] += 1;
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
            new WFCNode(1, 1, 1, 1, 1, 2, 2, 1, "tile000"),
            new WFCNode(2, 1, 1, 1, 1, 2, 2, 2, "tile001"),
            new WFCNode(2, 1, 1, 1, 1, 1, 1, 2, "tile002"), 
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile003"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile004"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile005"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile006"), 
            new WFCNode(1, 1, 1, 2, 2, 2, 2, 1, "tile007"),
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile008"), 
            new WFCNode(2, 2, 2, 1, 1, 1, 1, 2, "tile009"), 
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile010"), 
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile011"),
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile012"),
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile013"),
            new WFCNode(1, 1, 1, 2, 2, 1, 1, 1, "tile014"), 
            new WFCNode(1, 2, 2, 2, 2, 1, 1, 1, "tile015"), 
            new WFCNode(1, 2, 2, 1, 1, 1, 1, 1, "tile016"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile017"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile018"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile019"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile020"),
            new WFCNode(2, 2, 2, 1, 1, 2, 2, 2, "tile0200"),
            new WFCNode(2, 2, 2, 2, 2, 1, 1, 2, "tile0201"),
            new WFCNode(1, 2, 2, 2, 2, 2, 2, 1, "tile0202"),
            new WFCNode(2, 1, 1, 2, 2, 2, 2, 2, "tile0203"),
            new WFCNode(1, 1, 1, 2, 2, 2, 1, 1, "tile0204"),
            new WFCNode(1, 1, 1, 1, 1, 2, 2, 2, "tile0205"),
            new WFCNode(2, 2, 1, 1, 1, 1, 1, 2, "tile0206"),
            new WFCNode(1, 2, 2, 2, 1, 1, 1, 1, "tile0207"),
            new WFCNode(2, 1, 1, 2, 2, 1, 1, 2, "tile0208"),
            new WFCNode(2, 2, 2, 1, 1, 2, 2, 1, "tile0209"),
        };
        public static List<WFCNode> nodes2 = new()
        {
            new WFCNode(1, 1, 1, 2, 2, 3, 3, 3, "tile0210"),
            new WFCNode(3, 2, 2, 1, 1, 1, 3, 3, "tile0211"),
            new WFCNode(2, 2, 2, 2, 2, 2, 2, 2, "tile0212"),
            new WFCNode(3, 2, 2, 2, 2, 3, 3, 3, "tile0228"),
            new WFCNode(1, 1, 1, 2, 2, 2, 2, 1, "tile0213"),
            new WFCNode(2, 2, 2, 1, 1, 1, 1, 2, "tile0214"),
            new WFCNode(4, 4, 1, 2, 2, 2, 2, 1, "tile0215"),
            new WFCNode(2, 2, 2, 1, 4, 4, 1, 2, "tile0216"),
            new WFCNode(1, 1, 1, 1, 4, 4, 1, 2, "tile0217"),
            new WFCNode(4, 4, 1, 1, 1, 1, 2, 1, "tile0218"),
            new WFCNode(4, 4, 4, 4, 1, 1, 2, 1, "tile0219"),
            new WFCNode(1, 1, 4, 4, 4, 4, 1, 2, "tile0220"),
            new WFCNode(4, 4, 1, 1, 1, 1, 1, 1, "tile0221"),
            new WFCNode(1, 1, 1, 1, 4, 4, 1, 1, "tile0222"),
            new WFCNode(4, 4, 4, 4, 4, 4, 1, 1, "tile0223"),
            new WFCNode(4, 4, 4, 4, 4, 4, 1, 1, "tile0224"),
            new WFCNode(1, 1, 1, 1, 1, 1, 4, 4, "tile0225"),
            new WFCNode(4, 4, 4, 4, 1, 1, 4, 4, "tile0226"),
            new WFCNode(1, 1, 4, 4, 4, 4, 4, 4, "tile0227"),
            new WFCNode(1, 1, 1, 1, 1, 1, 1, 1, "tile0229"),
            new WFCNode(1, 1, 3, 3, 1, 1, 1, 1, "tile0230"),

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

        private Dictionary<int, List<Point>> _entropy;

        private int _lowestEntropy;

        private Point _startPoint;

        private bool _isDecoGrid;

        readonly private Random _random;



        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <param name="grid">A newly initialized grid, defined however you want.</param>
        /// <param name="isGridSquareCollapsable">A function defining if the grid square at an index is collapsable, or if it should be seen as a void.</param>
        /// <param name="getSize">This gets the size of the grid. It's important this is a function, not an int, that way if the size changes it gets updated here.</param>
        /// <param name="socketsPerGridSquare">The sockets each Grid Square has, either 4 (up, down, right, left) or 8 (diagonals too).</param>
        public WaveFunctionCollapse(Grid grid, Func<Point, bool> isGridSquareCollapsable, Func<Point> getSize, Func<Point, WFCNode> indexer, Func<Point, WFCNode, bool, bool> setter, bool isDecoGrid, int socketsPerGridSquare = 8, int socketTypes = 8)
        {
            _grid = grid;
            _isGridSquareCollapsable = isGridSquareCollapsable;
            _getSize = getSize;
            _indexer = indexer;
            _setter = setter;
            _WFCNodeType = Convert.ToBoolean((socketsPerGridSquare / 4) - 1);
            _isDecoGrid = isDecoGrid;

            _random = new ();
            _entropy  = new Dictionary<int, List<Point>>();
            _lowestEntropy = 8;

            Initialize();            
        }

        private void Initialize()
        {
            WFCNode startNode;

            foreach (Point point in Looper())
            {
                WFCNode node = new (_WFCNodeType);
                _setter(point, node, true);
                _grid.SetPathable(point, true);
            }



            if (_isDecoGrid)
            {
                for (int i = 0; i < 9; i++)
                {
                    _entropy.Add(i, new List<Point>());
                }

                for (int j = 0; j < 100; j++)
                {
                    _startPoint = new Point(_random.Next(0, _getSize().X - 1), _random.Next(_getSize().Y - 1));
                    startNode = _indexer(_startPoint);

                    _setter(_startPoint, WFCNodes.nodes2[0], false);
                    _entropy[8].Add(_startPoint);
                }

            }
            else
            {
                _startPoint = new Point(_getSize().X - 1, _getSize().Y - 1);

                for (int i = 0; i < 9; i++)
                {
                    _entropy.Add(i, new List<Point>());
                }

                startNode = _indexer(_startPoint);

                for (int i = 0; i < 8; i++)
                {
                    startNode.Pack(2, i);
                }
                _entropy[8].Add(_startPoint);

            }

            while (Collapse()) { }
        }

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
                    AddToEntropyList(neighborPos, _indexer(neighborPos).GetEntropy());
                }
            }

        }

        public void AddToEntropyList(Point point, int entropy)
        {
            //Debug.WriteLine(point.ToString() + entropy.ToString());
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
            WFCNode bestNode = WFCNodes.Invalid;

            foreach (WFCNode other in Nodes())
            {
                float score = -1f;

                for (int i = 0; i < 8; i++)
                {
                    if (self.GetSocket(i) == other.GetSocket(i)) { score++; continue; }
                    if (self.GetSocket(i) != 0) { score = -10f; }
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
            
            
            /*
            {
                foreach (Point3 neighbor in GetNeighbors(point))
                {
                    Point neighborPos = new(neighbor.X, neighbor.Y);
                    _indexer(neighborPos).Pack(-1, MirrorSocket(neighbor.Z));
                    _indexer(neighborPos).Pack(-1, MirrorSocket(neighbor.Z + 1));
                    AddToEntropyList(neighborPos, _indexer(neighborPos).GetEntropy());
                }
            }
            */
            
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

        public IEnumerable<WFCNode> Nodes()
        {
            if (_isDecoGrid)
            {
                return WFCNodes.nodes2;
            }
            else
            {
                return WFCNodes.nodes;
            }
        }

        public IEnumerable<string> TextureExporter()
        {
            foreach (WFCNode node in Nodes())
            {
                yield return node.Name;
            }
        }

    }
}
