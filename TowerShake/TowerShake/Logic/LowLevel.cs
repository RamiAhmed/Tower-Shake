﻿using System;
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
    class LowLevel : Critter
    {
        public LowLevel() :
               base()
        {
            Console.WriteLine("Creating low level critter");
            this.CritterColor = Color.Green;
            this.SlowColor = Color.GhostWhite;
            this.HP = Constants.LowLevelHP;
            this.Speed = Constants.LowLevelSpeed;
            this.Dexterity = Constants.LowLevelDexterity;
            this.Points = Constants.LowLevelPoints;
        }
    }
}
