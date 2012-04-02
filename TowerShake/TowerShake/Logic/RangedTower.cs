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
            this.ReloadSpeed = 1.0f;
            this.Cost = 25;
            this.Damage = 10;
            this.Range = 100;
            this.Texture = Presentation.PresentationController.ranged_tower;
        }
    }
}
