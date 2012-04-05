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
        public static List<Tower> towers = new List<Tower>();
        public static Boolean placingTower = false;

        // Protected variables

        // Private variables
        private LogicController _logic;
        private Critter _critterClass;
        //private ArrayList bullets = new ArrayList();
        private List<Bullet> bullets = new List<Bullet>();
        private int _cost,
                    _range,
                    _damage,
                    _x, _y;
        private long  _lastAttack;
        private float _accuracy,
                      _reloadSpeed;
        private TowerState _towerState;
        private AttackState _attackState;
        private Texture2D _towerTexture;
        private TowerType _towerType;
        private Random random;

        private int stageWidth = Presentation.PresentationController.STAGE_WIDTH;
        private int stageHeight = Presentation.PresentationController.STAGE_HEIGHT;

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
            random = new Random();
        }

        public void updateTowers(SpriteBatch batch, GameTime gameTime)
        {
            updateAllTowers(batch);
            updateBullets(batch, gameTime);
        }

        private void updateAllTowers(SpriteBatch batch)
        {
            if (towers.Count > 0)
            {
                foreach (Tower tower in towers)
                {
                    Color color = Color.White;
                    if (tower.TowerState == TowerState.Placing)
                    {
                        color = Color.Green;
                        if (isAnyTowerInRange(tower.X, tower.Y))
                        {
                            color = Color.Red;
                        }
                    }

                    batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    batch.Draw(tower.Texture, new Vector2(tower.X, tower.Y), color);
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

        private void updateBullets(SpriteBatch batch, GameTime gameTime)
        {
            if (bullets.Count > 0)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                int bulletsLength = bullets.Count;
                //foreach (Bullet bullet in bullets)
                for (int i = 0; i < bulletsLength; i++)
                {
                    Bullet bullet = bullets.ElementAt(i);
                    if (bullet.done)
                    {
                        bullets.Remove(bullet);

                        bulletsLength--;
                        i--;
                    }
                    else
                    {
                        //if (bullet.texture != null)
                        //{
                            bullet.Update(delta, stageWidth, stageHeight);

                            //batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                            batch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                            batch.Draw(bullet.texture, bullet.position, Color.White);
                            batch.End();
                       // }
                    }

                }
            }
        }

        private Critter shoot(Tower tower, Critter critter)
        {
            if (isEnemyInRange(tower, critter))
            {
                if (tower.AttackState == AttackState.Shooting)
                {
                    if (tower.Accuracy + getRandomDouble() >= critter.Dexterity + getRandomDouble(0.0, 0.5))
                    {
                        Console.WriteLine(tower.ToString() + " shooting at " + critter.ToString() + " with dmg: " + tower.Damage.ToString());
                        _critterClass.damageCritter(critter, tower.Damage);
                        tower.AttackState = AttackState.Reloading;
                        tower.LastAttack = getCurrentMilliseconds();

                        createBullet(5.0f, tower, critter);
                    }
                    else
                    {
                        Console.WriteLine("Tower " + tower.ToString() + " missed critter " + critter.ToString());
                    }
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
            return critter;
        }

        private double getRandomDouble(double min, double max)
        {
            return min + random.NextDouble() * (max - min);
        }

        private double getRandomDouble()
        {
            return random.NextDouble();
        }

        private Bullet createBullet(float speed, Tower tower, Critter critter)
        {
            Bullet bullet = new Bullet();
            bullet.speed = speed;

            bullet.position = new Vector2(tower.X + (tower.Texture.Width / 2), tower.Y + (tower.Texture.Height / 2));

            bullet.direction = new Vector2(critter.X, critter.Y) - new Vector2(tower.X, tower.Y);
            bullet.direction.Normalize();

            /*Texture2D texture = null;
            switch (tower.Type)
            {
                case TowerType.RangedTower: texture = Presentation.PresentationController.black_bullet; break;
                case TowerType.MeleeTower: texture = Presentation.PresentationController.black_bullet; break;
                case TowerType.SlowTower: texture = Presentation.PresentationController.black_bullet; break;
            }
            bullet.texture = texture;*/
            bullet.texture = Presentation.PresentationController.black_bullet;

            bullet.done = false;
            bullets.Add(bullet);
            return bullet;
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
                    case TowerType.MeleeTower:
                                            tower = new MeleeTower();
                                            tower.Type = TowerType.MeleeTower;
                                            break;
                    case TowerType.RangedTower:
                                            tower = new RangedTower();
                                            tower.Type = TowerType.RangedTower;
                                            break;
                    case TowerType.SlowTower:
                                            tower = new SlowTower();
                                            tower.Type = TowerType.SlowTower;
                                            break;
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

        protected TowerType Type
        {
            set { _towerType = value; }
            get { return _towerType; }
        }

        protected long LastAttack
        {
            set { _lastAttack = value; }
            get { return _lastAttack; }
        }

    }
}
