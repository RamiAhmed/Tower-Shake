using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerShake.Logic
{
    class RangedTower : Tower
    {
        public RangedTower()
        {
            this.Accuracy = 0.5f;
            this.ReloadSpeed = 1.5f;
            this.Cost = 25;
            this.Damage = 15;
            this.Range = 80;
            this.Texture = Presentation.PresentationController.ranged_tower;
        }
    }
}
