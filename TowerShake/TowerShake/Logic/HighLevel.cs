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
    class HighLevel : Critter
    {
        public HighLevel() :
               base()
        {
            this.CritterColor = Color.Red;
            this.SlowColor = Color.GhostWhite;
            this.HP = Constants.HighLevelHP;
            this.Speed = Constants.HighLevelSpeed;
            this.Dexterity = Constants.HighLevelDexterity;
            this.Points = Constants.HighLevelPoints;
        }
    }
}
