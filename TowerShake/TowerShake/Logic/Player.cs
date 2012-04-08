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
        KeyboardState currentKey, previousKey = Keyboard.GetState();

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

            if (currentKey.IsKeyDown(Keys.P) && previousKey.IsKeyUp(Keys.P))
            {
                playerAbility = PlayerAbility.PARALYZE;
            }
            else if (currentKey.IsKeyDown(Keys.S) && previousKey.IsKeyUp(Keys.S))
            {
                playerAbility = PlayerAbility.SLOW;
            }
            else if (currentKey.IsKeyDown(Keys.K) && previousKey.IsKeyUp(Keys.K))
            {
                playerAbility = PlayerAbility.KILL;
            }
            else if (currentKey.IsKeyDown(Keys.B) && previousKey.IsKeyUp(Keys.B))
            {
                playerAbility = PlayerAbility.BOOST;
            }
            else
            {
                playerAbility = PlayerAbility.NULL;
            }

            if (playerAbility != PlayerAbility.NULL)
            {
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
            Console.WriteLine("Special Ability: All critters paralyzed");
            if (Critter.critters.Count > 0)
            {
                foreach (Critter critter in Critter.critters)
                {
                    critter.Slowed = LogicController.getCurrentSeconds();
                    critter.SlowDamage = 1f;
                }
            }
        }

        private void kill()
        {
            Console.WriteLine("Special Ability: All critters killed");
            if (Critter.critters.Count > 0)
            {
                foreach (Critter critter in Critter.critters)
                {
                    critter.die();
                }
            }
        }

        private void slow()
        {
            Console.WriteLine("Special Ability: All critters slowed");
            if (Critter.critters.Count > 0)
            {
                foreach (Critter critter in Critter.critters)
                {
                    critter.Slowed = LogicController.getCurrentSeconds();
                    critter.SlowDamage = 0.75f;
                    critter.damageCritter(5);
                }
            }
        }

        private void boost()
        {
            Console.WriteLine("Special Ability: All towers boosted");
            if (Tower.towers.Count > 0)
            {
                foreach (Tower tower in Tower.towers)
                {
                    if (tower.Boosted == 0 && tower.TowerState == TowerState.Bought)
                    {
                        tower.Boosted = LogicController.getCurrentSeconds();

                        tower.Accuracy *= 2;
                        tower.Damage *= 2;
                        tower.ReloadSpeed *= 2;
                        tower.Accuracy *= 2;
                        tower.Range *= 2;
                        tower.Width *= 2;
                        tower.Height *= 2;
                    }
                }
            }
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
