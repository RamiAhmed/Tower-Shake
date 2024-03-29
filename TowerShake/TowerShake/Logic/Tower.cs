﻿using System;
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

        // Private variables
        private static List<Tower> _towersList = new List<Tower>();
        private static Boolean _placingTower = false;

        private int _cost,
                    _range,
                    _damage,
                    _boosted;
        private long  _lastAttack;
        private float _accuracy,
                      _reloadSpeed;
        private TowerState _towerState;
        private AttackState _attackState;
        private TowerType _towerType;

        private LogicController logicClass;
        private Critter critterClass;
        private List<Bullet> bullets = new List<Bullet>();
        private int stageWidth = Constants.StageWidth,
                    stageHeight = Constants.StageHeight;

        public Tower(LogicController parentClass)
        {
            logicClass = parentClass;

            Console.WriteLine("Tower class instantiated!");
            init();
        }

        public Tower()
        {
            init();
        }

        private void init()
        {
            critterClass = new Critter();
        }

        public void UpdateTowers(SpriteBatch batch, GameTime gameTime)
        {
            updateAllTowers(batch);
            updateBullets(batch, gameTime);
        }

        private void updateAllTowers(SpriteBatch batch)
        {
            if (TowersList.Count > 0)
            {
                foreach (Tower tower in TowersList)
                {
                    tower.checkForBoosted();

                    Color color = Color.White;
                    if (tower.TowerState == TowerState.Placing)
                    {
                        float yPos = stageHeight - ((float)(Presentation.PresentationController.MeleeTowerButtonTexture2D.Height) * 1.5f);
                        color = Color.Green;
                        if (IsAnyTowerInRange(tower.Position) || Presentation.Background.IsOnWalkPath(tower) ||
                            Presentation.Background.IsOnCity(tower) || tower.Position.Y > yPos)
                        {
                            color = Color.Red;
                        }
                    }
                    else if (tower.Boosted != 0)
                    {
                        color = Color.Gold;
                    }

                    Rectangle towerRect = new Rectangle((int)tower.Position.X, (int)tower.Position.Y, (int)tower.Width, (int)tower.Height);
                    batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    batch.Draw(tower.Texture, towerRect, color);
                    batch.End();

                    if (tower.TowerState == TowerState.Bought)
                    {
                        if (Critter.CrittersList.Count > 0)
                        {
                            foreach (Critter critter in Critter.CrittersList)
                            {
                                shoot(tower, critter);
                            }
                        }
                    }
                }
            }
        }

        private void checkForBoosted()
        {
            if (this.Boosted != 0)
            {
                if (LogicController.getCurrentSeconds() - this.Boosted > Constants.TowerBoostDuration)
                {
                    this.Boosted = 0;

                    float boostAmount = Constants.AbilityTowerBoost;
                    this.Accuracy /= boostAmount;
                    this.Damage /= (int)boostAmount;
                    this.ReloadSpeed *= boostAmount;
                    this.Accuracy /= boostAmount;
                    this.Range /= (int)boostAmount;
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
                        batch.Draw(bullet.Texture, bullet.Position, bullet.BulletColor);                            
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
                    float bulletSpeed = 5.0f + (float)RandomHandler.GetRandom(0.0, 2.0);
                    bool hitCritter = false;
                    if (tower.Accuracy + RandomHandler.GetRandom() >= critter.Dexterity + RandomHandler.GetRandom())
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

            bullet.Position = new Vector2(tower.Position.X + (tower.Width / 2), 
                                          tower.Position.Y + (tower.Height / 2));

            bullet.Direction = critter.Position - tower.Position;
            bullet.Direction.Normalize();

            Texture2D texture = null;
            switch (tower.Type)
            {
                case TowerType.RangedTower: texture = Presentation.PresentationController.BulletTexture2D;
                                            bullet.BulletColor = Color.DarkGray;
                                            break;
                case TowerType.MeleeTower: texture = Presentation.PresentationController.BulletTexture2D;
                                           bullet.BulletColor = Color.DarkRed;
                                           break;
                case TowerType.SlowTower: texture = Presentation.PresentationController.BulletTexture2D; 
                                          bullet.Slow = true;
                                          bullet.BulletColor = Color.Cyan;
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
            return (LogicController.TotalGameTimeMinutes * 60 * 1000) +
                   (LogicController.TotalGameTimeSeconds * 1000) +
                   LogicController.TotalGameTimeMilliseconds;
        }

        public static Tower BuyTower(TowerType type)
        {
            Console.WriteLine("Buy Tower called");
            Tower tower = null;

            if (!Tower.PlacingTower)
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
                    tower.Width = (float)tower.Texture.Width;
                    tower.Height = (float)tower.Texture.Height;

                    TowersList.Add(tower);
                    tower.TowerState = TowerState.Placing;
                    Tower.PlacingTower = true;

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
        public static bool BuildTower(Tower tower, Vector2 position)
        {
            return BuildTower(tower, (int)position.X, (int)position.Y);
        }

        // returns true when tower was built successfully
        public static bool BuildTower(Tower tower, int x, int y)
        {
            bool succesBuild = false;
            if (!Presentation.Background.IsOnCity(tower))
            {
                if (!Presentation.Background.IsOnWalkPath(tower))
                {
                    if (!IsAnyTowerInRange(x, y))
                    {
                        int cost = tower.Cost,
                            gold = Player.Gold;
                        if (gold >= cost)
                        {
                            tower.Move(x, y);
                            tower.TowerState = TowerState.Bought;
                            tower.AttackState = AttackState.Shooting;
                            Player.Gold -= cost;
                            Tower.PlacingTower = false;
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

        public void SellTower()
        {
            int gold = (int)(this.Cost * Constants.TowerSaleModifier);
            Player.Gold += gold;

            TowersList.RemoveAt(TowersList.IndexOf(this));

            Console.WriteLine("Sold tower");
        }

        public void UpgradeTower()
        {
            int cost = (int)(this.Cost * Constants.TowerUpgradeModifier);
            if (Player.Gold >= cost)
            {
                Player.Gold -= cost;
                this.Damage += (this.Damage / 10);
                this.Range += (this.Range / 10);
                this.Accuracy += (this.Accuracy * 1.1f);
                this.Cost += (this.Cost / 20);
                this.ReloadSpeed += (this.ReloadSpeed / 10);
                this.Width *= 1.05f;
                this.Height *= 1.05f;

                Console.WriteLine("Tower upgraded");
            }
            else
            {
                Console.WriteLine("Error: Player does not have enough gold to upgrade, cost: " + cost.ToString());
            }
        }

        public static bool IsAnyTowerInRange(Vector2 position)
        {
            return IsAnyTowerInRange((int)position.X, (int)position.Y);
        }

        public static bool IsAnyTowerInRange(int x, int y)
        {
            bool inRange = false;
            float xDiff, yDiff, towerWidth, towerHeight;

            if (TowersList.Count == 0)
            {
                return inRange;
            }

            foreach (Tower tower in TowersList)
            {
                if (tower.TowerState == TowerState.Bought)
                {
                    xDiff = Math.Abs(tower.Position.X - x);
                    yDiff = Math.Abs(tower.Position.Y - y);
                    towerWidth = tower.Width;
                    towerHeight = tower.Height;
                    if (xDiff + 1 < towerWidth && yDiff + 1 < towerHeight)
                    {
                        inRange = true;
                        break;
                    }
                }
            }

            return inRange;
        }

        public static Tower GetTowerAtPosition(Vector2 position)
        {
            foreach (Tower tower in TowersList) 
            {
                float area = (tower.Width + tower.Height) * 0.5f;
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

        public int Range
        {
            set { _range = value; }
            get { return _range; }
        }

        public int Damage
        {
            set { _damage = value; }
            get { return _damage; }
        }

        public float Accuracy
        {
            set { _accuracy = value; }
            get { return _accuracy; }
        }

        public float ReloadSpeed
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

        private long LastAttack
        {
            set { _lastAttack = value; }
            get { return _lastAttack; }
        }

        public int Boosted
        {
            set { _boosted = value; }
            get { return _boosted; }
        }

        public static List<Tower> TowersList
        {
            get { return _towersList; }
            set { _towersList = value; }
        }

        public static Boolean PlacingTower
        {
            get { return _placingTower; }
            set { _placingTower = value; }
        }

    }
}
