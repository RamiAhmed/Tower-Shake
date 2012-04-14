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
        private static int specialAbilityCost = Constants.SpecialAbilityCost;

        // private variables
        private LogicController logicClass;

        public Player(LogicController parentClass)
        {
            logicClass = parentClass;

            init();

            Console.WriteLine("Player instantiated");
        }

        private void init()
        {

        }

        public static void EndGame()
        {
            if (!Player.GameEnd)
            {
                Console.WriteLine("Shutting game down");
                GameStateHandler.CurrentGameState = GameState.END;
                Player.GameEnd = true;
            }
        }

        public static void SpecialAbility(PlayerAbility ability)
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
            if (Critter.CrittersList.Count > 0)
            {
                foreach (Critter critter in Critter.CrittersList)
                {
                    critter.Slowed = LogicController.getCurrentSeconds();
                    critter.SlowDamage = 1f;
                }
            }
        }

        private static void kill()
        {
            Console.WriteLine("Special Ability: All critters killed");
            if (Critter.CrittersList.Count > 0)
            {
                foreach (Critter critter in Critter.CrittersList)
                {
                    critter.Die();
                }
            }
        }

        private static void slow()
        {
            Console.WriteLine("Special Ability: All critters slowed");
            if (Critter.CrittersList.Count > 0)
            {
                foreach (Critter critter in Critter.CrittersList)
                {
                    critter.Slowed = LogicController.getCurrentSeconds();
                    critter.SlowDamage = Constants.AbilitySlowAmount;
                    critter.DamageCritter(Constants.AbilitySlowDamage);
                }
            }
        }

        private static void boost()
        {
            Console.WriteLine("Special Ability: All towers boosted");
            if (Tower.TowersList.Count > 0)
            {
                foreach (Tower tower in Tower.TowersList)
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
