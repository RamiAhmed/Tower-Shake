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
    public enum GameState { MENU, PAUSE, PLAY, END };

    public static class GameStateHandler
    {
        private static GameState _currentState;

        public static GameState CurrentGameState 
        {
            get { return _currentState; }
            set { _currentState = value; }
        }
    }
}
