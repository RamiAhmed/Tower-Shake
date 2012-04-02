using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerShake.Logic
{
    class MeleeTower : Tower
    {
        public MeleeTower()
        {
            this.Accuracy = 0.75f;
            this.ReloadSpeed = 0.1f;
            this.Cost = 20;
            this.Damage = 15;
            this.Range = 25;
            this.Texture = Presentation.PresentationController.melee_tower;
        }
    }
}
