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
    struct Bullet
    {
        public Vector2 position;
        public Vector2 direction;
        public float speed;
        public Texture2D texture;
        public Boolean done;

        public void Update(float delta, int stageWidth, int stageHeight) 
        {
            float _bulletSpeedMultiplier = 50.0f;
           /* Console.WriteLine("Updating bullet. \ndirection: " + this.direction.ToString() +
                              "\nspeed: " + this.speed.ToString() +
                              "\ndelta: " + delta.ToString());*/
            if (!bulletDone(stageWidth, stageHeight))
            {
                //this.position += this.direction * this.speed * delta * _bulletSpeedMultiplier;
                this.position += this.direction * this.speed * _bulletSpeedMultiplier;
                //Console.WriteLine("Bullet updating position to: " + this.position.ToString());
            }

        }

        private Boolean bulletDone(int stageWidth, int stageHeight)
        {
            int _posX = (int)this.position.X,
                _posY = (int)this.position.Y;
            if (((_posX < 0 || _posX > stageWidth) || (_posY < 0 || _posY > stageHeight)) ||
                (this.direction.X < 0.01f && this.direction.Y < 0.01f)) 
            {
                this.position = Vector2.Zero;
                this.done = true;
            }

            return this.done;
        }
    }
}
