using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperNova
{
    class GUI
    {
        #region declarations
        private int score;
        private int playerhp;
        private int playershields;
        private int lives;
        private int gameLevel;
        private int levelUp = 200;

        Texture2D[] healthSq = new Texture2D[4];
        Texture2D[] shieldSq = new Texture2D[4];
        Texture2D legend;
        Texture2D backDrop;

        SpriteFont scoreFont, labelFont, alertFont;
        GamePadState gamePad;
        PowerupManager powerupManager;
        EnemyManager eM;
        LaserManager lM;
        #endregion
        #region properties
        public int SCORE
        {
            get { return score; }
            set { this.score = value; }
        }
        public int PlayerHP
        {
            get { return playerhp; }
            set { this.playerhp = value; }
        }

        public int PlayerShields
        {
            get { return playershields; }
            set { this.playershields = value; }
        }
        public int LIVES
        {
            get { return lives; }
            set { this.lives = value; }
        }
        public int LEVEL
        {
            get { return gameLevel; }
            set { this.gameLevel = value; }
        }
        public int LEVELUP
        {
            get { return levelUp; }
            set { this.levelUp = value; }
        }
        #endregion
        public void Initialize(int Score, int HP, int Shields, int Lives, int Level, PowerupManager powerupManager, EnemyManager eM, LaserManager lM)
        {
            score = Score;
            playerhp = HP;
            PlayerShields = Shields;
            lives = Lives;
            gameLevel = Level;
            this.powerupManager = powerupManager;
            this.eM = eM;
            this.lM = lM;
        }

        public void LoadContent(ContentManager Content)
        {
            legend = Content.Load<Texture2D>("legend3");
            backDrop = Content.Load<Texture2D>("pauseBackdrop");
            scoreFont = Content.Load<SpriteFont>("scoreFont");
            labelFont = Content.Load<SpriteFont>("labelFont");
            alertFont = Content.Load<SpriteFont>("alertFont");
            //loading all of the content for the healthSq and shieldSq arrays
            for (int i = 0; i < 4; i++)
            {
                healthSq[i] = Content.Load<Texture2D>("health_sq");
                shieldSq[i] = Content.Load<Texture2D>("shield_sq");
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int screenWidth, int screenHeight, bool pause)
        {
            #region GUI
            #region score
            spriteBatch.Draw(legend, new Vector2(0, -10), Color.White); //background for the gui
            spriteBatch.DrawString(scoreFont, "" + score, new Vector2(590,7), Color.Black);
            #endregion
            #region health and shields
            const int ORIGIN_X = 23;
            const int ORIGIN_Y = 14;

            #region health code
            int HEALTH_ORIGIN_OFFSET_X = (screenWidth / 2) - 20;
            int numOfHSquares = 0;  //must NOT be more than 4 
            #region square boundaries
            if (PlayerHP <= 0)
                numOfHSquares = 0;
            if (PlayerHP > 0)
                numOfHSquares = 1;
            if (PlayerHP > 25)
                numOfHSquares = 2;
            if (PlayerHP > 50)
                numOfHSquares = 3;
            if (PlayerHP > 75)
                numOfHSquares = 4;
            #endregion
            #region draw health squares
            for (int i = 0; i < numOfHSquares; i++)
            {
                spriteBatch.Draw(healthSq[i], new Vector2(HEALTH_ORIGIN_OFFSET_X - ORIGIN_X * (i + 1), ORIGIN_Y), Color.White);
            }
            #endregion
            #endregion
            #region shield code
            int SHIELD_ORIGIN_OFFSET_X = screenWidth / 2;
            int numOfSSquares = 0;  //must NOT be more than 4
            #region square boundaries
            if (PlayerShields <= 0)
                numOfSSquares = 0;
            if (PlayerShields > 0)
                numOfSSquares = 1;
            if (PlayerShields > 25)
                numOfSSquares = 2;
            if (PlayerShields > 50)
                numOfSSquares = 3;
            if (PlayerShields > 75)
                numOfSSquares = 4;
            #endregion
            #region draw shield squares
            for (int i = 0; i < numOfSSquares; i++)
            {
                spriteBatch.Draw(shieldSq[i], new Vector2(SHIELD_ORIGIN_OFFSET_X + ORIGIN_X * (i + 1), ORIGIN_Y), Color.White);
            }
            #endregion
            #endregion
            #region lives code
            spriteBatch.DrawString(scoreFont, "" + lives, new Vector2(230, 7), Color.Black);
            #endregion
            #endregion
            #region pause
            
            if (pause == true)
            {
                spriteBatch.Draw(backDrop, new Vector2(0,0), Color.White);
                spriteBatch.DrawString(alertFont, "Game Paused", new Vector2(200, 150), Color.Red);
                gamePad = GamePad.GetState(PlayerIndex.One); //update the gamepad to check if its connected

                if (gamePad.IsConnected)
                {
                    spriteBatch.DrawString(labelFont, "Press The Start Button To Continue", new Vector2(200, 220), Color.Red);
                    spriteBatch.DrawString(labelFont, "Press The Back Button To Exit The Game", new Vector2(200, 250), Color.Red);
                    spriteBatch.DrawString(labelFont, "Press The Y Button To Return To The MainMenu", new Vector2(200, 280), Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(labelFont, "Press Enter To Continue", new Vector2(200, 220), Color.Red);
                    spriteBatch.DrawString(labelFont, "Press the Escape Key To Exit The Game", new Vector2(200, 250), Color.Red);
                    spriteBatch.DrawString(labelFont, "Press the M Key To Return To The Menu", new Vector2(200, 280), Color.Red);
                }
                spriteBatch.DrawString(labelFont, "Powerups Collected: " + powerupManager.numOfPower.ToString(), new Vector2(100, 400), Color.Red);
                spriteBatch.DrawString(labelFont, "Enemies Killed: " + (eM.enemiesDestroyed + lM.enemiesDestroyed).ToString(), new Vector2(100, 415), Color.Red);
            }
            #endregion
            #endregion
        }
    }
}
