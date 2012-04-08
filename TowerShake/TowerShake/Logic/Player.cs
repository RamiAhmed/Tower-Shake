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
    enum PlayerAbility { PARALYZE, KILL, SLOW, BOOST, NULL };

    class Player
    {
        // Public variables


        // Private variables
        private LogicController _logicClass;
        private static int gold = 50,
                           points = 0,
                           lives = 10;
        private static Boolean gameEnded = false;
        private static PlayerAbility playerAbility;
        KeyboardState currentKey, previousKey;

        public Player(LogicController parentClass)
        {
            _logicClass = parentClass;

            init();

            Console.WriteLine("Player instantiated");
        }

        private void init()
        {

        }

        public void keyboardHandler()
        {
            currentKey = Keyboard.GetState();

            if (currentKey.IsKeyDown(Keys.Space) && previousKey.IsKeyUp(Keys.Space))
            {
                if (currentKey.IsKeyDown(Keys.P))
                {
                    playerAbility = PlayerAbility.PARALYZE;
                }
                else if (currentKey.IsKeyDown(Keys.S))
                {
                    playerAbility = PlayerAbility.SLOW;
                }
                else if (currentKey.IsKeyDown(Keys.K))
                {
                    playerAbility = PlayerAbility.KILL;
                }
                else if (currentKey.IsKeyDown(Keys.B))
                {
                    playerAbility = PlayerAbility.BOOST;
                }
                else
                {
                    playerAbility = PlayerAbility.NULL;
                }

                specialAbility(playerAbility);
            }
            previousKey = currentKey;
        }

        private void specialAbility(PlayerAbility ability)
        {
            if (ability != PlayerAbility.NULL)
            {
                switch (ability)
                {
                    case PlayerAbility.PARALYZE: paralyze(); break;
                    case PlayerAbility.KILL: kill(); break;
                    case PlayerAbility.SLOW: slow(); break;
                    case PlayerAbility.BOOST: boost();  break;
                }
            }
        }

        private void paralyze()
        {
            foreach (Critter critter in Critter.critters)
            {

            }
        }

        private void kill()
        {

        }

        private void slow()
        {

        }

        private void boost()
        {

        }

        public static int Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        public static int Lives
        {
            get { return lives; }

            set 
            { 
                lives = value;
                if (lives <= 0)
                {
                    GameEnd = true;
                }
            }
        }

        public static int Points
        {
            get { return points; }
            set { points = value; }
        }

        public static Boolean GameEnd
        {
            get { return gameEnded; }
            set { gameEnded = value; }
        }

    }
}
