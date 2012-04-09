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
        private Tower _currentTower;
        private int _stageWidth = Presentation.PresentationController.STAGE_WIDTH,
                    _stageHeight = Presentation.PresentationController.STAGE_HEIGHT;
       // private LogicController _logicController;

        public Mouse()
        {
           //_logicController = parentClass;

            Console.WriteLine("Mouse instantiated");

            init();
        }

        private void init()
        {
            int yPos = _stageHeight,
                xPos = (int)((float)_stageWidth * 0.33);
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

        public void drawMouse(SpriteBatch batch)
        {
            if (this.Texture == null)
            {
                this.Texture = Presentation.PresentationController.mouse;
            }

            this.Move(currentMouse.X, currentMouse.Y);

            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(this.Texture, new Rectangle((int)this.Position.X, (int)this.Position.Y, 35, 20), Color.Black);
            batch.End();

            updateCurrentTower();
        }

        private void updateCurrentTower()
        {
            Tower _tower = this.CurrentTower;
            if (_tower != null)
            {
                _tower.Position = this.Position;
            }
        }

        private void mouseLeftButton()
        {
            Console.WriteLine("Left mouse button clicked");
            int sensitivity = 125;

            if (Tower.placingTower && this.CurrentTower != null)
            {
                float yPos = _stageHeight - ((float)(Presentation.PresentationController.melee_tower_button.Height) * 1.5f);
                if (this.Position.Y < yPos)
                {
                    if (Tower.build(this.CurrentTower, this.Position))
                    { // if tower was successfully built
                        this.CurrentTower = null;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Cannot place towers on button area");
                }
            }
            else
            {
                Tower tower = Tower.getTowerAtPosition(this.Position);
                if (tower != null)
                {
                    tower.upgrade();
                }

                if (Sprite.GetIsInRange(this.Position, leftButton, sensitivity))
                {
                    // left button clicked
                    Console.WriteLine("Ranged Tower button clicked");
                    this.CurrentTower = Tower.buy(TowerType.RangedTower);
                }

                else if (Sprite.GetIsInRange(this.Position, midButton, sensitivity))
                {
                    // mid button clicked
                    Console.WriteLine("Melee Tower button clicked");
                    this.CurrentTower = Tower.buy(TowerType.MeleeTower);
                }

                else if (Sprite.GetIsInRange(this.Position, rightButton, sensitivity))
                {
                    // right button clicked
                    Console.WriteLine("Slow Tower button clicked");
                    this.CurrentTower = Tower.buy(TowerType.SlowTower);
                }
            } 
            Console.WriteLine("Mouse : " + this.Position.ToString());
        }

        private void mouseRightButton()
        {
            // Right mouse button clicked
            Console.WriteLine("Right mouse button clicked");

            if (this.CurrentTower != null)
            {
                Tower.towers.RemoveAt(Tower.towers.IndexOf(this.CurrentTower));
                Tower.placingTower = false;
   
                this.CurrentTower = null;

                Console.WriteLine("Cancel tower selection");
            }
            else
            {
                Tower tower = Tower.getTowerAtPosition(this.Position);
                if (tower != null)
                {
                    tower.sell();
                }
            }
        }

        public Tower CurrentTower
        {
            set { _currentTower = value; }
            get { return _currentTower; }
        }

    }
}
