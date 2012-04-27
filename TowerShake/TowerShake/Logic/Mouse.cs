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
    class Mouse : Sprite
    {
        // Public variables

        // Private variables
        private MouseState currentMouse,
                           previousMouse;
        private Vector2 leftButton, midButton, rightButton;
        private Tower currentTower;
        private int stageWidth = Constants.StageWidth,
                    stageHeight = Constants.StageHeight;
       // private LogicController _logicController;
        private Rectangle startGameButton = new Rectangle(250, 200, 300, 50),
                          chatGameButton = new Rectangle(250, 300, 300, 50),
                          endGameButton = new Rectangle(250, 400, 300, 50);

        public Mouse()
        {
            Console.WriteLine("Mouse instantiated");

            init();
        }

        private void init()
        {
            int yPos = stageHeight,
                xPos = (int)((float)stageWidth * 0.33);
            leftButton = new Vector2((xPos / 2), yPos);
            midButton = new Vector2(xPos + (xPos / 2), yPos);
            rightButton = new Vector2((xPos * 2) + (xPos / 2), yPos);
        }

        public void mouseHandler()
        {
            currentMouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

            if (currentMouse.LeftButton == ButtonState.Pressed &&
                previousMouse.LeftButton == ButtonState.Released)
            {
                mouseLeftButton();
            }
            else if (currentMouse.RightButton == ButtonState.Pressed &&
                    previousMouse.RightButton == ButtonState.Released)
            {
                mouseRightButton();
            }

            previousMouse = currentMouse;
        }

        public void DrawMouse(SpriteBatch batch)
        {
            currentMouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

            if (this.Texture == null)
            {
                this.Texture = Presentation.PresentationController.MouseTexture2D;
            }

            this.Move(currentMouse.X, currentMouse.Y);

            Rectangle mouseRect = new Rectangle((int)this.Position.X, (int)this.Position.Y, Constants.MouseWidth, Constants.MouseHeight);
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(this.Texture, mouseRect, Color.Black);
            batch.End();

            updateCurrentTower();
        }

        private void updateCurrentTower()
        {
            Tower tower = this.CurrentTower;
            if (tower != null)
            {
                tower.Position = this.Position;
            }
        }

        private void mouseLeftButton()
        {
            if (GameStateHandler.CurrentGameState == GameState.MENU)
            {
                Rectangle mouseBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, 1, 1);

                if (mouseBox.Intersects(startGameButton))
                {
                    Console.WriteLine("Start game pressed!");
                    GameStateHandler.CurrentGameState = GameState.PLAY;
                }
                else if (mouseBox.Intersects(chatGameButton))
                {
                    Console.WriteLine("Chat pressed!");
                    // Start chat
                }
                else if (mouseBox.Intersects(endGameButton))
                {
                    Console.WriteLine("End game pressed!");
                    Player.EndGame();
                } 
            }
            else if (GameStateHandler.CurrentGameState == GameState.PLAY)
            {
                Console.WriteLine("Left mouse button clicked");
                int sensitivity = Constants.TowerButtonSensitivity;

                if (Tower.PlacingTower && this.CurrentTower != null)
                {
                    float yPos = stageHeight - ((float)(Presentation.PresentationController.MeleeTowerButtonTexture2D.Height) * 1.5f);
                    if (this.Position.Y < yPos && this.Position.Y > 60)
                    {
                        if (Tower.BuildTower(this.CurrentTower, this.Position))
                        { // if tower was successfully built
                            this.CurrentTower = null;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Cannot place towers on button area or top area");
                    }
                }
                else
                {
                    Tower tower = Tower.GetTowerAtPosition(this.Position);
                    if (tower != null)
                    {
                        tower.UpgradeTower();
                    }

                    if (Sprite.GetIsInRange(this.Position, leftButton, sensitivity))
                    {
                        // left button clicked
                        Console.WriteLine("Ranged Tower button clicked");
                        this.CurrentTower = Tower.BuyTower(TowerType.RangedTower);
                    }

                    else if (Sprite.GetIsInRange(this.Position, midButton, sensitivity))
                    {
                        // mid button clicked
                        Console.WriteLine("Melee Tower button clicked");
                        this.CurrentTower = Tower.BuyTower(TowerType.MeleeTower);
                    }

                    else if (Sprite.GetIsInRange(this.Position, rightButton, sensitivity))
                    {
                        // right button clicked
                        Console.WriteLine("Slow Tower button clicked");
                        this.CurrentTower = Tower.BuyTower(TowerType.SlowTower);
                    }

                    else if (Sprite.GetIsInRange(this.Position, new Vector2(40, 40), 30))
                    {
                        // exit button clicked
                        Console.WriteLine("Exit button clicked");
                        Player.GameEnd = true;
                    }
                }
                Console.WriteLine("Mouse : " + this.Position.ToString());
            }
        }

        private void mouseRightButton()
        {
            // Right mouse button clicked
            Console.WriteLine("Right mouse button clicked");

            if (this.CurrentTower != null)
            {
                Tower.TowersList.RemoveAt(Tower.TowersList.IndexOf(this.CurrentTower));
                Tower.PlacingTower = false;
   
                this.CurrentTower = null;

                Console.WriteLine("Cancel tower selection");
            }
            else
            {
                Tower tower = Tower.GetTowerAtPosition(this.Position);
                if (tower != null)
                {
                    tower.SellTower();
                }
            }
        }

        public Tower CurrentTower
        {
            set { currentTower = value; }
            get { return currentTower; }
        }

    }
}
