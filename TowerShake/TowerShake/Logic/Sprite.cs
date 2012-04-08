using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerShake.Logic
{
    class Sprite
    {
        // Public variables

        // Private variables
        private Vector2 _pos, _dir;
        private Texture2D _texture;
        private static Random _rand;

        public Sprite()
        {
            _rand = new Random();
        }

        public static int GetRandom(int min, int max)
        {
            if (max > min)
            {
                return _rand.Next(min, max);
            }
            else
            {
                return -1;
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
                return -1;
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
                return -1.0;
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

        public static Boolean GetIsInRange(Vector2 one, Vector2 two, float range)
        {
            if (Math.Abs(one.X - two.X) < range && Math.Abs(one.Y - two.Y) < range)
            {
                return true;
            }

            return false;
        }

        public void Move(int x, int y)
        {
            this.Move((float)x, (float)y);
        }

        public void Move(float x, float y)
        {
            this.Position = new Vector2(x, y);
        }

        public Vector2 Position
        {
            set { _pos = value; }
            get { return _pos; }
        }

        public Vector2 Direction
        {
            set { _dir = value; }
            get { return _dir; }
        }

        public Texture2D Texture
        {
            set { _texture = value; }
            get { return _texture; }
        }

    }
}
