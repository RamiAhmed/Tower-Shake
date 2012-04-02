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
    enum TowerState { Placing, Bought };
    enum AttackState { Shooting, Reloading };

    class Tower
    {
        // Public variables
        public static int towerWidth = 15;
        public static ArrayList towers = new ArrayList();
        public static Boolean placingTower = false;

        // Protected variables

        // Private variables
        private LogicController _logic;
        Critter _critterClass;
        private int _cost,
                    _range,
                    _damage,
                    _x, _y;
        private long  _lastAttack;
        private TowerState _towerState;
        private AttackState _attackState;
        private float _accuracy,
                      _reloadSpeed;
        private Texture2D _towerTexture;

        public Tower(LogicController parentClass)
        {
            _logic = parentClass;

            Console.WriteLine("Tower class instantiated!");
            init();
        }

        public Tower()
        {
            init();
        }

        private void init()
        {
            _critterClass = new Critter();
        }

        public void updateTowers(SpriteBatch batch)
        {
            if (towers.Count > 0)
            {
                foreach (Tower tower in towers)
                {
                    batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                    if (tower.TowerState == TowerState.Placing)
                    {
                        Color color = Color.Green;
                        if (isAnyTowerInRange(tower.X, tower.Y))
                        {
                            color = Color.Red;
                        }
                        batch.Draw(tower.Texture, new Vector2(tower.X, tower.Y), color);
                    }
                    else
                    {
                        batch.Draw(tower.Texture, new Vector2(tower.X, tower.Y), Color.White);
                    }
                    
                    batch.End();


                    if (tower.TowerState == TowerState.Bought)
                    {
                        if (Critter.critters.Count > 0)
                        {
                            try
                            {
                                foreach (Critter critter in Critter.critters)
                                {
                                    shoot(tower, critter);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error: " + e.ToString());
                            }
                        }
                    }
                }
            }

        }

        private void shoot(Tower tower, Critter critter)
        {
            if (isEnemyInRange(tower, critter))
            {
                if (tower.AttackState == AttackState.Shooting)
                {
                    Console.WriteLine(tower.ToString() + " shooting at " + critter.ToString() + " with dmg: " + tower.Damage.ToString());
                    _critterClass.damageCritter(critter, tower.Damage);
                    tower.AttackState = AttackState.Reloading;
                    tower.LastAttack = getCurrentMilliseconds();
                }
                else
                {
                    if (getCurrentMilliseconds() - tower.LastAttack > (long)(tower.ReloadSpeed * 1000))
                    {
                        Console.WriteLine(tower.ToString() + " finished reloading");
                        tower.AttackState = AttackState.Shooting;
                    }
                }
            }
        }

        private long getCurrentMilliseconds()
        {
            return (LogicController.totalGameTimeMinutes * 60 * 1000) +
                   (LogicController.totalGameTimeSeconds * 1000) +
                   LogicController.timeMilliSeconds;
        }

        public static Tower buy(TowerType type)
        {
            Console.WriteLine("Buy (Tower) called");
            Tower tower = null;

            if (!Tower.placingTower)
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
                    //Player.gold -= cost;
                    towers.Add(tower);
                    tower.TowerState = TowerState.Placing;
                    Tower.placingTower = true;

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
                Console.WriteLine("Error: Already placing tower");
                tower = null;
            }

            return tower;
        }

        public static void build(Tower tower, int x, int y)
        {
            if (!isAnyTowerInRange(x, y))
            {
                int cost = tower.Cost,
                    gold = Player.gold;
                if (gold >= cost)
                {
                    tower.X = x;
                    tower.Y = y;
                    tower.TowerState = TowerState.Bought;
                    tower.AttackState = AttackState.Shooting;
                    Player.gold -= cost;
                    Tower.placingTower = false;
                }
                else
                {
                    Console.WriteLine("Error: Player does not have enough gold");
                }
            }
            else
            {
                Console.WriteLine("Error: Cannot place tower, another tower in range");
            }

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
                tower.ReloadSpeed += (tower.ReloadSpeed / 10);
            }
            else
            {
                Console.WriteLine("Error: Player does not have enough gold to upgrade, cost: " + cost.ToString());
            }
        }

        protected bool isEnemyInRange(Tower tower, Critter critter)
        {
            bool inRange = false;
            int xDiff = Math.Abs(tower.X - critter.X);
            int yDiff = Math.Abs(tower.Y - critter.Y);
            int range = tower.Range;

            if (xDiff < range && yDiff < range)
            {
                inRange = true;
            }

            return inRange;
        }

        public static bool isAnyTowerInRange(int x, int y)
        {
            bool inRange = false;
            int xDiff, yDiff, towerWidth, towerHeight;

            if (towers.Count == 0)
            {
                return inRange;
            }

            foreach (Tower tower in towers)
            {
                if (tower.TowerState == TowerState.Bought)
                {
                    xDiff = Math.Abs(tower.X - x);
                    yDiff = Math.Abs(tower.Y - y);
                    towerWidth = tower.Texture.Width;
                    towerHeight = tower.Texture.Height;
                    if (xDiff + 1 < towerWidth && yDiff + 1 < towerHeight)
                    {
                        inRange = true;
                        break;
                    }
                }
            }

            return inRange;
        }

        public int X
        {
            set { _x = value; }
            get { return _x; }
        }

        public int Y
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

        protected float ReloadSpeed
        {
            set { _reloadSpeed = value; }
            get { return _reloadSpeed; }
        }

        public Texture2D Texture
        {
            set { _towerTexture = value; }
            get { return _towerTexture; }
        }

        public TowerState TowerState
        {
            set { _towerState = value; }
            get { return _towerState; }
        }

        public AttackState AttackState
        {
            set { _attackState = value; }
            get { return _attackState; }
        }

        private long LastAttack
        {
            set { _lastAttack = value; }
            get { return _lastAttack; }
        }

    }
}
