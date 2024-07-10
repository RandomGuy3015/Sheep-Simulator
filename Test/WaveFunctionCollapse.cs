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
        private bool _WFCNodeType;

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
        }

        public WFCNode(int left, int topLeft, int top, int topRight, int right, int bottomRight, int bottom, int bottomLeft)
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
            else { throw new Exception("Error"); }
        }

        public void Print()
        {
            Debug.WriteLine(Left.ToString() + " " + TopLeft.ToString() + " " 
                + Top.ToString() + " " + TopRight.ToString() + " " 
                + Right.ToString() + " " + BottomRight.ToString() + " " 
                + Bottom.ToString() + " " + BottomLeft.ToString());
        }

    }

    internal class WFCNodes
    {
        internal WFCNodes() { }

        public static WFCNode Invalid = new(-1, -1, -1, -1, -1, -1, -1, -1);
        public static WFCNode Node0 = new(2, 2, 2, 2, 2, 2, 2, 2);
        public static WFCNode Node1 = new(2, 2, 2, 2, 1, 1, 1, 1);
        public static WFCNode Node2 = new(1, 1, 1, 1, 2, 2, 2, 2);
        public static WFCNode Node3 = new(1, 1, 1, 1, 1, 1, 1, 1);

        public static Texture2D InvalidTexture = DynamicContentManager.Instance.Load<Texture2D>("invalidTexture");
        public static Texture2D Node0Texture = DynamicContentManager.Instance.Load<Texture2D>("node0Texture");
        public static Texture2D Node1Texture = DynamicContentManager.Instance.Load<Texture2D>("node1Texture");
        public static Texture2D Node2Texture = DynamicContentManager.Instance.Load<Texture2D>("node2Texture");
        public static Texture2D Node3Texture = DynamicContentManager.Instance.Load<Texture2D>("node3Texture");
        /*
        public static WFCNode Node4 = new(2, 2, 2, 2, 2, 2, 2, 2);
        public static WFCNode Node5 = new(2, 2, 2, 2, 2, 2, 2, 2);
        public static WFCNode Node6 = new(2, 2, 2, 2, 2, 2, 2, 2);
        public static WFCNode Node7 = new(2, 2, 2, 2, 2, 2, 2, 2);
        */
        public static Texture2D GetTexture(WFCNode node)
        {
            if (node == Invalid) { return InvalidTexture; }
            if (node == Node0) { return Node0Texture; }
            if (node == Node1) { return Node1Texture; }
            if (node == Node2) { return Node2Texture; }
            if (node == Node3) { return Node3Texture; }
            node.Print();
            return null;
        }
    }
    internal class WaveFunctionCollapse
    {
        // The grid.
        readonly private Grid _grid;

        // This tells the class if the grid Square is to be collapsed.
        readonly private Func<Point, bool> _isGridSquareCollapsable;

        readonly private Func<Point, WFCNode> _indexer;

        readonly private Func<Point, WFCNode, bool> _setter;

        // The function to get the size of the grid.
        readonly private Func<Point> _getSize;

        // This defines how the WFCNode is packed. False is 4*4 bits, True is 4*8 bits.
        readonly private bool _WFCNodeType;

        private List<Point> _lowestEntropy;

        private int _lowestEntropyValue;

        private Point _startPoint;

        private List<WFCNode> _WFCNodes;


        readonly private Random _random;



        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <param name="grid">A newly initialized grid, defined however you want.</param>
        /// <param name="isGridSquareCollapsable">A function defining if the grid square at an index is collapsable, or if it should be seen as a void.</param>
        /// <param name="getSize">This gets the size of the grid. It's important this is a function, not an int, that way if the size changes it gets updated here.</param>
        /// <param name="socketsPerGridSquare">The sockets each Grid Square has, either 4 (up, down, right, left) or 9 (diagonals too).</param>
        public WaveFunctionCollapse(Grid grid, Func<Point, bool> isGridSquareCollapsable, Func<Point> getSize, Func<Point, WFCNode> indexer, Func<Point, WFCNode, bool> setter,  int socketsPerGridSquare = 8, int socketTypes = 8)
        {
            _grid = grid;
            _isGridSquareCollapsable = isGridSquareCollapsable;
            _getSize = getSize;
            _indexer = indexer;
            _setter = setter;
            _WFCNodeType = Convert.ToBoolean((socketsPerGridSquare / 4) - 1);
            _WFCNodes = new List<WFCNode>() { WFCNodes.Node0, WFCNodes.Node1, WFCNodes.Node2, WFCNodes.Node3 };

            _random = new ();

            Initialize();            
        }

        private void Initialize()
        {
            foreach (Point point in Looper())
            {
                WFCNode node = new(_WFCNodeType);
                _setter(point, node);

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

            for (int i = 0; i < 8; i++)
            {
                _indexer(_startPoint).Pack(2, i);
            }

            /*
            // _startPoint gets assigned all 2s in it's sockets
            foreach (Point3 point in GetNeighbors(_startPoint))
            {
                Point neighbor = new (point.X, point.Y);

                if (_indexer(neighbor) != null)
                {
                    _indexer(neighbor).Pack(2, point.Z);
                    AddToEntropyList(neighbor, 7);
                }


            }
            */

        }

        public void Collapse()
        {
            int i = _random.Next(0, _lowestEntropy.Count);

            WFCNode node = GetRandomOption(_lowestEntropy[i]);

            _setter(_lowestEntropy[i], node);

            _lowestEntropy.RemoveAt(i);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Point point in Looper())
            {
                spriteBatch.Draw(WFCNodes.GetTexture(_indexer(point)), new Rectangle(point.X, point.Y, 10, 10), Color.White);
            }
        }


        public void AddToEntropyList(Point point, int entropy)
        {
            //if (_lowestEntropy.Count == 0) { throw new Exception("Uhhh.. ");  }
            
            if (entropy > _lowestEntropyValue) { return; }
            
            if (entropy == _lowestEntropyValue) { _lowestEntropy.Add(point); return; }

            _lowestEntropy.Clear();
            _lowestEntropy.Add(point);
            _lowestEntropyValue = entropy;
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

        public IEnumerable<Point3> GetNeighbors(Point point)
        {
            if (!_WFCNodeType)
            {
                yield return new Point3(point.X - 1, point.Y, 0);
                yield return new Point3(point.X + 1, point.Y, 4);
                yield return new Point3(point.X, point.Y - 1, 2);
                yield return new Point3(point.X, point.Y + 1, 6);
            }
            else
            {
                yield return new Point3(point.X - 1, point.Y - 1, 1);
                yield return new Point3(point.X, point.Y - 1, 2);
                yield return new Point3(point.X + 1, point.Y - 1, 3);
                yield return new Point3(point.X - 1, point.Y, 0);
                yield return new Point3(point.X + 1, point.Y, 4);
                yield return new Point3(point.X - 1, point.Y + 1, 7);
                yield return new Point3(point.X, point.Y + 1, 6);
                yield return new Point3(point.X + 1, point.Y + 1, 5);
            }
        }

        private WFCNode GetRandomOption(Point point)
        {
            WFCNode self = _indexer(point);

            foreach (WFCNode other in _WFCNodes)
            {
                if (self.Left != 0 && self.Left != other.Left) { continue; }
                if (self.TopLeft != 0 && self.TopLeft != other.TopLeft) { continue; }
                if (self.Top != 0 && self.Top != other.Top) { continue; }
                if (self.TopRight != 0 && self.TopRight != other.TopRight) { continue; }
                if (self.Right != 0 && self.Right != other.Right) { continue; }
                if (self.BottomRight != 0 && self.BottomRight != other.BottomRight) { continue; }
                if (self.Bottom != 0 && self.Bottom != other.Bottom) { continue; }
                if (self.BottomLeft != 0 && self.BottomLeft != other.BottomLeft) { continue; }

                return other;
            }
            return WFCNodes.Invalid;
        }
    }
}
