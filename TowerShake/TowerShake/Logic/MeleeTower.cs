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
            this.Accuracy = 0.5f;
            this.ReloadSpeed = 0.5f;
            this.Cost = 25;
            this.Damage = 5;
            this.Range = 50;
            this.Texture = Presentation.PresentationController.melee_tower;
        }
    }
}
