using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperNova
{
    class LaserManager
    {
        #region Declarations
        static Texture2D laserTexture;
        static Rectangle laserRectangle;
        static public List<Laser> laserBeams;
        const float SECONDS_IN_MINUTE = 60f;
        const float RATE_OF_FIRE = 350f;
        static float laserSpeed;
        static TimeSpan laserSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
        static TimeSpan previousLaserSpawnTime;
        static Vector2 graphicsInfo;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;
        
        int multiplier = 1;
        int multiplierTimer = 0;

        public int enemiesDestroyed = 0;
        #endregion

        public void multiply(int multiple, int time)
        {
            this.multiplier = multiple;
            multiplierTimer = time;
        }

        public void Initialize(Texture2D texture, GraphicsDevice Graphics)
        {
            laserBeams = new List<Laser>();
            previousLaserSpawnTime = TimeSpan.Zero;
            laserTexture = texture;
            graphicsInfo.X = Graphics.Viewport.Width;
            graphicsInfo.Y = Graphics.Viewport.Height;
        }

        public void clean()
        {
            laserBeams.Clear();
        }

        private static void FireLaser(GameTime gameTime, Player p, Sound SND)
        {
            if (gameTime.TotalGameTime - previousLaserSpawnTime > laserSpawnTime)
            {
                previousLaserSpawnTime = gameTime.TotalGameTime;
                // Add the laer to our list.
                AddLaser(p);
                SND.playSound();
            }
        }
        private static void AddLaser(Player p)
        {
            laserSpeed = 30f;
            Animation laserAnimation = new Animation();
            laserAnimation.Intialize(laserTexture, p.Position, 60, 21, 1, 30, Color.White, 1f, true);
            Laser laser = new Laser();
            laser.Friendly = true;
            var laserPostion = p.Position;
            laserPostion.Y += 12;
            laserPostion.X += 50;
            laser.Initialize(laserAnimation, laserPostion, laserSpeed);
            laserBeams.Add(laser);
        }

        public void UpdateManagerLaser(GameTime gameTime, Player p, Sound laserSnd, Sound explosionSound, ExplosionManager vfx, GUI gui)
        {
            //Save the previous state of the keyboard and game pad so we can determine single key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            //Read the current state of the keyboard and gamepad and store it
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            currentKeyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0f)
            {
                FireLaser(gameTime, p, laserSnd);
            }

            // update laserBeams
            for (var i = 0; i < laserBeams.Count; i++)
            {
                laserBeams[i].Update(gameTime);
                //if the laser reaches the end of the screen remove it
                if (!laserBeams[i].Active || laserBeams[i].Position.X > graphicsInfo.X)
                {
                    laserBeams.Remove(laserBeams[i]);
                }
            }
            foreach (Enemy e in EnemyManager.enemiesType1)
            {
                //create a retangle for the enemy
                Rectangle enemyRectangle = new Rectangle((int)e.Position.X,(int)e.Position.Y,e.Width,e.Height);

                //check if the laser collides with any of the enemies
                foreach (Laser L in LaserManager.laserBeams)
                {
                    //create hitbox rectangle for laser
                    laserRectangle = new Rectangle((int)L.Position.X,(int)L.Position.Y,L.Width,L.Height);

                    //is the laser inside the enemy
                    if (laserRectangle.Intersects(enemyRectangle))
                    {
                        //play sound here
                        //pass the hit location to the explosion
                        vfx.addExplosion(e.Position, explosionSound);

                        //add one to count of enemies destroyed
                        enemiesDestroyed++;

                        //kill the enemy
                        e.Health = 0;

                        //when enemy variations are added allow different scores for the each enemy type
                        gui.SCORE += (e.Value * multiplier);
                        //disable the laser
                        L.Active = false;
                    }
                }
            }

            if (multiplier != 1 && multiplierTimer > 0)
            {
                multiplierTimer--;
            }
            else
            {
                multiplier = 1;
            }

        }


        public void DrawLasers(SpriteBatch spriteBatch)
        {
            //draw stuff
            foreach (var l in laserBeams)
            {
                l.Draw(spriteBatch);
            }
        }
    }

}

