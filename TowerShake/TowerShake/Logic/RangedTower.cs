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
            this.Accuracy = Constants.RangedTowerAccuracy;
            this.ReloadSpeed = Constants.RangedTowerSpeed;
            this.Cost = Constants.RangedTowerCost;
            this.Damage = Constants.RangedTowerDamage;
            this.Range = Constants.RangedTowerRange;
            this.Texture = Presentation.PresentationController.RangedTowerTexture2D;
        }
    }
}
