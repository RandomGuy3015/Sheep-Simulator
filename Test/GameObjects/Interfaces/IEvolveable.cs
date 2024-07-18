using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Test.GameObjects.Interfaces
{
    internal interface IEvolveable
    {
        public float MaxEnergy { get; set; }
        public float Energy { get; set; }

        public float MutationRate { get; set; }
        public float OffspringNumber { get; set; }
        public float OffspringSize { get; set; }

        public float MaxAge { get; set; }
        public float Age { get; set; }
        public float SexualMaturityAge {  get; set; }

        public float MaxSpeed { get; set; }
        public float Speed { get; set; }
        public float MaxSize { get; set; }
        public float Size { get; set; }
        public float Sight { get; set; }
    }
}
