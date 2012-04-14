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
            this.Accuracy = Constants.MeleeTowerAccuracy;
            this.ReloadSpeed = Constants.MeleeTowerSpeed;
            this.Cost = Constants.MeleeTowerCost;
            this.Damage = Constants.MeleeTowerDamage;
            this.Range = Constants.MeleeTowerRange;
            this.Texture = Presentation.PresentationController.MeleeTowerTexture2D;
        }
    }
}
