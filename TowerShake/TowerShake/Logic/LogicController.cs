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
    public class LogicController : DrawableGameComponent
    {
        // Public variables
        public static int totalGameTimeMinutes = 0;
        public static int totalGameTimeSeconds = 0;
        public static long timeMilliSeconds = 0;
        public int lastWave = 0;

        // Private variables
        private Player _player;
        private Critter _critter;
        private Tower _tower;
        private Logic.Mouse _mouse;        
        private Game _game;

        public LogicController(Game game) 
                        : base(game)
        {
            _game = game;
            Console.WriteLine("LogicController instantiated");

            init();
        }

        private void init()
        {
            _player = new Player(this);
            _critter = new Critter(this);
            _mouse = new Logic.Mouse();
            _tower = new Tower(this);
        }

        // Runs in Game.cs under 'Update'
        public override void Update(GameTime gameTime)
        { 
            updateGameClock(gameTime);
            _mouse.mouseHandler();

            if (Player.GameEnd)
            {
                _game.Exit();
            }

            base.Update(gameTime);
        }

        // Runs in Game.cs under 'Draw'
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            _tower.updateTowers(spriteBatch, gameTime);
            _critter.updateCritters(spriteBatch);
            _mouse.drawMouse(spriteBatch);

            base.Draw(gameTime);
        }

        private void updateGameClock(GameTime gameTime)
        {
            timeMilliSeconds += (long)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeMilliSeconds >= 1000)
            {
                timeMilliSeconds = 0;

                totalGameTimeSeconds += 1;

                if (totalGameTimeSeconds >= 60)
                {
                    totalGameTimeSeconds = 0;

                    totalGameTimeMinutes += 1;
                    if (totalGameTimeMinutes >= 60)
                    {
                        // if hours are implemented
                    }
                }
            }
        }

        public static int getCurrentSeconds()
        {
            return (totalGameTimeMinutes * 60) + totalGameTimeSeconds;
        }

        public int getSecondsSinceLast()
        {
            return getCurrentSeconds() - lastWave;
        }

        public void saveWaveTime()
        {
            lastWave = getCurrentSeconds();
        }
    }
}
