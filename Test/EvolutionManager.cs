using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.GameObjects.Interfaces;

namespace Test
{
    internal class EvolutionManager
    {
        private Random _random;

        public EvolutionManager()
        {
            _random = new Random();
        }

        public void Mate(IEvolveable a, IEvolveable b)
        {
            if (a.Age <= a.SexualMaturityAge || b.Age <= b.SexualMaturityAge) { return; }
            if (!SexuallyCompatible(a, b)) { return; }

            // Mutation rate
            float mR = (a.MutationRate + b.MutationRate) / 2f;

            if (a.Energy < GetMatingCost(a) || b.Energy < GetMatingCost(b)) { return; }

            // for each a.OffspringNumber
            //Mammal baby = new Mammal() { Age = 0f;
            //               SexualMaturityAge = GeneMixer(a.SexualMaturityAge, b.SexualMaturityAge, mR);
            //               ...}
            // _objectManager.Add(baby);

            a.Energy -= GetMatingCost(a);
            b.Energy -= GetMatingCost(b);
        }


        public bool SexuallyCompatible(IEvolveable a, IEvolveable b)
        {
            float c = 1f;
            c -= Math.Abs((a.MaxSize / b.MaxSize) - 1f);
            c -= Math.Abs((a.MaxSpeed / b.MaxSpeed) - 1f);
            c -= Math.Abs((a.SexualMaturityAge / b.SexualMaturityAge) - 1f);
            c -= Math.Abs((a.MaxEnergy / b.MaxEnergy) - 1f);
            c -= Math.Abs((a.Size / b.Size) - 1f);
            return c > 0f;
        }

        private float GeneMixer(float a, float b, float mR)
        {
            //     average of a and b, times mutation rate adjusted for scale, times random factor from -1 to 1
            return (a + b) / 2f + (a / b) * mR * (_random.NextSingle() - 0.5f) * 2f;
        }

        private static float GetMatingCost(IEvolveable a)
        {
            throw new NotImplementedException();
        }
    }

}
