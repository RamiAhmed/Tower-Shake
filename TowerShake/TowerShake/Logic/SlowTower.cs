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
            this.Accuracy = 0.25f;
            this.AttackSpeed = 1;
            this.Cost = 30;
            this.Damage = 5;
            this.Range = 25;
        }
    }
}
