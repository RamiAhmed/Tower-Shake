using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerShake.Logic
{
    public static class Constants
    {
        // Player constants
        private static int START_GOLD = 50;
        private static int START_LIVES = 10;

        private static int SPECIAL_ABILITY_COST = 50;
        private static int SPECIAL_ABILITY_SLOW_DAMAGE = 5;
        private static float SPECIAL_ABILITY_SLOW_AMOUNT = 0.75f;
        private static float SPECIAL_ABILITY_TOWER_BOOST = 2f; // NOTE: Some properties only take integers!
        private static int SPECIAL_ABILITY_TOWER_BOOST_DURATION = 5;

        // Tower constants = { ranged tower, melee tower, slow tower }
        private static int[] COST = { 20, 25, 30 };
        private static int[] DAMAGE = { 15, 10, 5 };
        private static int[] RANGE = { 100, 50, 150 };
        private static float[] ACCURACY = { 0.5f, 0.75f, 1f };
        private static float[] RELOAD_SPEED = { 0.75f, 0.5f, 0.9f };

        private static int TOWER_BUTTON_SENSITIVITY = 125;
        private static int SLOW_TOWER_SPLASH_RANGE = 30;
        private static float BULLET_SPEED_MULTIPLIER = 1.5f;
        private static float SLOW_TOWER_SLOW_AMOUNT = 0.25f;
        private static float TOWER_SALE_MODIFIER = 0.5f;
        private static float TOWER_UPGRADE_MODIFIER = 0.35f;

        // Critter constants = { low level, medium level, high level }
        private static float[] SPEED = { 1.5f, 2.5f, 5.0f};
        private static int[] HP = { 15, 30, 60 };
        private static int[] POINTS = { 1, 3, 6 }; 
        private static float[] DEXTERITY = { 0.25f, 0.5f, 0.75f };
        private static int LEVEL_LENGTH = 5; // amount of waves per level
        private static float UPGRADE_CHANCE = 0.05f;

        private static float CRITTER_LEVEL_HP_MOD = 0.25f;
        private static float CRITTER_LEVEL_SPEED_MOD = 0.05f;
        private static float CRITTER_LEVEL_POINTS_MOD = 0.1f;
        private static float CRITTER_LEVEL_DEX_MOD = 0.01f;

        private static int CRITTER_DEFAULT_LEVEL_SIZE = 10;
        private static float CRITTER_LEVEL_SIZE_MODIFIER = 0.5f;
        private static int CRITTER_MAX_LEVEL_SIZE = 25;

        // Misc constants
        private static int CITY_SIZE = 65;
        private static int WALK_PATH_WIDTH = 5;

        private static int SLOW_DURATION = 5; // counts for both towers and special ability

        private static int MOUSE_WIDTH = 35;
        private static int MOUSE_HEIGHT = 20;

        private static int STAGE_WIDTH = 800;
        private static int STAGE_HEIGHT = 600;

        private static string WINDOW_TITLE = "Tower Defense Game - Tower Shake";
        private static float FRAMES_PER_SECOND = 40;


        // Misc getters
        #region MiscGetters

        public static int WalkPathWidth
        {
            get { return WALK_PATH_WIDTH; }
        }
        public static float FPS
        {
            get { return FRAMES_PER_SECOND; }
        }
        public static string WindowTitle
        {
            get { return WINDOW_TITLE; }
        }
        public static int StageWidth
        {
            get { return STAGE_WIDTH; }
        }
        public static int StageHeight
        {
            get { return STAGE_HEIGHT; }
        }
        public static int MouseWidth
        {
            get { return MOUSE_WIDTH; }
        }
        public static int MouseHeight
        {
            get { return MOUSE_HEIGHT; }
        }
        public static int SlowDuration
        {
            get { return SLOW_DURATION; }
        }
        public static int CitySize
        {
            get { return CITY_SIZE; }
        }
        #endregion MiscGetters

        // Tower getters
        #region TowerGetters

        public static float TowerUpgradeModifier
        {
            get { return TOWER_UPGRADE_MODIFIER; }
        }
        public static float TowerSaleModifier
        {
            get { return TOWER_SALE_MODIFIER; }
        }
        public static float SlowTowerSlowAmount
        {
            get { return SLOW_TOWER_SLOW_AMOUNT; }
        }
        public static float BulletSpeedMultiplier
        {
            get { return BULLET_SPEED_MULTIPLIER; }
        }
        public static int SlowTowerSplashRange
        {
            get { return SLOW_TOWER_SPLASH_RANGE; }
        }
        public static int TowerButtonSensitivity
        {
            get { return TOWER_BUTTON_SENSITIVITY; }
        }
        public static float RangedTowerSpeed
        {
            get { return RELOAD_SPEED[0]; }
        }
        public static float MeleeTowerSpeed
        {
            get { return RELOAD_SPEED[1]; }
        }
        public static float SlowTowerSpeed
        {
            get { return RELOAD_SPEED[2]; }
        }
        public static float RangedTowerAccuracy
        {
            get { return ACCURACY[0]; }
        }
        public static float MeleeTowerAccuracy
        {
            get { return ACCURACY[1]; }
        }
        public static float SlowTowerAccuracy
        {
            get { return ACCURACY[2]; }
        }
        public static int RangedTowerRange
        {
            get { return RANGE[0]; }
        }
        public static int MeleeTowerRange
        {
            get { return RANGE[1]; }
        }
        public static int SlowTowerRange
        {
            get { return RANGE[2]; }
        }
        public static int RangedTowerDamage
        {
            get { return DAMAGE[0]; }
        }
        public static int MeleeTowerDamage
        {
            get { return DAMAGE[1]; }
        }
        public static int SlowTowerDamage
        {
            get { return DAMAGE[2]; }
        }
        public static int RangedTowerCost
        {
            get { return COST[0]; }
        }
        public static int MeleeTowerCost
        {
            get { return COST[1]; }
        }
        public static int SlowTowerCost
        {
            get { return COST[2]; }
        }
        #endregion TowerGetters

        // Player getters
        #region PlayerGetters

        public static int TowerBoostDuration
        {
            get { return SPECIAL_ABILITY_TOWER_BOOST_DURATION; }
        }
        public static float CritterLevelLength
        {
            get { return LEVEL_LENGTH; }
        }
        public static float AbilityTowerBoost
        {
            get { return SPECIAL_ABILITY_TOWER_BOOST; }
        }
        public static float AbilitySlowAmount
        {
            get { return SPECIAL_ABILITY_SLOW_AMOUNT; }
        }
        public static int AbilitySlowDamage
        {
            get { return SPECIAL_ABILITY_SLOW_DAMAGE; }
        }
        public static int SpecialAbilityCost
        {
            get { return SPECIAL_ABILITY_COST; }
        }
        public static int StartGold
        {
            get { return START_GOLD; }
        }
        public static int StartLives
        {
            get { return START_LIVES; }
        }
        #endregion PlayerGetters

        // Critter getters
        #region CritterGetters        

        public static int CritterLevelMaxSize
        {
            get { return CRITTER_MAX_LEVEL_SIZE; }
        }
        public static float CritterLevelSizeModifier
        {
            get { return CRITTER_LEVEL_SIZE_MODIFIER; }
        }
        public static int CritterDefaultLevelSize
        {
            get { return CRITTER_DEFAULT_LEVEL_SIZE; }
        }
        public static float CritterLevelDexterityModifier
        {
            get { return CRITTER_LEVEL_DEX_MOD; }
        }
        public static float CritterLevelPointsModifier
        {
            get { return CRITTER_LEVEL_POINTS_MOD; }
        }
        public static float CritterLevelSpeedModifier
        {
            get { return CRITTER_LEVEL_SPEED_MOD; }
        }
        public static float CritterLevelHpModifier
        {
            get { return CRITTER_LEVEL_HP_MOD; }
        }
        public static float CritterUpgradeChance
        {
            get { return UPGRADE_CHANCE; }
        }
        public static int LowLevelPoints
        {
            get { return POINTS[0]; }
        }
        public static int MediumLevelPoints
        {
            get { return POINTS[1]; }
        }
        public static int HighLevelPoints
        {
            get { return POINTS[2]; }
        }

        public static int LowLevelHP
        {
            get { return HP[0]; }
        }
        public static int MediumLevelHP
        {
            get { return HP[1]; }
        }
        public static int HighLevelHP
        {
            get { return HP[2]; }
        }

        public static float LowLevelDexterity
        {
            get { return DEXTERITY[0]; }
        }
        public static float MediumLevelDexterity
        {
            get { return DEXTERITY[1]; }
        }
        public static float HighLevelDexterity
        {
            get { return DEXTERITY[2]; }
        }

        public static float LowLevelSpeed
        {
            get { return SPEED[0]; }
        }
        public static float MediumLevelSpeed
        {
            get { return SPEED[1]; }
        }
        public static float HighLevelSpeed
        {
            get { return SPEED[2]; }
        }
        #endregion CritterGetters

    }
}
