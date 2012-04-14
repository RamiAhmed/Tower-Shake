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

        // Public static variables
        public static Texture2D MouseTexture2D,
                                CritterTexture2D,
                                RangedTowerTexture2D,
                                SlowTowerTexture2D,
                                MeleeTowerTexture2D,
                                BulletTexture2D,
                                PathTexture2D,
                                CityTexture2D,
                                BackgroundTexture2D,
                                MeleeTowerButtonTexture2D,
                                RangedTowerButtonTexture2D,
                                SlowTowerButonTexture2D,
                                MainMenuTexture2D;

        // Private variables
        private List<Texture2D> loadedTextures = new List<Texture2D>();
        private Presentation.Background bg;
        private Game game; 
        private Vector2 gameClockVector,
                        gameLivesVector,
                        gameGoldVector;
        private SpriteFont gameFont;

        public PresentationController(Game game)
            : base(game)
        {
            this.game = game;
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

        private Texture2D loadTexture2D(string assetName)
        {
            Texture2D tex =  game.Content.Load<Texture2D>(assetName);
            if (tex != null)
            {
                loadedTextures.Add(tex);
            }
            else
            {
                Console.WriteLine("Error: Cannot load Texture2D by name: " + assetName);
            }
            return tex;
        }

        // Loads content at game start
        protected override void LoadContent()
        {
            MainMenuTexture2D = loadTexture2D("shake_menu");

            BackgroundTexture2D = loadTexture2D("background4");
            PathTexture2D = loadTexture2D("path_block");
            CityTexture2D = loadTexture2D("city");

            CritterTexture2D = loadTexture2D("critter1");
            MouseTexture2D = loadTexture2D("mouse");

            RangedTowerTexture2D = loadTexture2D("ranged_tower");
            SlowTowerTexture2D = loadTexture2D("slow_tower");
            MeleeTowerTexture2D = loadTexture2D("melee_tower");

            RangedTowerButtonTexture2D = loadTexture2D("ranged_tower_button");
            SlowTowerButonTexture2D = loadTexture2D("slow_tower_button");
            MeleeTowerButtonTexture2D = loadTexture2D("melee_tower_button");

            BulletTexture2D = loadTexture2D("white_bullet");

            gameFont = game.Content.Load<SpriteFont>("GameFont");

            bg = new Presentation.Background();

            base.LoadContent();
        }

        // Draws as often as possible
        public override void Draw(GameTime gameTime)
        {
            // Get the current sprite batch
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            if (Logic.GameStateHandler.CurrentGameState == Logic.GameState.MENU)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(MainMenuTexture2D, new Rectangle(100, 100, 600, 400), Color.White);
                spriteBatch.End();
            }
            else if (Logic.GameStateHandler.CurrentGameState == Logic.GameState.PAUSE)
            {
                string paused = "Game Paused";
                int pauseWidth = (int)gameFont.MeasureString(paused).X,
                    pauseHeight = (int)gameFont.MeasureString(paused).Y;
                Vector2 pos = new Vector2((Logic.Constants.StageWidth / 2) - (pauseWidth / 2), (Logic.Constants.StageHeight / 2) - (pauseHeight) / 2);
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.DrawString(gameFont, paused, pos, Color.Black);
                spriteBatch.End();                
            }
            else if (Logic.GameStateHandler.CurrentGameState == Logic.GameState.PLAY)
            {
                bg.DrawBackground(spriteBatch);

                int width = GraphicsDevice.Viewport.Width;
                int height = GraphicsDevice.Viewport.Height;
                int gold = Logic.Player.Gold;
                int lives = Logic.Player.Lives;

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                spriteBatch.DrawString(gameFont, getGameTime(), gameClockVector, Color.Black);
                spriteBatch.DrawString(gameFont, lives.ToString(), gameLivesVector, Color.Black);
                spriteBatch.DrawString(gameFont, gold.ToString(), gameGoldVector, Color.Black);

                spriteBatch.End();
            }
            else if (Logic.GameStateHandler.CurrentGameState == Logic.GameState.END)
            {
            }
            base.Draw(gameTime);
        }

        // Unloads content at game end
        protected override void UnloadContent()
        {
            foreach (Texture2D texture in loadedTextures)
            {
                texture.Dispose();
            }
        }

        private string getGameTime()
        {
            string gameTime = "";

            int _gameTimeSeconds = Logic.LogicController.TotalGameTimeSeconds,
                _gameTimeMinutes = Logic.LogicController.TotalGameTimeMinutes;
            string seconds = _gameTimeSeconds.ToString();
            if (_gameTimeSeconds < 10)
            {
                seconds = "0" + _gameTimeSeconds.ToString();
            }
            gameTime = _gameTimeMinutes.ToString() + ":" + seconds;

            return gameTime;
        }


    }
}
