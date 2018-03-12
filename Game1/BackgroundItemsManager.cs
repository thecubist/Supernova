using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;



namespace SuperNova
{
    class BackgroundItemsManager
    {
        #region declarations
        Texture2D[] planetTexture = new Texture2D[5];
        static public List<Planet> planetType1 = new List<Planet>();
        TimeSpan planetSpawnTime;
        TimeSpan previousSpawnTime = TimeSpan.Zero;
        Random random = new Random();
        Vector2 graphicsInfo;
        RandomMath randomMath = new RandomMath();
        int prevPlanet = 11;

        float moveSpeed;

        #endregion  

        public void Intiialize(GraphicsDevice Graphics)
        {
            
            planetSpawnTime = TimeSpan.FromSeconds(randomMath.GetRandomDouble(15.0, 40.0));
            graphicsInfo.X = Graphics.Viewport.Width;
            graphicsInfo.Y = Graphics.Viewport.Height;
            this.moveSpeed = randomMath.GetRandomDouble(0.1,10.0);
        }

        public void loadItems(ContentManager content)
        {
            for (int i = 0; i < planetTexture.Length; i++) {
                planetTexture[i] = content.Load<Texture2D>("planet" + (i+1));
                    }
        }


        private void Addplanet()
        {   //choose and random planet skin between 0 and n
            int randPlanet = random.Next(0,5);
            
            if (randPlanet != prevPlanet)
            {
                Animation planetAnimation = new Animation();
                planetAnimation.Intialize(planetTexture[randPlanet], Vector2.Zero, 64, 64, 1, 30, Color.White, 1f, true);
                int newY = (int)graphicsInfo.Y;
                Vector2 position = new Vector2(graphicsInfo.X + planetTexture[randPlanet].Width / 2, random.Next(50, newY - 50));
                Planet planet = new Planet();
                planet.Initialize(planetAnimation, position, moveSpeed, random.Next());
                planetType1.Add(planet);
                prevPlanet = randPlanet;
            }
            
        }

        public void UpdatePlanets(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > planetSpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                Addplanet();
            }

            for (int i = (planetType1.Count - 1); i >= 0; i--)
            {
                planetType1[i].Update(gameTime);
                if (planetType1[i].Active == false)
                { planetType1.RemoveAt(i); }
            }
        }

        public void DrawPlanets(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < planetType1.Count; i++)
            {
                planetType1[i].Draw(spriteBatch);
            }
        }
    }
}
    

