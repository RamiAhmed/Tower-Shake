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

        // Private variables
        private static int _totalGameTimeMinutes = 0;
        private static int _totalGameTimeSeconds = 0;
        private static long _totalGameTimeMilliseconds = 0;
        private static int _lastWave = 0;

        private Player player;
        private Critter critter;
        private Tower tower;
        private Logic.Mouse mouse;
        private KeyboardHandler keyboard;
        private Game game;

        public LogicController(Game game) 
                        : base(game)
        {
            this.game = game;
            Console.WriteLine("LogicController instantiated");

            init();
        }

        private void init()
        {
            mouse = new Logic.Mouse();
            keyboard = new KeyboardHandler();

            player = new Player(this);
            critter = new Critter(this);
            tower = new Tower(this);

            RandomHandler.Init();
        }

        // Updates every 1/40th second
        public override void Update(GameTime gameTime)
        {
            if (Logic.GameStateHandler.CurrentGameState == Logic.GameState.MENU)
            {
                mouse.mouseHandler();
            }
            else if (Logic.GameStateHandler.CurrentGameState == Logic.GameState.PAUSE)
            {
                keyboard.keyboardHandler();
            }
            else if (GameStateHandler.CurrentGameState == GameState.PLAY)
            {
                updateGameClock(gameTime);
                mouse.mouseHandler();
                keyboard.keyboardHandler();
            }
            else if (GameStateHandler.CurrentGameState == GameState.END)
            {
            }

            base.Update(gameTime);
        }

        // Updates as often as possible
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            if (GameStateHandler.CurrentGameState == GameState.MENU)
            {
                mouse.DrawMouse(spriteBatch);
            }
            else if (GameStateHandler.CurrentGameState == GameState.PAUSE)
            {
                mouse.DrawMouse(spriteBatch);
            }
            else if (GameStateHandler.CurrentGameState == GameState.PLAY)
            {
                tower.UpdateTowers(spriteBatch, gameTime);
                critter.UpdateCritters(spriteBatch);
                mouse.DrawMouse(spriteBatch);
            }
            else if (GameStateHandler.CurrentGameState == GameState.END)
            {
            }

            base.Draw(gameTime);
        }

        private void updateGameClock(GameTime gameTime)
        {
            TotalGameTimeMilliseconds += (long)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (TotalGameTimeMilliseconds >= 1000)
            {
                TotalGameTimeMilliseconds = 0;

                TotalGameTimeSeconds += 1;

                if (TotalGameTimeSeconds >= 60)
                {
                    TotalGameTimeSeconds = 0;

                    TotalGameTimeMinutes += 1;
                    if (TotalGameTimeMinutes >= 60)
                    {
                        // if hours are implemented
                    }
                }
            }
        }

        public static int getCurrentSeconds()
        {
            return (TotalGameTimeMinutes * 60) + TotalGameTimeSeconds;
        }

        public int getSecondsSinceLast()
        {
            return getCurrentSeconds() - _lastWave;
        }

        public void saveWaveTime()
        {
            _lastWave = getCurrentSeconds();
        }

        public static int TotalGameTimeMinutes
        {
            get { return _totalGameTimeMinutes;}
            set { _totalGameTimeMinutes = value; }
        }

        public static int TotalGameTimeSeconds
        {
            get { return _totalGameTimeSeconds; }
            set { _totalGameTimeSeconds = value; }
        }

        public static long TotalGameTimeMilliseconds
        {
            get { return _totalGameTimeMilliseconds; }
            set { _totalGameTimeMilliseconds = value; }
        }
    }
}
