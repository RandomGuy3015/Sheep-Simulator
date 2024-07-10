using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Test.Helper_classes
{
    internal class Point3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static Point3 Zero
        {
            get => new Point3(0, 0, 0);
        }


        public Point3(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

    }
}
