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
    class KeyboardHandler
    {
        private PlayerAbility playerAbility;
        private KeyboardState currentKey, 
                              previousKey = Keyboard.GetState();

        public KeyboardHandler()
        {
        }

        public void keyboardHandler()
        {
            currentKey = Keyboard.GetState();

            if (currentKey.IsKeyDown(Keys.Escape) && previousKey.IsKeyUp(Keys.Escape))
            {
                Player.EndGame();
            }
            else if (currentKey.IsKeyDown(Keys.Pause) && previousKey.IsKeyUp(Keys.Pause))
            {
                if (GameStateHandler.CurrentGameState == GameState.PLAY)
                {
                    Console.WriteLine("Game paused");
                    GameStateHandler.CurrentGameState = GameState.PAUSE;
                }
                else if (GameStateHandler.CurrentGameState == GameState.PAUSE)
                {
                    Console.WriteLine("Game unpaused");
                    GameStateHandler.CurrentGameState = GameState.PLAY;
                }
            }
            else
            {
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

                Player.SpecialAbility(playerAbility);
            }

            previousKey = currentKey;
        }
    }
}
