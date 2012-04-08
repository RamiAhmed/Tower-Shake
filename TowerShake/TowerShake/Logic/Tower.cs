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

    class Tower : Sprite
    {
        // Public variables
        public static int towerWidth = 15;
        public static List<Tower> towers = new List<Tower>();
        public static Boolean placingTower = false;

        // Protected variables

        // Private variables
        private LogicController _logic;
        private Critter _critterClass;
        private List<Bullet> bullets = new List<Bullet>();
        private int _cost,
                    _range,
                    _damage;
        private long  _lastAttack;
        private float _accuracy,
                      _reloadSpeed;
        private TowerState _towerState;
        private AttackState _attackState;
        private TowerType _towerType;

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
                        if (isAnyTowerInRange(tower.Position) || Presentation.Background.isOnWalkPath(tower) ||
                            Presentation.Background.isOnCity(tower)) 
                        {
                            color = Color.Red;
                        }
                    }

                    batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    batch.Draw(tower.Texture, tower.Position, color);
                    batch.End();

                    if (tower.TowerState == TowerState.Bought)
                    {
                        if (Critter.critters.Count > 0)
                        {
                            foreach (Critter critter in Critter.critters)
                            {
                                shoot(tower, critter);
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

                for (int i = 0; i < bulletsLength; i++)
                {
                    Bullet bullet = bullets.ElementAt(i);
                    if (bullet.Done)
                    {
                        bullets.Remove(bullet);

                        bulletsLength--;
                        i--;
                    }
                    else
                    {
                        bullet.Update(delta, stageWidth, stageHeight);

                        batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                        batch.Draw(bullet.Texture, bullet.Position, Color.White);                            
                        batch.End();
                    }

                }
            }
        }

        private Critter shoot(Tower tower, Critter critter)
        {
            if (Sprite.GetIsInRange(tower.Position, critter.Position, tower.Range))
            {
                if (tower.AttackState == AttackState.Shooting)
                {
                    float bulletSpeed = 5.0f + (float)Sprite.GetRandom(0.0, 2.0);
                    bool hitCritter = false;
                    if (tower.Accuracy + Sprite.GetRandom() >= critter.Dexterity + Sprite.GetRandom())
                    {
                        hitCritter = true;
                    }

                    tower.AttackState = AttackState.Reloading;
                    tower.LastAttack = getCurrentMilliseconds();
                    createBullet(bulletSpeed, tower, critter, hitCritter);
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

        private Bullet createBullet(float speed, Tower tower, Critter critter, Boolean hit)
        {
            Bullet bullet = new Bullet();
            bullet.Speed = speed;

            bullet.Position = new Vector2(tower.Position.X + (tower.Texture.Width / 2), 
                                          tower.Position.Y + (tower.Texture.Height / 2));

            bullet.Direction = critter.Position - tower.Position;
            bullet.Direction.Normalize();

            Texture2D texture = null;
            switch (tower.Type)
            {
                case TowerType.RangedTower: texture = Presentation.PresentationController.black_bullet; 
                                            break;
                case TowerType.MeleeTower: texture = Presentation.PresentationController.black_bullet; 
                                           break;
                case TowerType.SlowTower: texture = Presentation.PresentationController.black_bullet; 
                                          bullet.Slow = true; 
                                          break;
            }
            bullet.Texture = texture;

            bullet.Target = critter;
            bullet.Damage = tower.Damage;
            bullet.Hit = hit;

            bullet.Done = false;
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
            Console.WriteLine("Buy Tower called");
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

                int gold = Player.Gold,
                    cost = tower.Cost;

                if (gold >= cost)
                {
                    towers.Add(tower);
                    tower.TowerState = TowerState.Placing;
                    Tower.placingTower = true;

                    //Console.WriteLine("Player buying tower of type: " + type.ToString());
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

        // returns true when tower was built successfully
        public static bool build(Tower tower, Vector2 position)
        {
            return build(tower, (int)position.X, (int)position.Y);
        }

        // returns true when tower was built successfully
        public static bool build(Tower tower, int x, int y)
        {
            bool succesBuild = false;
            if (!Presentation.Background.isOnCity(tower))
            {
                if (!Presentation.Background.isOnWalkPath(tower))
                {
                    if (!isAnyTowerInRange(x, y))
                    {
                        int cost = tower.Cost,
                            gold = Player.Gold;
                        if (gold >= cost)
                        {
                            tower.Move(x, y);
                            tower.TowerState = TowerState.Bought;
                            tower.AttackState = AttackState.Shooting;
                            Player.Gold -= cost;
                            Tower.placingTower = false;
                            succesBuild = true;

                            Console.WriteLine("Building tower");
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
                else
                {
                    Console.WriteLine("Error: Cannot place towers on path");
                }
            }
            else
            {
                Console.WriteLine("Error: Tower cannot be placed on city");
            }

            return succesBuild;
        }

        public void sell()
        {
            int gold = this.Cost / 2;
            Player.Gold += gold;

            towers.RemoveAt(towers.IndexOf(this));

            Console.WriteLine("Sold tower");
        }

        public void upgrade()
        {
            int cost = this.Cost / 3;
            if (Player.Gold >= cost)
            {
                Player.Gold -= cost;
                this.Damage += (this.Damage / 10);
                this.Range += (this.Range / 10);
                this.Accuracy += (this.Accuracy * 1.1f);
                this.Cost += (this.Cost / 20);
                this.ReloadSpeed += (this.ReloadSpeed / 10);

                Console.WriteLine("Tower upgraded");
            }
            else
            {
                Console.WriteLine("Error: Player does not have enough gold to upgrade, cost: " + cost.ToString());
            }
        }

        public static bool isAnyTowerInRange(Vector2 position)
        {
            return isAnyTowerInRange((int)position.X, (int)position.Y);
        }

        public static bool isAnyTowerInRange(int x, int y)
        {
            bool inRange = false;
            float xDiff, yDiff;
            int towerWidth, towerHeight;

            if (towers.Count == 0)
            {
                return inRange;
            }

            foreach (Tower tower in towers)
            {
                if (tower.TowerState == TowerState.Bought)
                {
                    xDiff = Math.Abs(tower.Position.X - x);
                    yDiff = Math.Abs(tower.Position.Y - y);
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

        public static Tower getTowerAtPosition(Vector2 position)
        {
            foreach (Tower tower in towers) 
            {
                int area = (tower.Texture.Width + tower.Texture.Height) / 2;
                if (Sprite.GetIsInRange(tower.Position, position, area) &&
                    tower.TowerState == TowerState.Bought)
                {
                    return tower;
                }
            }

            return null;
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
