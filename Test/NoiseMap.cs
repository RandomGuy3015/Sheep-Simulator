using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class NoiseMap
    {
        public int[] WoodGrid { get; }
        public int[] StoneGrid { get; }
        public int[] NegativeGrid { get; }
        public int WoodCutoff { get; }
        public int StoneCutoff { get; }


        private readonly int mSize;

        public NoiseMap(int seed, int size)
        {
            WoodGrid = new int[size * size];
            StoneGrid = new int[size * size];
            NegativeGrid = new int[size * size];
            mSize = size;
            int wood = seed / 1000000;
            int stone = (seed / 10000) % 100;
            int starterMap = ((seed / 100) % 100) * 2 / 2 + 16;
            int generations = seed % 100 + 160;

            WoodCutoff = (wood * -1 + 100) / 5;
            StoneCutoff = (stone * -1 + 100) / 5;

            InitMaps(starterMap);
            GenerateMaps(generations);

        }
        public NoiseMap(int wood, int stone, int seed, int size)
        {
            WoodGrid = new int[size * size];
            StoneGrid = new int[size * size];
            NegativeGrid = new int[size * size];
            mSize = size;
            int starterMap = (seed / 100) * 2 / 2 + 16;
            int generations = seed % 100 + 80;

            WoodCutoff = (wood * -1 + 100) / 5;
            StoneCutoff = (stone * -1 + 100) / 5;

            InitMaps(starterMap);
            GenerateMaps(generations);
        }

        private void InitMaps(int starterMap)
        {
            string bin = Convert.ToString(starterMap, 2);
            int[] starterWood = new int[] { 12, 14, 0, 0, 5, 6, 14, 0, 2, 13, 1, 0, Convert.ToInt32(bin[3]) + 9, Convert.ToInt32(bin[2]) + 9, Convert.ToInt32(bin[1]) + 9, Convert.ToInt32(bin[0]) + 9 };
            int[] starterStone = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(bin[0]) + 9, Convert.ToInt32(bin[1]) + 9, Convert.ToInt32(bin[2]) + 9, Convert.ToInt32(bin[3]) + 9 };
            int[] starterNegative = new int[] { 0, 14, 15, 15, 2, 0, 0, 11, 0, 1, 11, 0, Convert.ToInt32(bin[0]) + 9, Convert.ToInt32(bin[1]) + 9, Convert.ToInt32(bin[2]) + 9, Convert.ToInt32(bin[3]) + 9 };
            for (int y = mSize / 2 - 2; y < mSize / 2 + 2; y++)
            {
                for (int x = mSize / 2 - 2; x < mSize / 2 + 2; x++)
                {
                    WoodGrid[y * mSize + x] = starterWood[(y + 2 - (mSize / 2)) * 4 + (x + 2 - (mSize / 2))];
                    StoneGrid[y * mSize + x] = starterStone[(y + 2 - (mSize / 2)) * 4 + (x + 2 - (mSize / 2))];
                    NegativeGrid[y * mSize + x] = starterNegative[(y + 2 - (mSize / 2)) * 4 + (x + 2 - (mSize / 2))];
                }
            }
        }

        private void GenerateMaps(int generations)
        {
            while (generations > 0)
            {
                UpdateNeighbors();
                Iterate();
                generations--;
            }
            UpdateNeighbors();
            CreateForest();

            UpdateNeighbors();

        }
        private void Iterate()
        {
            for (int i = 0; i < mSize * mSize; i++)
            {
                if (WoodGrid[i] == 2 || WoodGrid[i] == 3)
                {
                    WoodGrid[i] = 13;
                }
                else if (WoodGrid[i] > 10 && WoodGrid[i] != 12 && WoodGrid[i] != 13)
                {
                    WoodGrid[i] = WoodGrid[i] - 10;
                }
                if (StoneGrid[i] == 2 || StoneGrid[i] == 3)
                {
                    StoneGrid[i] = 13;
                }
                else if (StoneGrid[i] > 10 && StoneGrid[i] != 12 && StoneGrid[i] != 13)
                {
                    StoneGrid[i] = StoneGrid[i] - 10;
                }
                if (NegativeGrid[i] == 2 || NegativeGrid[i] == 3)
                {
                    NegativeGrid[i] = 13;
                }
                else if (NegativeGrid[i] > 10 && NegativeGrid[i] != 12 && NegativeGrid[i] != 13)
                {
                    NegativeGrid[i] = NegativeGrid[i] - 10;
                }
            }
        }
        private void UpdateNeighbors()
        {
            for (int i = 0; i < mSize * mSize; i++)
            {
                if (WoodGrid[i] >= 10)
                {
                    WoodGrid[i] = 10 + GetNeighbors(i, 0);
                }
                else
                {
                    WoodGrid[i] = GetNeighbors(i, 0);
                }
                if (StoneGrid[i] >= 10)
                {
                    StoneGrid[i] = 10 + GetNeighbors(i, 1);
                }
                else
                {
                    StoneGrid[i] = GetNeighbors(i, 1);
                }
                if (NegativeGrid[i] >= 10)
                {
                    NegativeGrid[i] = 10 + GetNeighbors(i, 2);
                }
                else
                {
                    NegativeGrid[i] = GetNeighbors(i, 2);
                }
            }
        }

        private int GetNeighbors(int i, int map)
        {
            if (map == 0)
            {
                if (i < mSize) { i += mSize; }
                if (i > mSize * (mSize - 1) - 1) { i -= mSize; }
                if (i % mSize == 0) { i++; }
                if (i % mSize == mSize - 1) { i--; }
                return WoodGrid[i - mSize - 1] / 10 + WoodGrid[i - mSize] / 10 + WoodGrid[i - mSize + 1] / 10
                     + WoodGrid[i - 1] / 10 + WoodGrid[i + 1] / 10
                     + WoodGrid[i + mSize - 1] / 10 + WoodGrid[i + mSize] / 10 + WoodGrid[i + mSize + 1] / 10;
            }
            else if (map == 1)
            {
                if (i < mSize) { i += mSize; }
                if (i > mSize * (mSize - 1) - 1) { i -= mSize; }
                if (i % mSize == 0) { i++; }
                if (i % mSize == mSize - 1) { i--; }
                return StoneGrid[i - mSize - 1] / 10 + StoneGrid[i - mSize] / 10 + StoneGrid[i - mSize + 1] / 10
                     + StoneGrid[i - 1] / 10 + StoneGrid[i + 1] / 10
                     + StoneGrid[i + mSize - 1] / 10 + StoneGrid[i + mSize] / 10 + StoneGrid[i + mSize + 1] / 10;
            }
            else
            {
                if (i < mSize) { i += mSize; }
                if (i > mSize * (mSize - 1) - 1) { i -= mSize; }
                if (i % mSize == 0) { i++; }
                if (i % mSize == mSize - 1) { i--; }
                return NegativeGrid[i - mSize - 1] / 10 + NegativeGrid[i - mSize] / 10 + NegativeGrid[i - mSize + 1] / 10
                     + NegativeGrid[i - 1] / 10 + NegativeGrid[i + 1] / 10
                     + NegativeGrid[i + mSize - 1] / 10 + NegativeGrid[i + mSize] / 10 + NegativeGrid[i + mSize + 1] / 10;
            }
        }

        private void CreateForest()
        {

            for (int i = 0; i < mSize * mSize; i++)
            {
                if (NegativeGrid[i] >= 11 + WoodCutoff * 0.3)
                {
                    WoodGrid[i] = 0;
                }
            }

            for (int m = 4; m < 33; m += 8)
            {
                for (int y = 0; y < mSize / m; y++)
                {
                    for (int x = 0; x < mSize / m; x++)
                    {
                        if (WoodGrid[y * mSize + x] >= 13 && WoodGrid[y * mSize + x] <= 16)
                        {
                            for (int j = 0; j < m; j++)
                            {
                                for (int k = 0; k < m; k++)
                                {
                                    if (WoodGrid[(y * mSize * m + j * mSize + x * m + k)] > 1 && WoodGrid[(y * mSize * m + j * mSize + x * m + k)] < 4)
                                    {
                                        WoodGrid[(y * mSize * m + j * mSize + x * m + k)] = WoodGrid[y * mSize + x];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < mSize * mSize; i++)
            {
                if (NegativeGrid[i] >= 10 + WoodCutoff * 0.2)
                {
                    WoodGrid[i] = 0;
                }
            }
        }
    }
}
