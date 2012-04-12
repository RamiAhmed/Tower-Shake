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

        // Private static variables        
        private static int gold = Constants.StartGold,
                           lives = Constants.StartLives;
        private static Boolean gameEnded = false;

        // private variables
        private LogicController _logicClass;
        private static int specialAbilityCost = Constants.SpecialAbilityCost;

        public Player(LogicController parentClass)
        {
            _logicClass = parentClass;

            init();

            Console.WriteLine("Player instantiated");
        }

        private void init()
        {

        }

        public static void endGame()
        {
            if (!Player.GameEnd)
            {
                Console.WriteLine("Shutting game down");
                GameStateHandler.CurrentGameState = GameState.END;
                Player.GameEnd = true;
            }
        }

        public static void specialAbility(PlayerAbility ability)
        {
            if (ability != PlayerAbility.NULL)
            {
                if (Player.Gold >= specialAbilityCost)
                {
                    Player.Gold -= specialAbilityCost;

                    switch (ability)
                    {
                        case PlayerAbility.PARALYZE: paralyze(); break;
                        case PlayerAbility.KILL: kill(); break;
                        case PlayerAbility.SLOW: slow(); break;
                        case PlayerAbility.BOOST: boost(); break;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Player cannot afford special ability");
                }
            }
        }

        private static void paralyze()
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

        private static void kill()
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

        private static void slow()
        {
            Console.WriteLine("Special Ability: All critters slowed");
            if (Critter.critters.Count > 0)
            {
                foreach (Critter critter in Critter.critters)
                {
                    critter.Slowed = LogicController.getCurrentSeconds();
                    critter.SlowDamage = Constants.AbilitySlowAmount;
                    critter.damageCritter(Constants.AbilitySlowDamage);
                }
            }
        }

        private static void boost()
        {
            Console.WriteLine("Special Ability: All towers boosted");
            if (Tower.towers.Count > 0)
            {
                foreach (Tower tower in Tower.towers)
                {
                    if (tower.Boosted == 0 && tower.TowerState == TowerState.Bought)
                    {
                        tower.Boosted = LogicController.getCurrentSeconds();

                        float boostAmount = Constants.AbilityTowerBoost;
                        tower.Accuracy *= boostAmount;
                        tower.Damage *= (int)boostAmount;
                        tower.ReloadSpeed /= boostAmount;
                        tower.Accuracy *= boostAmount;
                        tower.Range *= (int)boostAmount;
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

        public static Boolean GameEnd
        {
            get { return gameEnded; }
            set { gameEnded = value; }
        }

    }
}
