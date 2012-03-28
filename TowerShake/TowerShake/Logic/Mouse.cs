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
    class Mouse 
    {
        // Public variables

        // Private variables
        private MouseState currentMouse,
                           previousMouse;
        private int _x, _y;
        private Vector2 leftButton, midButton, rightButton;
       // private LogicController _logicController;

        public Mouse()
        {
           //_logicController = parentClass;

            Console.WriteLine("Mouse instantiated");
        }

        private void init()
        {
            leftButton = new Vector2(157, 477);
            midButton = new Vector2(410, 477);
            rightButton = new Vector2(670, 477);
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
            this.X = currentMouse.X;
            this.Y = currentMouse.Y;

            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(Presentation.PresentationController.mouse, new Rectangle(this.X, this.Y, 35, 20), Color.White);
            batch.End();
        }

        private void mouseLeftButton()
        {
            int mouseX = currentMouse.X,
                mouseY = currentMouse.Y;
            Vector2 mouseVector = new Vector2(mouseX, mouseY);

            if (isApproximatelyEqual(mouseVector, leftButton))
            {
                // left button clicked
                Tower.buy(TowerType.MeleeTower, mouseX, mouseY);
            }
            
            else if (isApproximatelyEqual(mouseVector, midButton))
            {
                // mid button clicked
                Tower.buy(TowerType.RangedTower, mouseX, mouseY);
            }

            else if (isApproximatelyEqual(mouseVector, rightButton))
            {
                // right button clicked
                Tower.buy(TowerType.SlowTower, mouseX, mouseY);
            }

            Console.WriteLine("MouseX : " + mouseX.ToString() + ", MouseY: " + mouseY.ToString());
        }

        private void mouseRightButton()
        {
            // Right mouse button clicked
        }

        private bool isApproximatelyEqual(Vector2 one, Vector2 two)
        {
            bool equal = false;

            int xDiff = (int)two.X - (int)one.X,
                yDiff = (int)two.Y - (int)one.Y;

            if (Math.Abs(xDiff) < 50 && Math.Abs(yDiff) < 50)
            {
                equal = true;
            }

            return equal;
        }

        private bool isApproximatelyEqual(Vector2 one, Vector2 two, int sensitivity)
        {
            bool equal = false;

            int xDiff = (int)two.X - (int)one.X,
                yDiff = (int)two.Y - (int)one.Y;

            if (Math.Abs(xDiff) < sensitivity && Math.Abs(yDiff) < sensitivity)
            {
                equal = true;
            }

            return equal;
        }

        private int X
        {
            set {_x = value;}
            get { return _x; }
        }

        private int Y
        {
            set { _y = value; }
            get { return _y; }
        }

    }
}
