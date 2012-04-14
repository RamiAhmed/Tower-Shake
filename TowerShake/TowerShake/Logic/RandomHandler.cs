using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerShake.Logic
{
    public static class RandomHandler
    {
        private static Random _rand;

        public static void Init()
        {
            if (_rand == null)
            {
                _rand = new Random();
            }
        }

        public static int GetRandom(int min, int max)
        {
            if (max > min)
            {
                return _rand.Next(min, max);
            }
            else
            {
                return 0;
            }
        }

        public static int GetRandom(int max)
        {
            if (max > 0)
            {
                return _rand.Next(max);
            }
            else
            {
                return 0;
            }
        }

        public static double GetRandom(double min, double max)
        {
            if (max > min)
            {
                return (_rand.NextDouble() * (max - min)) + min;
            }
            else
            {
                return 0;
            }
        }

        public static double GetRandom(double max)
        {
            return GetRandom(0, max);
        }

        public static double GetRandom()
        {
            return _rand.NextDouble();
        }

    }
}
