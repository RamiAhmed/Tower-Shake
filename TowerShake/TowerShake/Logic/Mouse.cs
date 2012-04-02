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
        private Tower _currentTower;
       // private LogicController _logicController;

        public Mouse()
        {
           //_logicController = parentClass;

            Console.WriteLine("Mouse instantiated");

            init();
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

            updateTower();
        }

        private void updateTower()
        {
            Tower _tower = this.CurrentTower;
            if (_tower != null)
            {
                _tower.X = this.X;
                _tower.Y = this.Y;
            }
        }

        private void mouseLeftButton()
        {
            Console.WriteLine("Left mouse button clicked");
            int sensitivity = 100;
            Vector2 mouseVector = new Vector2(this.X, this.Y);

            if (Tower.placingTower && this.CurrentTower != null)
            {
                Tower.build(this.CurrentTower, this.X, this.Y);
                this.CurrentTower = null;
            }

            if (isApproximatelyEqual(mouseVector, leftButton, sensitivity))
            {
                // left button clicked
                Console.WriteLine("Ranged Tower button clicked");
                this.CurrentTower = Tower.buy(TowerType.RangedTower);
            }

            else if (isApproximatelyEqual(mouseVector, midButton, sensitivity))
            {
                // mid button clicked
                Console.WriteLine("Melee Tower button clicked");
                this.CurrentTower = Tower.buy(TowerType.MeleeTower);
            }

            else if (isApproximatelyEqual(mouseVector, rightButton, sensitivity))
            {
                // right button clicked
                Console.WriteLine("Slow Tower button clicked"); 
                this.CurrentTower = Tower.buy(TowerType.SlowTower);
            }

            Console.WriteLine("MouseX : " + this.X.ToString() + ", MouseY: " + this.Y.ToString());
        }

        private void mouseRightButton()
        {
            // Right mouse button clicked
            Console.WriteLine("Right mouse button clicked");
        }

        private bool isApproximatelyEqual(Vector2 one, Vector2 two, int sensitivity)
        {
            bool equal = false;

           // Console.WriteLine("xDiff (|" + xDiff.ToString() + "|) < " + sensitivity.ToString() +
           //              " && yDiff (|" + yDiff.ToString() + "|) < " + sensitivity.ToString());
            //Console.WriteLine(" X1:" + one.X.ToString() + " - X2:" + two.X.ToString() + " = " + (one.X - two.X).ToString());
            if (Math.Abs(one.X - two.X) < sensitivity)
            {
                if (Math.Abs(one.Y - two.Y) < sensitivity)
                {
                    equal = true;
                }

            }
            //Console.WriteLine("equal = " + equal.ToString());
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

        public Tower CurrentTower
        {
            set { _currentTower = value; }
            get { return _currentTower; }
        }

    }
}
