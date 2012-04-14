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
            this.Accuracy = Constants.SlowTowerAccuracy;
            this.ReloadSpeed = Constants.SlowTowerSpeed;
            this.Cost = Constants.SlowTowerCost;
            this.Damage = Constants.SlowTowerDamage;
            this.Range = Constants.SlowTowerRange;
            this.Texture = Presentation.PresentationController.SlowTowerTexture2D;
        }
    }
}
