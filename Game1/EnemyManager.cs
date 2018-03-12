using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperNova
{
    class EnemyManager
    {
        #region Declrations
        Texture2D[] enemyTexture = new Texture2D[2];
        static public List<Enemy> enemiesType1 = new List<Enemy>();
        TimeSpan enemySpawnTime = TimeSpan.FromSeconds(0.7f);
        TimeSpan previousSpawnTime = TimeSpan.Zero;
        static public List<Laser> laserEBeams;
        LaserManager enemyLasers = new LaserManager();
        Rectangle laserRectangle;
        static float laserSpeed;
        Texture2D laserTexture;
        const float SECONDS_IN_MINUTE = 60f;
        const float RATE_OF_FIRE = 100f;
        static TimeSpan laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
        static SpriteFont alertFont;
        Random random = new Random();
        public int enemiesDestroyed = 0;
        

        Vector2 graphicsInfo;
        #endregion  

        public void Initialize(Texture2D[] texture, GraphicsDevice Graphics, Texture2D laserTexture)
        {
            graphicsInfo.X = Graphics.Viewport.Width;
            graphicsInfo.Y = Graphics.Viewport.Height;
            enemyTexture = texture;
            this.laserTexture = laserTexture;
            laserEBeams = new List<Laser>();
            
            
        }

        public void Load(ContentManager content)
        {
            alertFont = content.Load<SpriteFont>("labelFont");
        }

        public void clean()
        {
            enemiesType1.Clear();
            laserEBeams.Clear();
        }

        //Adding a random enemy on the field
        private void AddEnemy(int GodsWill)
        {
            Animation enemyAnimation = new Animation();
            int newY;
            Vector2 position;
            Enemy enemy;
            // 40% chance of a normal enemy
            #region Normal
            if (GodsWill >= 0 && GodsWill < 40)
            {
                int type = 0;
                enemyAnimation.Intialize(enemyTexture[0], Vector2.Zero, 49, 16, 4, 30, Color.White, 1f, true);
                newY = (int)graphicsInfo.Y;
                position = new Vector2(graphicsInfo.X + enemyTexture[type].Width / 8, random.Next(100, newY - 100));
                enemy = new Enemy();
                enemy.Initialize(enemyAnimation, position, type);
                enemiesType1.Add(enemy);
            }
            #endregion
            //30% chance of a fast enemy
            #region Fast
            if (GodsWill >= 40 && GodsWill < 70)
            {
                int type = 1;
                enemyAnimation.Intialize(enemyTexture[1], Vector2.Zero, 26, 54, 4, 30, Color.White, 1f, true);
                newY = (int)graphicsInfo.Y;
                position = new Vector2(graphicsInfo.X + enemyTexture[type].Width / 8, random.Next(100, newY - 100));
                enemy = new Enemy();
                enemy.Initialize(enemyAnimation, position, type);
                enemiesType1.Add(enemy);
            }
            #endregion
            //15% chance of line formation
            #region Line
            if (GodsWill >= 70 && GodsWill < 85) { AddLine(); }
            #endregion
            //10% chance of wing formation
            #region Wing
            if (GodsWill >= 85 && GodsWill < 95) { AddWing(); }
            #endregion
            //5% chance of diamond formation
            #region Diamond
            if (GodsWill >= 95) { AddDiamond(); }
            #endregion
        }


        //Add a enemy to the formation
        private void AddSpecEnemy(Vector2 position)
        {
            Animation enemyAnimation = new Animation();
            enemyAnimation.Intialize(enemyTexture[0], Vector2.Zero, 39, 16, 1, 30, Color.White, 1f, true);
            Vector2 Position = position;
            Enemy enemy = new Enemy();
            enemy.Initialize(enemyAnimation, position, 0);
            enemiesType1.Add(enemy);
        }

        //Add a line of enemy ships
        private void AddLine()
        {
            int Ypos = random.Next(100, (int)graphicsInfo.Y - 100);
            int Xpos = (int)graphicsInfo.X + enemyTexture[0].Width / 8;
            AddSpecEnemy(new Vector2(Xpos, Ypos));
            Xpos += enemyTexture[0].Width / 4;
            AddSpecEnemy(new Vector2(Xpos, Ypos));
            Xpos += enemyTexture[0].Width / 4;
            AddSpecEnemy(new Vector2(Xpos, Ypos));
            Xpos += enemyTexture[0].Width / 4;
            AddSpecEnemy(new Vector2(Xpos, Ypos));
        }
        //Add a diamond formation of enemy ships
        private void AddDiamond()
        {
            int YStart, YOneUp, YTop, YOneDown, YBot, X0, X1, X2, X3;
            int margin = enemyTexture[0].Height / 2 + 100;
            YStart = random.Next(margin, (int)graphicsInfo.Y - margin);
            YOneUp = YStart + enemyTexture[0].Height / 2;
            YTop = YStart + enemyTexture[0].Height;
            YOneDown = YStart - enemyTexture[0].Height / 2;
            YBot = YStart - enemyTexture[0].Height;
            X0 = (int)graphicsInfo.X + enemyTexture[0].Width / 8;
            X1 = X0 + enemyTexture[0].Width / 4;
            X2 = X1 + enemyTexture[0].Width / 4;
            X3 = X2 + enemyTexture[0].Width / 4;
            AddSpecEnemy(new Vector2(X0, YStart));
            AddSpecEnemy(new Vector2(X1, YOneUp));
            AddSpecEnemy(new Vector2(X1, YOneDown));
            AddSpecEnemy(new Vector2(X2, YTop));
            AddSpecEnemy(new Vector2(X2, YStart));
            AddSpecEnemy(new Vector2(X2, YBot));
            AddSpecEnemy(new Vector2(X3, YOneUp));
            AddSpecEnemy(new Vector2(X3, YOneDown));
        }

        private void AddWing()
        {
            int YStart, YOneUp, YTop, YOneDown, YBot, X0, X1, X2;
            int margin = enemyTexture[0].Height / 2 + 100;
            YStart = random.Next(margin, (int)graphicsInfo.Y - margin);
            YOneUp = YStart + enemyTexture[0].Height / 2;
            YTop = YStart + enemyTexture[0].Height;
            YOneDown = YStart - enemyTexture[0].Height / 2;
            YBot = YStart - enemyTexture[0].Height;
            X0 = (int)graphicsInfo.X + enemyTexture[0].Width / 8;
            X1 = X0 + enemyTexture[0].Width / 4;
            X2 = X1 + enemyTexture[0].Width / 4;
            AddSpecEnemy(new Vector2(X0, YStart));
            AddSpecEnemy(new Vector2(X1, YOneUp));
            AddSpecEnemy(new Vector2(X1, YOneDown));
            AddSpecEnemy(new Vector2(X2, YTop));
            AddSpecEnemy(new Vector2(X2, YBot));
        }

        public void UpdateEnemies(GameTime gameTime, Player player,ExplosionManager VFX, Sound exSND, SpriteBatch spriteBatch)
        {
            if(gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                AddEnemy(random.Next(100));
            }

            UpdateColission(player, VFX, exSND, spriteBatch);

            for (int i = (enemiesType1.Count - 1); i >= 0; i--)
            {
                enemiesType1[i].Update(gameTime);
                if (enemiesType1[i].Active == false)
                { enemiesType1.RemoveAt(i); }
            }
        }

        public void UpdateEnemyLaser(GameTime gameTime, Player p, Sound snd, Sound exSND, ExplosionManager vfx, GUI gui)
        {
            foreach (Enemy e in enemiesType1)
            {
                if (e.shoot == true)
                {
                    FireELaser(gameTime, e, snd);
                }

            }
            for (var i = 0; i < laserEBeams.Count; i++)
            {
                laserEBeams[i].Update(gameTime);
                //if the laser reaches the end of the screen remove it
                if (!laserEBeams[i].Active || laserEBeams[i].Position.X < 0)
                {
                    laserEBeams.Remove(laserEBeams[i]);
                }
            }
            //collision code for enemy lasers and the player
            Rectangle playerRectangle = new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Width, p.Height);
            foreach (Laser L in laserEBeams)
            {
                laserRectangle = new Rectangle((int)L.Position.X, (int)L.Position.Y, L.Width, L.Height);

                if (laserRectangle.Intersects(playerRectangle))
                {
                    vfx.addExplosion(p.Position, exSND);
                    p.Health = p.Health - L.DMG;
                    L.Active = false;
                }
            }
        }
        //Adds a laser at the enemy position
        private void AddELaser(Enemy p)
        {
            laserSpeed = 10f;
            Animation laserAnimation = new Animation();
            laserAnimation.Intialize(laserTexture, p.Position, 20, 20, 5, 30, Color.White, 1f, true);
            Laser laser = new Laser();
            laser.Friendly = false;
            var laserPostion = p.Position;
            laserPostion.Y += 20;
            laser.Initialize(laserAnimation, laserPostion, laserSpeed);
            laserEBeams.Add(laser);
        }
        //Fires the laser
        public void FireELaser(GameTime gameTime, Enemy p, Sound SND)
        {
            if (gameTime.TotalGameTime - p.previousLaserSpawnTime > laserSpawnTime)
            {
                p.previousLaserSpawnTime = gameTime.TotalGameTime;
                // Add the laser to our list.
                AddELaser(p);
                SND.playSound();
            }
        }

        public void UpdateColission(Player player, ExplosionManager VFX, Sound exSND, SpriteBatch spriteBatch)
        {
            Rectangle rect1, rect2;

            rect1 = new Rectangle(
                (int)player.Position.X,
                (int)player.Position.Y,
                player.Width, player.Height);

            for (int i = 0; i < enemiesType1.Count; i++)
            {
                rect2 = new Rectangle(
                    (int)enemiesType1[i].Position.X,
                    (int)enemiesType1[i].Position.Y,
                    enemiesType1[i].Width,
                    enemiesType1[i].Height);

                if (rect1.Intersects(rect2))
                {
                    player.Health -= enemiesType1[i].Damage;

                    enemiesType1[i].Health = 0;


                    VFX.addExplosion(enemiesType1[i].EnemeyPosition, exSND);
                    enemiesDestroyed++;

                    if (player.Health <= 0)
                    {
                        player.Active = false;
                    }
                }
                
            }
        }


        public void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (var l in laserEBeams)
            {
                l.Draw(spriteBatch);
            }

            for (int i = 0; i < enemiesType1.Count; i++)
            {
                enemiesType1[i].Draw(spriteBatch);
            }

        }
    }
}
