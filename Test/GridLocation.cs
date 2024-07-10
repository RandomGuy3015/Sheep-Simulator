using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Test
{
    class GridLocation
    {
        //possible A* prep
        public bool IsFilled { get; private set; }
        public bool IsPathable { get; private set; }
        public bool IsPassable { get; private set; }

        public WFCNode Node { get; set; }

        private float mFScore, mCost, mCurrentDist;

        public GridLocation()
        {
            IsFilled = false;
            IsPathable = false;
            IsPassable = false;
        }

        public void SetToFilled(bool impassable)
        {
            IsFilled = true;
        }

        public void SetPathable(bool set)
        {
            IsPathable = set;
        }

        public void Clear(bool impassable)
        {
            IsFilled = false;
        }
    }
}
