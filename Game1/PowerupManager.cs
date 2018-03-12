using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace SuperNova
{
    class PowerupManager
    {
        #region declarations
        Texture2D[] PowerupTexture = new Texture2D[6];
        static public List<Powerup> PowerupList = new List<Powerup>();
        TimeSpan PowerupSpawnTime;
        TimeSpan previousSpawnTime = TimeSpan.Zero;
        Random random = new Random();
        Vector2 graphicsInfo;
        RandomMath randomMath = new RandomMath();
        float moveSpeed;
        bool aquired = false;
        string outMessage;
        SpriteFont alertFont;
        int notiDelay = 0;
        public int numOfPower;
        #endregion  

        public void Intiialize(GraphicsDevice Graphics)
        {

            PowerupSpawnTime = TimeSpan.FromSeconds(randomMath.GetRandomDouble(15, 30));
            graphicsInfo.X = Graphics.Viewport.Width;
            graphicsInfo.Y = Graphics.Viewport.Height;
            this.moveSpeed = randomMath.GetRandomDouble(0.6, 1.0);
            
        }

        public void clean()
        {
            PowerupList.Clear();
        }

        public void loadItems(ContentManager content)
        {
            PowerupTexture[0] = content.Load<Texture2D>("x2Spritesheet");
            PowerupTexture[1] = content.Load<Texture2D>("x3Spritesheet");
            PowerupTexture[2] = content.Load<Texture2D>("x4Spritesheet");
            PowerupTexture[3] = content.Load<Texture2D>("healthSpritesheet");
            PowerupTexture[4] = content.Load<Texture2D>("lifeSpritesheet");
            PowerupTexture[5] = content.Load<Texture2D>("shieldSpritesheet");

            alertFont = content.Load<SpriteFont>("labelFont");
        }


        private void AddPowerup()
        {   //choose and random Powerup skin between 0 and n
            int randPowerup = random.Next(0, PowerupTexture.Length);
            Animation PowerupAnimation = new Animation();
            PowerupAnimation.Intialize(PowerupTexture[randPowerup], Vector2.Zero, 30, 30, 11, 100, Color.White, 1f, true);
            int newY = (int)graphicsInfo.Y;
            Vector2 position = new Vector2(graphicsInfo.X + PowerupTexture[randPowerup].Width / 2, random.Next(50, newY - 50));
            Powerup Powerup = new Powerup();
            Powerup.Initialize(PowerupAnimation, position, moveSpeed);
            Powerup.Type = randPowerup;
            PowerupList.Add(Powerup);
        }

        public void UpdatePowerups(GameTime gameTime, Player player, LaserManager lasMan, Sound puSND, GUI gui)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > PowerupSpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                AddPowerup();
            }

            Collision(player, lasMan, puSND, gui);

            for (int i = (PowerupList.Count - 1); i >= 0; i--)
            {
                PowerupList[i].Update(gameTime);
                if (PowerupList[i].Active == false)
                { PowerupList.RemoveAt(i); }
            }
        }

        public void DrawPowerups(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < PowerupList.Count; i++)
            {
                PowerupList[i].Draw(spriteBatch);
            }

            if(aquired == true && notiDelay > 0)
            {
                spriteBatch.DrawString(alertFont, outMessage, new Vector2(300, 300), Color.Red);
                notiDelay--;
            }
        }

        public void Collision(Player player, LaserManager lasMan, Sound puSND, GUI gui)
        {
            Rectangle rect1, rect2;
            int type;
            //rectangle for the player hitbox
            rect1 = new Rectangle((int)player.Position.X,(int)player.Position.Y,player.Width, player.Height);

            for (int i = 0; i < PowerupList.Count; i++)
            {
                //rectangle for the powerup hitbox
                rect2 = new Rectangle((int)PowerupList[i].Position.X,(int)PowerupList[i].Position.Y, PowerupList[i].Width, PowerupList[i].Height);

                if (rect1.Intersects(rect2))
                {
                    aquired = true;
                    type = PowerupList[i].Type;
                    notiDelay = 300;
                    puSND.playSound();

                    switch (type)
                    {
                        case 0:
                            lasMan.multiply(2, notiDelay);
                            outMessage = "Double Score";
                            numOfPower++;
                            Debug.WriteLine("Double score");
                            break;
                        case 1:
                            lasMan.multiply(3, notiDelay);
                            outMessage = "Triple Score";
                            numOfPower++;
                            Debug.WriteLine("triple score");
                            break;
                        case 2:
                            lasMan.multiply(4, notiDelay);
                            outMessage = "Quad Score";
                            numOfPower++;
                            Debug.WriteLine("quad score");
                            break;
                        case 3:
                            player.changeHealth(25);
                            outMessage = "+25 Health";
                            Debug.WriteLine("p health - " + player.Health);
                            break;
                        case 4:
                            gui.LIVES = gui.LIVES + 1;
                            outMessage = "+1 life";
                            break;
                        case 5:
                            player.changeShields(25);
                            outMessage = "+25 Shields";
                            Debug.WriteLine("p shields - " + player.Shields);
                            break;
                        
                    }
                    //collision code here ----------------------------------------------------------------------------------------------------------------------------------------------------
                    PowerupList[i].Health = 0;
                }
            }
        }
    }
}