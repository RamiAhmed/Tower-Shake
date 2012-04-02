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
    class LowLevel : Critter
    {
        public LowLevel() :
               base()
        {
            Console.WriteLine("Creating low level critter");
            this.CritterColor = Color.Yellow;
            this.HP = 15;
            this.Speed = 1;
            this.Dexterity = 0.2f;
        }
    }
}
