using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerShake
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        // Private variables
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch spriteBatch;
        private Presentation.PresentationController presentationController;
        private Logic.LogicController logicController;

        public Game()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content\graphics";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            presentationController = new Presentation.PresentationController(this);
            logicController = new Logic.LogicController(this);

            Components.Add(presentationController);
            Components.Add(logicController);

            //this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / Logic.Constants.FPS);
            graphicsManager.IsFullScreen = false;
            graphicsManager.PreferredBackBufferWidth = Logic.Constants.StageWidth;
            graphicsManager.PreferredBackBufferHeight = Logic.Constants.StageHeight;
            graphicsManager.ApplyChanges();
            Window.Title = Logic.Constants.WindowTitle;

            Logic.GameStateHandler.CurrentGameState = Logic.GameState.MENU;

            Console.WriteLine("Initializing");
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            //Services.AddService(typeof(GraphicsDevice), GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Logic.Player.GameEnd)
            {               
                this.Exit();
            }

            // TODO: Add your update logic here

            //Console.WriteLine("Update");
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //if (Logic.GameStateHandler.CurrentGameState != Logic.GameState.PAUSE)
            //{
                GraphicsDevice.Clear(Color.DarkGray);
            //}

            // TODO: Add your drawing code here

            //Console.WriteLine("Draw");
            base.Draw(gameTime);
        }
    }
}
