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

        // Private variables
        private static List<Critter> _crittersList = new List<Critter>();

        private int _hp, _points, _slowed;
        private float _dexterity, _speed, _slowDamage;
        private Color _color, _slowColor;
        private Boolean _dead, _active;

        private LogicController logicClass;
        private CritterType crittersLevel;
        private List<Rectangle> paths = Presentation.Background.PathsList;
        private int critterStartX = 50,
                    critterStartY = 0,
                    waveLevel = 0;

        public Critter(LogicController parentClass)
        {
            logicClass = parentClass;
            //Console.WriteLine("Critter instantiated!");
        }

        public Critter()
        {
        }

        private void updateCritterState()
        {
            if (waveLevel < Constants.CritterLevelLength)
            {
                crittersLevel = CritterType.LowLevel;
            }
            else if (waveLevel < (Constants.CritterLevelLength * 2))
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
            if (CrittersList.Count > 0)
            {
                //Console.WriteLine("Move critter");
                float speed, xPos, yPos;
                Rectangle critterBox;
                foreach (Critter critter in CrittersList)
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

                    if (Presentation.Background.IsOnCity(critter))
                    {
                        // Critters have reached "The City" - Player lost a life
                        Player.Lives--;
                        critter.Die();
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
                    else if (!Presentation.Background.IsOnWalkPath(critter))
                    {
                        string critterName = "critter" + CrittersList.IndexOf(critter).ToString();
                        Console.WriteLine("Error: " + critterName + " is outside path");

                        critter.Die();
                        break;
                    }
                    else
                    {
                        if (paths[4].Intersects(critterBox))
                        {   // on fifth path (vertical)
                            critter.Move(xPos, yPos + speed);
                        }
                        else if (paths[3].Intersects(critterBox))
                        {   // on fourth path (horizontal)
                            critter.Move(xPos + speed, yPos);
                        }
                        else if (paths[2].Intersects(critterBox))
                        {   // on third path (vertical)
                            critter.Move(xPos, yPos - speed);
                        }
                        else if (paths[1].Intersects(critterBox))
                        {   // on second path (horizontal)
                            critter.Move(xPos + speed, yPos);
                        }
                        else if (paths[0].Intersects(critterBox))
                        {   // on first path (vertical)
                            critter.Move(xPos, yPos + speed);
                        }
                    }
                }
            }
        }

        public void UpdateCritters(SpriteBatch batch)
        {
            //Console.WriteLine("updateCritters");
            if (CrittersList.Count == 0)
            {
                if (logicClass.getSecondsSinceLast() > 10)
                {
                    createWave();
                    updateCritterState();
                    logicClass.saveWaveTime();
                } 
            }

            updateDrawWave(batch);
        }

        private void updateDrawWave(SpriteBatch batch)
        {
            if (CrittersList.Count > 0)
            {
                moveCritters();

                int crittersLength = CrittersList.Count;
                for (int i = 0; i < crittersLength; i++)
                {
                    Critter critter = CrittersList.ElementAt(i);
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
                        CrittersList.RemoveAt(CrittersList.IndexOf(critter));
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
            Critter critter = null;
            switch (crittersLevel)
            {
                case CritterType.LowLevel: critter = new LowLevel(); break;
                case CritterType.MediumLevel: critter = new MediumLevel(); break;
                case CritterType.HighLevel: critter = new HighLevel(); break;
            }

            critter.Move(critterStartX, critterStartY);

            critter.Texture = Presentation.PresentationController.CritterTexture2D;
            critter.Width = critter.Texture.Width;
            critter.Height = critter.Texture.Height;

            critter.HP += (int)(waveLevel * Constants.CritterLevelHpModifier);
            critter.Speed += waveLevel * Constants.CritterLevelSpeedModifier;
            critter.Points += (int)(waveLevel * Constants.CritterLevelPointsModifier);
            critter.Dexterity += waveLevel * Constants.CritterLevelDexterityModifier;

            critter.Dead = false;
            critter.Active = false;
            critter.Slowed = 0;

            if ((float)RandomHandler.GetRandom() < Constants.CritterUpgradeChance)
            {
                critter.upgradeCritter();
            }

            CrittersList.Add(critter);
            return critter;
        }

        private void upgradeCritter()
        {
            this.HP += waveLevel * 2;
            this.Speed *= 0.95f;
            this.CritterColor = Color.Black;
            this.Points += waveLevel / 6;
            this.Dexterity *= 1.25f;
            this.Width *= 1.5f;
            this.Height *= 1.5f;
        }

        private void createWave()
        {
            waveLevel++;
            int i = 0, 
                waveSize = calculateWaveSize();
            float yPos = 0f;
            Critter critter = null;
            for (i = 0; i < waveSize; i++)
            {
                //Console.WriteLine("Creating critter : " + i.ToString());
                critter = create();

                yPos = critter.Position.Y - ((critter.Height + RandomHandler.GetRandom(15)) * i);
                critter.Move(critter.Position.X, yPos);
            }
        }

        public void DamageCritter(int damage)
        {
            if (damage > 0)
            {
                this.HP -= damage;
                //Console.WriteLine("critter " + this.ToString() + " receives " + damage.ToString() + " damage");
                if (this.HP <= 0)
                {
                    this.Die();
                    Player.Gold += this.Points;
                }
            }
        }

        public void Die()
        {
            //Console.WriteLine("killing critter: " + critter.ToString());
            this.Dead = true;
        }

        private int calculateWaveSize()
        {
            int waveSize = Constants.CritterDefaultLevelSize + (int)(waveLevel * Constants.CritterLevelSizeModifier);
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

        public static List<Critter> CrittersList
        {
            set { _crittersList = value; }
            get { return _crittersList; }
        }

    }
}
