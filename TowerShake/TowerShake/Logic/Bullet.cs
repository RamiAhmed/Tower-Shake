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
    class Bullet : Sprite
    {
        private Critter _target;
        private float _speed;
        private Boolean _done, _hit;
        private int _dmg;

        float _bulletSpeedMultiplier = 1.5f;

        public void Update(float delta, int stageWidth, int stageHeight) 
        {            
            if (!bulletDone(stageWidth, stageHeight))
            {
                this.Position += this.Direction * this.Speed * delta * _bulletSpeedMultiplier;            
            }

        }

        private Boolean bulletDone(int stageWidth, int stageHeight)
        {
            int _posX = (int)this.Position.X,
                _posY = (int)this.Position.Y,
                _range = 30;
            if (Sprite.GetIsInRange(this.Position, this.Target.Position, _range) && this.Hit)
            {
                this.Position = Vector2.Zero;
                this.Done = true;
                this.Target.damageCritter(this.Damage);
                // Damaged critter
            }
            else if (((_posX < 0 || _posX > stageWidth) || (_posY < 0 || _posY > stageHeight)))
            {
                this.Position = Vector2.Zero;
                this.Done = true;
                // Out of screen
            }

            return this.Done;
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Boolean Done
        {
            get { return _done; }
            set { _done = value; }
        }

        public Critter Target
        {
            get { return _target; }
            set { _target = value; }
        }
        
        public int Damage
        {
            get { return _dmg; }
            set { _dmg = value; }
        }

        public Boolean Hit
        {
            get { return _hit; }
            set { _hit = value; }
        }

    }
}
