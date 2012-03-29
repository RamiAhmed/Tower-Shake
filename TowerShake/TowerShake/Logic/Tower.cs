using System;
using System.Collections.Generic;
using System.Collections;
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
    enum TowerType { RangedTower, MeleeTower, SlowTower };

    class Tower
    {
        // Public variables
        public static int towerWidth = 15;
        public static ArrayList towers = new ArrayList();

        // Protected variables

        // Private variables
        private LogicController _logic;
        private int _cost,
                    _range,
                    _damage,
                    _attack_speed,
                    _x, _y;
        private float _accuracy;
        private Texture2D _towerTexture;

        public Tower(LogicController parentClass)
        {
            _logic = parentClass;

            Console.WriteLine("Tower class instantiated!");
            init();
        }

        public Tower()
        {

        }

        private void init()
        {

        }

        public void updateTowers(SpriteBatch batch)
        {
            if (towers.Count > 0)
            {
                foreach (Tower tower in towers)
                {
                    batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                    batch.Draw(tower.Texture, new Vector2(tower.X, tower.Y), Color.White);
                    batch.End();
                }
            }

        }

        protected virtual void shoot(Critter critter)
        {
        }

        public static Tower buy(TowerType type, int x, int y)
        {
            Console.WriteLine("buy (Tower) called");
            Tower tower = null;
            if (!isAnyTowerInRange(x, y))
            {
                switch (type)
                {
                    case TowerType.MeleeTower: tower = new MeleeTower(); break;
                    case TowerType.RangedTower: tower = new RangedTower(); break;
                    case TowerType.SlowTower: tower = new SlowTower(); break;
                }

                int gold = Player.gold,
                    cost = tower.Cost;

                if (gold >= cost)
                {
                    Player.gold -= cost;
                    towers.Add(tower);
                    tower.X = x;
                    tower.Y = y;

                    Console.WriteLine("Player buying tower of type: " + type.ToString());
                }
                else
                {
                    Console.WriteLine("Error: Player cannot afford to buy this tower");
                    tower = null;
                }
            }
            else
            {
                Console.WriteLine("Error: Another tower is too close");
            }

            return tower;
        }

        private void sell(Tower tower)
        {
            int gold = tower.Cost / 2;
            Player.gold += gold;

            towers.Remove(tower);
            tower = null;
        }

        private void update(Tower tower)
        {
            int cost = tower.Cost / 3;
            if (Player.gold >= cost)
            {
                Player.gold -= cost;
                tower.Damage += (tower.Damage / 10);
                tower.Range += (tower.Range / 10);
                tower.Accuracy += (tower.Accuracy * 1.05f);
                tower.Cost += (tower.Cost / 20);
                tower.AttackSpeed += (tower.AttackSpeed / 10);
            }
            else
            {
                Console.WriteLine("Error: Player does not have enough gold to upgrade, cost: " + cost.ToString());
            }
        }

        protected bool isEnemyInRange(Tower tower, Critter critter)
        {
            bool inRange = false;
            int xDiff = (int)tower.X - critter.X;
            int yDiff = (int)tower.Y - critter.Y;
            int range = tower.Range;

            if (Math.Abs(xDiff) < range && Math.Abs(yDiff) < range)
            {
                inRange = true;
            }

            return inRange;
        }

        public static bool isAnyTowerInRange(int x, int y)
        {
            bool inRange = false;
          
            foreach (Tower tower in towers)
            {
                int xDiff = tower.X - x,
                    yDiff = tower.Y - y;
                if (xDiff < towerWidth && yDiff < towerWidth)
                {
                    inRange = true;
                    break;
                }
            }

            return inRange;
        }

        protected int X
        {
            set { _x = value; }
            get { return _x; }
        }

        protected int Y
        {
            set { _y = value; }
            get { return _y; }
        }

        protected int Cost
        {
            set { _cost = value; }
            get { return _cost; }
        }

        protected int Range
        {
            set { _range = value; }
            get { return _range; }
        }

        protected int Damage
        {
            set { _damage = value; }
            get { return _damage; }
        }

        protected float Accuracy
        {
            set { _accuracy = value; }
            get { return _accuracy; }
        }

        protected int AttackSpeed
        {
            set { _attack_speed = value; }
            get { return _attack_speed; }
        }

        protected Texture2D Texture
        {
            set { _towerTexture = value; }
            get { return _towerTexture; }
        }

    }
}
