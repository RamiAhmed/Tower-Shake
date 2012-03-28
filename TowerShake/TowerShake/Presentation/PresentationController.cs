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

namespace TowerShake.Presentation
{
    public class PresentationController : DrawableGameComponent
    {
        // Public variables
        public SpriteFont gameFont;
        public int STAGE_WIDTH = 800,
                   STAGE_HEIGHT = 600;

        // Static public variables
        public static Texture2D mouse;
        public static Texture2D critter_circle;

        // Private variables
        private Game _gameClass;
        private Texture2D bg;
        private Vector2 gameClockVector,
                        gameLivesVector,
                        gameGoldVector;

        public PresentationController(Game parentClass) 
                                    : base(parentClass)
        {
            _gameClass = parentClass;
            Console.WriteLine("PresentationController instantiated");

            init();
        }

        private void init()
        {
            gameClockVector = new Vector2(380, 13);
            gameGoldVector = new Vector2(540, 13);
            gameLivesVector = new Vector2(700, 13);
        }

        // Called in Game.cs under 'LoadContent'
        protected override void LoadContent()
        {
            bg = _gameClass.Content.Load<Texture2D>("background");
            critter_circle = _gameClass.Content.Load<Texture2D>("critter1");
            mouse = _gameClass.Content.Load<Texture2D>("mouse");

            gameFont = _gameClass.Content.Load<SpriteFont>("GameFont");

            base.LoadContent();
        }

        // Called in Game.cs under 'Draw'
        public override void Draw(GameTime gameTime)
        {
            // Get the current sprite batch
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            int width = this.GraphicsDevice.Viewport.Width;
            int height = this.GraphicsDevice.Viewport.Height;
            int gold = Logic.Player.gold;
            int lives = Logic.Player.lives;

            //Console.WriteLine("Gold: " + gold.ToString() + ", lives: " + lives.ToString());

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(bg, new Rectangle(0, 0, width, height), Color.White);
            /*spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            */
            spriteBatch.DrawString(gameFont, getGameTime(), gameClockVector, Color.Black);
            spriteBatch.DrawString(gameFont, lives.ToString(), gameLivesVector, Color.Black);
            spriteBatch.DrawString(gameFont, gold.ToString(), gameGoldVector, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Called in Game.cs under 'UnloadContent'
        protected override void UnloadContent()
        {
            critter_circle.Dispose();
            
            bg.Dispose();
        }

        
        private string getGameTime()
        {
            string gameTime = "";

            int _gameTimeSeconds = Logic.LogicController.totalGameTimeSeconds,
                _gameTimeMinutes = Logic.LogicController.totalGameTimeMinutes;
            
            gameTime = _gameTimeMinutes.ToString() + ":" + _gameTimeSeconds.ToString();
            //Console.WriteLine(gameTime);
            return gameTime;
        }

    }
}
