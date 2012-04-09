using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerShake.Logic
{
    enum CritterType { LowLevel, MediumLevel, HighLevel };

    class Critter : Sprite
    {
        // Public variables
        public static List<Critter> critters = new List<Critter>();
        public int level = 0;

        // Private variables
        private LogicController _logicClass;
        private int _hp, _points, _slowed;
        private float _dexterity, _speed, _slowDamage;
        private Color _color, _slowColor;
        private Boolean _dead, _active;
        private CritterType crittersLevel;
        private List<Rectangle> _paths = Presentation.Background.paths;
        private int critterStartX = 50,
                    critterStartY = 0;

        public Critter(LogicController parentClass)
        {
            _logicClass = parentClass;
            Console.WriteLine("Critter instantiated!");
        }

        public Critter()
        {
        }

        private void updateCritterState()
        {
            if (level < Constants.CritterLevelLength)
            {
                crittersLevel = CritterType.LowLevel;
            }
            else if (level < (Constants.CritterLevelLength * 2))
            {
                crittersLevel = CritterType.MediumLevel;
            }
            else
            {
                crittersLevel = CritterType.HighLevel;
            }
        }

        private void moveCritters()
        {
            if (critters.Count > 0)
            {
                //Console.WriteLine("Move critter");
                float speed, xPos, yPos;
                Rectangle critterBox;
                foreach (Critter critter in critters)
                {
                    xPos = critter.Position.X;
                    yPos = critter.Position.Y;
                    speed = critter.Speed;
                    critterBox = new Rectangle((int)xPos, (int)yPos, 
                        (int)(critter.Width * 0.5f), (int)(critter.Height * 0.5f));

                    if (critter.Slowed != 0)
                    {
                        speed *= 1f - critter.SlowDamage;
                    }

                    if (Presentation.Background.isOnCity(critter))
                    {
                        // Critters have reached "The City" - Player lost a life
                        Player.Lives--;
                        critter.die();
                        break;
                    }

                    if (!critter.Active)
                    {
                        critter.Move(xPos, yPos + speed);
                        if (yPos > 0)
                        {
                            critter.Active = true;
                        }
                    }
                    else if (!Presentation.Background.isOnWalkPath(critter))
                    {
                        string critterName = "critter" + critters.IndexOf(critter).ToString();
                        Console.WriteLine("Error: " + critterName + " is outside path");

                        critter.die();
                        break;
                    }
                    else
                    {
                        if (_paths[4].Intersects(critterBox))
                        {   // on fifth path (vertical)
                            critter.Move(xPos, yPos + speed);
                        }
                        else if (_paths[3].Intersects(critterBox))
                        {   // on fourth path (horizontal)
                            critter.Move(xPos + speed, yPos);
                        }
                        else if (_paths[2].Intersects(critterBox))
                        {   // on third path (vertical)
                            critter.Move(xPos, yPos - speed);
                        }
                        else if (_paths[1].Intersects(critterBox))
                        {   // on second path (horizontal)
                            critter.Move(xPos + speed, yPos);
                        }
                        else if (_paths[0].Intersects(critterBox))
                        {   // on first path (vertical)
                            critter.Move(xPos, yPos + speed);
                        }
                    }
                }
            }
        }

        public void updateCritters(SpriteBatch batch)
        {
            //Console.WriteLine("updateCritters");
            if (critters.Count == 0)
            {
                if (_logicClass.getSecondsSinceLast() > 10)
                {
                    createWave();
                    updateCritterState();
                    _logicClass.saveWaveTime();
                } 
            }

            updateDrawWave(batch);
        }

        private void updateDrawWave(SpriteBatch batch)
        {
            if (critters.Count > 0)
            {
                moveCritters();

                int crittersLength = critters.Count;
                for (int i = 0; i < crittersLength; i++)
                {
                    Critter critter = critters.ElementAt(i);
                    if (!critter.Dead)
                    {
                        critter.checkForSlow();

                        Color color = critter.CritterColor;
                        if (critter.Slowed != 0)
                        {
                            color = critter.SlowColor;
                        }

                        Rectangle critterRect = new Rectangle((int)critter.Position.X, (int)critter.Position.Y,
                                                              (int)critter.Width, (int)critter.Height);
                        batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                        batch.Draw(critter.Texture, critterRect, color);
                        batch.End();
                    }
                    else
                    {
                        critters.RemoveAt(critters.IndexOf(critter));
                        critter = null;

                        crittersLength--;
                        i--;
                    }
                }
            }
        }

        private void checkForSlow()
        {
            if (this.Slowed != 0)
            {
                if (LogicController.getCurrentSeconds() - this.Slowed > Constants.SlowDuration)
                {
                    this.Slowed = 0;
                    this.SlowDamage = 0f;
                }
            }
        }

        private Critter create()
        {
            Critter _critter = null;
            switch (crittersLevel)
            {
                case CritterType.LowLevel: _critter = new LowLevel(); break;
                case CritterType.MediumLevel: _critter = new MediumLevel(); break;
                case CritterType.HighLevel: _critter = new HighLevel(); break;
            }

            _critter.Move(critterStartX, critterStartY);

            _critter.Texture = Presentation.PresentationController.critter_circle;
            _critter.Width = _critter.Texture.Width;
            _critter.Height = _critter.Texture.Height;

            _critter.HP += (int)(level * Constants.CritterLevelHpModifier);
            _critter.Speed += level * Constants.CritterLevelSpeedModifier;
            _critter.Points += (int)(level * Constants.CritterLevelPointsModifier);
            _critter.Dexterity += level * Constants.CritterLevelDexterityModifier;

            _critter.Dead = false;
            _critter.Active = false;
            _critter.Slowed = 0;

            if ((float)Sprite.GetRandom() < Constants.CritterUpgradeChance)
            {
                _critter.upgradeCritter();
            }

            critters.Add(_critter);
            return _critter;
        }

        private void upgradeCritter()
        {
            this.HP += level * 2;
            this.Speed *= 0.95f;
            this.CritterColor = Color.Black;
            this.Points += level / 6;
            this.Dexterity *= 1.25f;
            this.Width *= 1.5f;
            this.Height *= 1.5f;
        }

        private void createWave()
        {
            level++;
            int i = 0, 
                _waveSize = calculateWaveSize();
            float yPos = 0f;
            Critter _critter = null;
            for (i = 0; i < _waveSize; i++)
            {
                //Console.WriteLine("Creating critter : " + i.ToString());
                _critter = create();

                yPos = _critter.Position.Y - ((_critter.Texture.Height + 5 + GetRandom(10)) * i);
                _critter.Move(_critter.Position.X, yPos);
            }
        }

        public void damageCritter(int damage)
        {
            if (damage > 0)
            {
                this.HP -= damage;
                //Console.WriteLine("critter " + this.ToString() + " receives " + damage.ToString() + " damage");
                if (this.HP <= 0)
                {
                    this.die();
                    Player.Gold += this.Points;
                }
            }
        }

        public void die()
        {
            //Console.WriteLine("killing critter: " + critter.ToString());
            this.Dead = true;
        }

        private int calculateWaveSize()
        {
            int waveSize = Constants.CritterDefaultLevelSize + (int)(level * Constants.CritterLevelSizeModifier);
            if (waveSize > Constants.CritterLevelMaxSize)
            {
                waveSize = Constants.CritterLevelMaxSize;
            }

            return waveSize;
        }

        public int HP
        {
            set {_hp = value; }
            get { return _hp; }
        }

        public float Speed
        {
            set { _speed = value; }
            get { return _speed; }
        }

        public float Dexterity
        {
            set { _dexterity = value; }
            get { return _dexterity; }
        }

        public Color CritterColor
        {
            set { _color = value; }
            get { return _color; }
        }

        protected int Points
        {
            set { _points = value; }
            get { return _points; }
        }

        private Boolean Dead
        {
            set { _dead = value; }
            get { return _dead; }
        }

        private Boolean Active
        {
            set { _active = value; }
            get { return _active; }
        }

        public int Slowed
        {
            set { _slowed = value; }
            get { return _slowed; }
        }

        public float SlowDamage
        {
            set { _slowDamage = value; }
            get { return _slowDamage; }
        }

        public Color SlowColor
        {
            set { _slowColor = value; }
            get { return _slowColor; }
        }

    }
}
