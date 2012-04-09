using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerShake.Presentation
{
    class Background : Logic.Sprite
    {
        // Public variables
        public static List<Rectangle> paths = new List<Rectangle>();
        public static Rectangle city;

        // Private variables
        private Texture2D bgTexture = PresentationController.bgTexture, 
                          pathTexture = PresentationController.path_block,
                          cityTexture = PresentationController.city,
                          leftButtonTexture = PresentationController.ranged_tower_button,
                          midButtonTexture = PresentationController.melee_tower_button,
                          rightButtonTexture = PresentationController.slow_tower_button;
        private int pathWidth = Logic.Constants.WalkPathWidth;
        private Rectangle entireScreen = new Rectangle(0, 0, Logic.Constants.StageWidth, Logic.Constants.StageHeight);
        private int stageHeight = Logic.Constants.StageHeight;

        public Background()
        {
            initWalkPath();
        }

        public void drawBackground(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            batch.Draw(bgTexture, entireScreen, Color.White);
            batch.End();

            drawTowerButtons(batch);

            drawPaths(batch);

            drawCity(batch);
        }

        private void drawTowerButtons(SpriteBatch batch)
        {
            int yPos = stageHeight - leftButtonTexture.Height - 1;
            Color active = Color.White,
                  inactive = new Color(0.5f, 0.5f, 0.5f, 1f);

            batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            Color leftColor = active;
            if (Logic.Player.Gold < Logic.Constants.RangedTowerCost)
            {
                leftColor = inactive;
            }
            batch.Draw(leftButtonTexture, new Vector2(0, yPos), leftColor); // ranged tower

            Color midColor = active;
            if (Logic.Player.Gold < Logic.Constants.MeleeTowerCost)
            {
                midColor = inactive;
            }
            batch.Draw(midButtonTexture, new Vector2(leftButtonTexture.Width, stageHeight - midButtonTexture.Height + 2), midColor); // melee tower

            Color rightColor = active;
            if (Logic.Player.Gold < Logic.Constants.SlowTowerCost) 
            {
                rightColor = inactive;
            }
            batch.Draw(rightButtonTexture, new Vector2(leftButtonTexture.Width + midButtonTexture.Width, yPos), rightColor); // slow tower
           
            batch.End();
        }

        private void drawCity(SpriteBatch batch)
        {
            int size = Logic.Constants.CitySize;
            Rectangle lastPath = paths.ElementAt(paths.Count - 1);
            Vector2 cityPos = new Vector2(lastPath.X, lastPath.Y + lastPath.Height);
            city = new Rectangle((int)cityPos.X - (size / 2), (int)cityPos.Y - (size / 2), size, size);

            batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            batch.Draw(cityTexture, city, Color.White);
            batch.End();
        }

        private void drawPaths(SpriteBatch batch)
        {
            if (paths.Count > 0)
            {
                batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                foreach (Rectangle path in paths)
                {
                    batch.Draw(pathTexture, path, Color.White);
                   // Console.WriteLine("Drawing path" + paths.IndexOf(path).ToString() + ": " + path.ToString());
                }
                batch.End();
            }
        }

        private void initWalkPath()
        {
            // First path downwards in left side
            int firstPathStartX = 50,
                firstPathStartY = 0,
                firstPathHeight = 450;

            // Second path connects first path with third path horizontally
            int secondPathStartX = firstPathStartX,
                secondPathStartY = firstPathHeight,
                secondPathWidth = 300;

            // Third path upwards in middle
            int thirdPathStartX = secondPathStartX + secondPathWidth,
                thirdPathStartY = firstPathHeight - 360,
                thirdPathHeight = 370;

            // Fourth path connects third and fifth path
            int fourthPathStartX = thirdPathStartX,
                fourthPathStartY = thirdPathStartY,
                fourthPathWidth = 375;

            // Fifth path downwards right side
            int fifthPathStartX = fourthPathStartX + fourthPathWidth,
                fifthPathStartY = fourthPathStartY,
                fifthPathHeight = 350;

            // First lane (leftmost) - vertical
            paths.Add(new Rectangle(firstPathStartX, firstPathStartY, pathWidth, firstPathHeight));
            // Connector between first and second lane - horizontal
            paths.Add(new Rectangle(secondPathStartX, secondPathStartY, secondPathWidth, pathWidth));
            // Second lane (middle) - vertical
            paths.Add(new Rectangle(thirdPathStartX, thirdPathStartY, pathWidth, thirdPathHeight));
            // Connector between second and third lane - horizontal
            paths.Add(new Rectangle(fourthPathStartX, fourthPathStartY, fourthPathWidth, pathWidth));
            // Third lane (rightmost) - vertical
            paths.Add(new Rectangle(fifthPathStartX, fifthPathStartY, pathWidth, fifthPathHeight));

        }

        public static bool isOnWalkPath(Logic.Tower tower)
        {
            bool onPath = false;
            Rectangle towerBox = new Rectangle((int)tower.Position.X, (int)tower.Position.Y, tower.Texture.Width, tower.Texture.Height);
            foreach (Rectangle path in paths)
            {
                if (path.Intersects(towerBox))
                {
                    onPath = true;
                    break;
                }
            }

            return onPath;
        }

        public static bool isOnWalkPath(Logic.Critter critter)
        {
            bool onWalkPath = false;
            Rectangle critterBox = new Rectangle((int)critter.Position.X, (int)critter.Position.Y, critter.Texture.Width, critter.Texture.Height);
            foreach (Rectangle path in paths)
            {
                if (path.Intersects(critterBox))
                {
                    onWalkPath = true;
                    break;
                }
            }

            return onWalkPath;
        }

        public static bool isOnCity(Logic.Critter critter)
        {
            bool onCity = false;
            Rectangle critterBox = new Rectangle((int)critter.Position.X, (int)critter.Position.Y, critter.Texture.Width, critter.Texture.Height);
            if (city.Intersects(critterBox))
            {
                onCity = true;
            }

            return onCity;
        }

        public static bool isOnCity(Logic.Tower tower)
        {
            bool onCity = false;
            Rectangle towerBox = new Rectangle((int)tower.Position.X, (int)tower.Position.Y, tower.Texture.Width, tower.Texture.Height);
            if (city.Intersects(towerBox))
            {
                onCity = true;
            }

            return onCity;
        }
                

    }
}
