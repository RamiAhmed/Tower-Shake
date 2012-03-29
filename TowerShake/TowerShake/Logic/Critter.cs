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

    class Critter
    {
        // Public variables
        public ArrayList critters = new ArrayList();
        public int level = 0;

        // Private variables
       // private Vector2 critterVector;
        private LogicController _logicClass;
        private int _x, _y, _hp, _speed;
        private float _dexterity;
        private Texture2D _texture;
        private Color _color;
        private int critterStartX = 72,
                    critterStartY = 40;
        private CritterType crittersLevel;

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
            if (level < 10)
            {
                crittersLevel = CritterType.LowLevel;
            }
            else if (level < 20)
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
            //Console.WriteLine("Move critter");
            int xPos, yPos, speed, width, height;
            try
            {
                foreach (Critter critter in critters)
                {
                    xPos = critter.X;
                    yPos = critter.Y;
                    speed = critter.Speed;
                    height = critter.Texture.Height / 2;
                    width = critter.Texture.Width / 2;

                    if (xPos < 375 + width)
                    {
                        // Critters are on the first lane (leftmost)
                        if (yPos < 380 + height)
                        {
                            // Critters move downwards
                            critter.Y += speed;
                        }
                        else
                        {
                            // Critters have reached the bottom of the first lane
                            critter.X += speed;
                        }
                    }
                    else if (xPos < 670 + width)
                    {
                        // Critters are on the second lane (middle)
                        if (yPos > 34 + height)
                        {
                            // Critters move upwards
                            critter.Y -= speed;
                        }
                        else
                        {
                            // Critters have reached the top of the second lane
                            critter.X += speed;
                        }
                    }
                    else
                    {
                        // Critters are on the third lane (rightmost)
                        if (yPos < 400 + height)
                        {
                            // Critters move downwards
                            critter.Y += speed;
                        }
                        else
                        {
                            // Critters have reached "The City" - Player lost a life
                            Player.lives--;
                            die(critter);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
        }

        public void updateCritters(SpriteBatch batch)
        {
            //Console.WriteLine("updateCritters");
            if (critters.Count == 0)
            {
                if (_logicClass.getSecondsSinceLast() > 10)
                {
                    createWave(batch);
                    updateCritterState();
                    _logicClass.saveWaveTime();
                }
            }
       
            updateDrawWave(batch);
        }

        private void updateDrawWave(SpriteBatch batch)
        {
            moveCritters();
            if (critters.Count > 0)
            {
                foreach (Critter critter in critters)
                {
                    batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                    batch.Draw(critter.Texture, new Vector2(critter.X, critter.Y), critter.CritterColor);
                    batch.End();
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
            _critter.X = critterStartX;
            _critter.Y = critterStartY;
            _critter.HP += level / 4;
            _critter.Speed += level / 10;
            _critter.Texture = Presentation.PresentationController.critter_circle;

            critters.Add(_critter);
            return _critter;
        }

        private void createWave(SpriteBatch batch)
        {
            level++;
            int i = 0, 
                _waveSize = calculateWaveSize(),
                yPos = 0;
            Vector2 startVector;
            Critter _critter = null;
            for (i = 0; i < _waveSize; i++)
            {
                //Console.WriteLine("Creating critter : " + i.ToString());
                _critter = create();

                yPos = _critter.Y - ((_critter.Texture.Height + 10) * i);
                _critter.Y = yPos;

                startVector = new Vector2(_critter.X, yPos);

                batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                batch.Draw(_critter.Texture, startVector, _critter.CritterColor);
                batch.End();
            }
        }

        private void die(Critter critter)
        {
            Console.WriteLine("killing critter");
            critters.Remove(critter);
            critter = null;
            _logicClass.saveWaveTime();
        }

        private int calculateWaveSize()
        {
            int waveSize = 10 + level;
            if (waveSize > 20)
            {
                waveSize = 20;
            }

            return waveSize;
        }

        public int X 
        {
            set {_x = value; }
            get { return _x; }
        }

        public int Y
        {
            set { _y = value; }
            get { return _y; }
        }

        protected int HP
        {
            set {_hp = value; }
            get { return _hp; }
        }

        protected int Speed
        {
            set { _speed = value; }
            get { return _speed; }
        }

        protected float Dexterity
        {
            set { _dexterity = value; }
            get { return _dexterity; }
        }

        protected Texture2D Texture
        {
            set { _texture = value; }
            get { return _texture; }
        }

        protected Color CritterColor
        {
            set { _color = value; }
            get { return _color; }
        }
    }
}
