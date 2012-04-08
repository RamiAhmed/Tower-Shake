using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerShake.Logic
{
    class SlowTower : Tower
    {
        public SlowTower()
        {
            this.Accuracy = 1f;
            this.ReloadSpeed = 0.5f;
            this.Cost = 30;
            this.Damage = 5;
            this.Range = 150;
            this.Texture = Presentation.PresentationController.slow_tower;
        }
    }
}
