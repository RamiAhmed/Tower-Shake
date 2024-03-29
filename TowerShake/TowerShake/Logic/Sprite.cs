﻿using System;
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
        private Vector2 _pos;
        private Texture2D _texture;
        private float _width, _height;

        public Sprite()
        {

        }

 
        public static Boolean GetIsInRange(Vector2 one, Vector2 two, float range)
        {
            bool inRange = false;
            if (Math.Abs(one.X - two.X) < range && Math.Abs(one.Y - two.Y) < range)
            {
                inRange = true;
            }

            return inRange;
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

        public Texture2D Texture
        {
            set { _texture = value; }
            get { return _texture; }
        }

        public float Width
        {
            set { _width = value; }
            get { return _width; }
        }

        public float Height
        {
            set { _height = value; }
            get { return _height; }
        }

    }
}
