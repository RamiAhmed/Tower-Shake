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
    class MediumLevel : Critter
    {
        public MediumLevel() :
                base()
        {
            this.CritterColor = Color.Yellow;
            this.SlowColor = Color.GhostWhite;
            this.HP = Constants.MediumLevelHP;
            this.Speed = Constants.MediumLevelSpeed;
            this.Dexterity = Constants.MediumLevelDexterity;
            this.Points = Constants.MediumLevelPoints;
        }
    }
}
