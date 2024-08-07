using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    internal class WFCNodes
    {
        public List<WFCNode> Nodes { get; }
        readonly public WFCNode Invalid;
        readonly public WFCNode Uncollapsed; 

        internal WFCNodes(List<WFCNode> nodes, int invalid, int uncollapsed)
        {
            Nodes = nodes;
            Invalid = nodes[invalid];
            Uncollapsed = nodes[uncollapsed];
        }
        public WFCNode this[int key]
        {
            get => Nodes[key];
        }

    }
        internal class WFCManager
    {
        WaveFunctionCollapse _ground;
        WaveFunctionCollapse _plants;

        Grid _groundGrid;
        Grid _plantGrid;

        WFCNodes _groundNodes;
        WFCNodes _plantsNodes;

        readonly Vector2 _size;
        readonly int _gridSquareSize;

        public WFCManager(Vector2 size, int gridSquareSize)
        {
            _size = size;
            _gridSquareSize = gridSquareSize;
            Init();
        }
        void Init()
        {
            _groundGrid = new(_size, _gridSquareSize, Vector2.Zero);
            _plantGrid = new(_size, _gridSquareSize, Vector2.Zero);

            _groundNodes = new(new List<WFCNode>()
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
            }, 0, 2);

            _plantsNodes = new(new List<WFCNode>()
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
            }, 0, 1);

            _ground = (new(_groundGrid, _groundGrid.IsPathable, () => new Point((int)_size.X, (int)_size.Y), _groundGrid.GetNode, _groundGrid.SetNode, _groundNodes));
            _plants = (new(_plantGrid, _plantGrid.IsPathable, () => new Point((int)_size.X, (int)_size.Y), _plantGrid.GetNode, _plantGrid.SetNode, _plantsNodes));

        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _ground.Draw(_spriteBatch);
            _plants.Draw(_spriteBatch);
        }

        public List<string> TextureExporter()
        {
            List<string> textures = new();

            foreach (string tile in _ground.TextureExporter())
            {
                textures.Add(tile + ".png");
            }
            foreach (string tile in _plants.TextureExporter())
            {
                textures.Add(tile + ".png");
            }

            return textures;
        }
    }
}
