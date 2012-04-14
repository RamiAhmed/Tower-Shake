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

        // Private variables
        private static List<Rectangle> _pathsList = new List<Rectangle>();
        private static Rectangle _city;

        private Texture2D bgTexture = PresentationController.BackgroundTexture2D, 
                          pathTexture = PresentationController.PathTexture2D,
                          cityTexture = PresentationController.CityTexture2D,
                          leftButtonTexture = PresentationController.RangedTowerButtonTexture2D,
                          midButtonTexture = PresentationController.MeleeTowerButtonTexture2D,
                          rightButtonTexture = PresentationController.SlowTowerButonTexture2D;
        private int pathWidth = Logic.Constants.WalkPathWidth;
        private Rectangle entireScreen = new Rectangle(0, 0, Logic.Constants.StageWidth, Logic.Constants.StageHeight);
        private int stageHeight = Logic.Constants.StageHeight;

        public Background()
        {
            initWalkPath();
        }

        public void DrawBackground(SpriteBatch batch)
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
            Rectangle lastPath = PathsList.ElementAt(PathsList.Count - 1);
            Vector2 cityPos = new Vector2(lastPath.X, lastPath.Y + lastPath.Height);
            City = new Rectangle((int)cityPos.X - (size / 2), (int)cityPos.Y - (size / 2), size, size);

            batch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            batch.Draw(cityTexture, City, Color.White);
            batch.End();
        }

        private void drawPaths(SpriteBatch batch)
        {
            if (PathsList.Count > 0)
            {
                batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                foreach (Rectangle path in PathsList)
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
            PathsList.Add(new Rectangle(firstPathStartX, firstPathStartY, pathWidth, firstPathHeight));
            // Connector between first and second lane - horizontal
            PathsList.Add(new Rectangle(secondPathStartX, secondPathStartY, secondPathWidth, pathWidth));
            // Second lane (middle) - vertical
            PathsList.Add(new Rectangle(thirdPathStartX, thirdPathStartY, pathWidth, thirdPathHeight));
            // Connector between second and third lane - horizontal
            PathsList.Add(new Rectangle(fourthPathStartX, fourthPathStartY, fourthPathWidth, pathWidth));
            // Third lane (rightmost) - vertical
            PathsList.Add(new Rectangle(fifthPathStartX, fifthPathStartY, pathWidth, fifthPathHeight));

        }

        public static bool IsOnWalkPath(Logic.Tower tower)
        {
            bool onPath = false;
            Rectangle towerBox = new Rectangle((int)tower.Position.X, (int)tower.Position.Y, tower.Texture.Width, tower.Texture.Height);
            foreach (Rectangle path in PathsList)
            {
                if (path.Intersects(towerBox))
                {
                    onPath = true;
                    break;
                }
            }

            return onPath;
        }

        public static bool IsOnWalkPath(Logic.Critter critter)
        {
            bool onWalkPath = false;
            Rectangle critterBox = new Rectangle((int)critter.Position.X, (int)critter.Position.Y, critter.Texture.Width, critter.Texture.Height);
            foreach (Rectangle path in PathsList)
            {
                if (path.Intersects(critterBox))
                {
                    onWalkPath = true;
                    break;
                }
            }

            return onWalkPath;
        }

        public static bool IsOnCity(Logic.Critter critter)
        {
            bool onCity = false;
            Rectangle critterBox = new Rectangle((int)critter.Position.X, (int)critter.Position.Y, critter.Texture.Width, critter.Texture.Height);
            if (City.Intersects(critterBox))
            {
                onCity = true;
            }

            return onCity;
        }

        public static bool IsOnCity(Logic.Tower tower)
        {
            bool onCity = false;
            Rectangle towerBox = new Rectangle((int)tower.Position.X, (int)tower.Position.Y, tower.Texture.Width, tower.Texture.Height);
            if (City.Intersects(towerBox))
            {
                onCity = true;
            }

            return onCity;
        }

        public static List<Rectangle> PathsList
        {
            set { _pathsList = value; }
            get { return _pathsList; }
        }

        public static Rectangle City
        {
            set { _city = value; }
            get { return _city; }
        }


    }
}
