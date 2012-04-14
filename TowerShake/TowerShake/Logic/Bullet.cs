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
        private Boolean _done, _hit, _slow = false;
        private int _dmg;
        private Color _color;
        private Vector2 _dir;

        private float bulletSpeedMultiplier = Constants.BulletSpeedMultiplier;
        private int splashRange = Constants.SlowTowerSplashRange;
        

        public void Update(float delta, int stageWidth, int stageHeight) 
        {            
            if (!bulletDone(stageWidth, stageHeight))
            {
                this.Position += this.Direction * this.Speed * delta * bulletSpeedMultiplier;            
            }

        }

        private Boolean bulletDone(int stageWidth, int stageHeight)
        {
            int posX = (int)this.Position.X,
                posY = (int)this.Position.Y,
                range = 30;
            if (Sprite.GetIsInRange(this.Position, this.Target.Position, range) && this.Hit)
            {
                this.Position = new Vector2(-this.Texture.Width, -this.Texture.Height);
                this.Done = true;
                this.Target.DamageCritter(this.Damage);
                if (this.Slow)
                {
                    slow(this.Target);

                    foreach (Critter critter in Critter.CrittersList)
                    {
                        if (Sprite.GetIsInRange(this.Target.Position, critter.Position, splashRange) &&
                            critter != this.Target)
                        {
                            critter.DamageCritter(this.Damage);
                            slow(critter);
                        }
                    }
                }
                // Damaged critter
            }
            else if (((posX < 0 || posX > stageWidth) || (posY < 0 || posY > stageHeight)))
            {
                this.Position = new Vector2(-this.Texture.Width, -this.Texture.Height);
                this.Done = true;
                // Out of screen
            }

            return this.Done;
        }

        private void slow(Critter critter)
        {
            float slowDamage = Constants.SlowTowerSlowAmount; 

            if (critter.SlowDamage < slowDamage)
            {
                critter.SlowDamage = slowDamage; 
            }
            critter.Slowed = LogicController.getCurrentSeconds();
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

        public bool Slow
        {
            get { return _slow; }
            set { _slow = value; }
        }

        public Color BulletColor
        {
            get { return _color; }
            set { _color = value; }
        }

        public Vector2 Direction
        {
            set { _dir = value; }
            get { return _dir; }
        }

    }
}
