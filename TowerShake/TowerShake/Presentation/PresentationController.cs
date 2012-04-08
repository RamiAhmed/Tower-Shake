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
        public static int STAGE_WIDTH = 800,
                          STAGE_HEIGHT = 600;
        
        // Static public variables
        public static Texture2D mouse;
        public static Texture2D critter_circle;
        public static Texture2D ranged_tower;
        public static Texture2D slow_tower;
        public static Texture2D melee_tower;
        public static Texture2D black_bullet;
        public static Texture2D path_block;
        public static Texture2D city;
        public static Texture2D bgTexture;

        // Private variables
        private Background bg;
        private Game _gameClass; 
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
            float yPos = 20;
            gameClockVector = new Vector2(380, yPos);
            gameGoldVector = new Vector2(545, yPos);
            gameLivesVector = new Vector2(705, yPos);         
        }

        // Called in Game.cs under 'LoadContent'
        protected override void LoadContent()
        {
            bgTexture = _gameClass.Content.Load<Texture2D>("background3");
            path_block = _gameClass.Content.Load<Texture2D>("path_block");
            city = _gameClass.Content.Load<Texture2D>("city");

            critter_circle = _gameClass.Content.Load<Texture2D>("critter1");
            mouse = _gameClass.Content.Load<Texture2D>("mouse");

            ranged_tower = _gameClass.Content.Load<Texture2D>("ranged_tower");
            slow_tower = _gameClass.Content.Load<Texture2D>("slow_tower");
            melee_tower = _gameClass.Content.Load<Texture2D>("melee_tower");

            black_bullet = _gameClass.Content.Load<Texture2D>("black_bullet");

            gameFont = _gameClass.Content.Load<SpriteFont>("GameFont");

            bg = new Background();

            base.LoadContent();
        }

        // Called in Game.cs under 'Draw'
        public override void Draw(GameTime gameTime)
        {
            // Get the current sprite batch
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            bg.drawBackground(spriteBatch);

            int width = this.GraphicsDevice.Viewport.Width;
            int height = this.GraphicsDevice.Viewport.Height;
            int gold = Logic.Player.Gold;
            int lives = Logic.Player.Lives;

            //Console.WriteLine("Gold: " + gold.ToString() + ", lives: " + lives.ToString());

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.Draw(bg, new Rectangle(0, 0, width, height), Color.White);
            //spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.DrawString(gameFont, getGameTime(), gameClockVector, Color.Black);
            spriteBatch.DrawString(gameFont, lives.ToString(), gameLivesVector, Color.Black);
            spriteBatch.DrawString(gameFont, gold.ToString(), gameGoldVector, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Called in Game.cs under 'UnloadContent'
        protected override void UnloadContent()
        {
            mouse.Dispose();
            critter_circle.Dispose();

            ranged_tower.Dispose();
            slow_tower.Dispose();
            melee_tower.Dispose();

            black_bullet.Dispose();

            city.Dispose();
            path_block.Dispose();
            bgTexture.Dispose();
        }


        private string getGameTime()
        {
            string gameTime = "";

            int _gameTimeSeconds = Logic.LogicController.totalGameTimeSeconds,
                _gameTimeMinutes = Logic.LogicController.totalGameTimeMinutes;
            string seconds = _gameTimeSeconds.ToString();
            if (_gameTimeMinutes > 0 && _gameTimeSeconds < 10)
            {
                seconds = "0" + _gameTimeSeconds.ToString();
            }
            gameTime = _gameTimeMinutes.ToString() + ":" + seconds;
            //Console.WriteLine(gameTime);
            return gameTime;
        }



    }
}
