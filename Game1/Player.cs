using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SuperNova
{
    class Player
    {
        #region Declarations
        public Animation PlayerAnimation;
        public Vector2 Position, graphicsInfo, startPosition;
        public bool Active;
        public int Health;
        public int Shields;
        public Rectangle sourceRect;
        #region control declarations
        KeyboardState cks,pks;
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        #endregion
        private int previousHealth;
        private int depletedHealth;
        float elapsed, delay = 200f, playerMoveSpeed;
        int frames = 0;
        #endregion
        #region properties
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }
        public int getHealth
        {
            get { return Health; }
        }

        #endregion
        public void initialize(Animation animation, Vector2 position, Vector2 grInfo)
        {
            PlayerAnimation = animation;
            sourceRect = new Rectangle(0, 0, 115, 69);
            Position = position;
            startPosition = position;
            Active = true;
            Health = 100;
            Shields = 100;
            graphicsInfo = grInfo;
            playerMoveSpeed = 11.0f;
            previousHealth = 100;
        }

        public void initialize()
        {
            Active = true;
            Health = 100;
            Shields = 100;
            previousHealth = 100;
            Position = startPosition;
        }

        public void update(GameTime gameTime, GUI gui)
        {
            previousGamePadState = currentGamePadState;
            pks = cks;

            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            cks = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            #region gui updates
            if (previousHealth > Health)
            {

                depletedHealth = (previousHealth - Health);
                previousHealth = Health;

                if (Shields > 0)
                {
                    previousHealth = Health + depletedHealth;
                    changeHealth(Health - (Health + depletedHealth));
                    Health = Health + depletedHealth;
                    validate();
                }
                else
                {
                    changeHealth(previousHealth - Health);
                    validate();
                }
                Debug.WriteLine("depleted health - " + depletedHealth);
                Debug.WriteLine("previous health - " + previousHealth);
                Debug.WriteLine("----------------------------------------");
            }

            gui.PlayerHP = Health;
            gui.PlayerShields = Shields;
            #endregion

            #region move code
            Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            Position.Y += (-(currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed));//inversion required due to inverted Y axis bug on controller

            if (cks.IsKeyDown(Keys.A) || currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                Position.X -= playerMoveSpeed;
            }
            if (cks.IsKeyDown(Keys.D) || currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                Position.X += playerMoveSpeed;
            }
            if (cks.IsKeyDown(Keys.W) || currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                Position.Y -= playerMoveSpeed;
            }
            if (cks.IsKeyDown(Keys.S) || currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                Position.Y += playerMoveSpeed;
            }
            else if (Position.X != 0)
            {
                Position.X -= 4;
            }

            if (cks.IsKeyDown(Keys.LeftShift) || currentGamePadState.Buttons.A == ButtonState.Pressed)
            {
                playerMoveSpeed = 20.0f;
            }
            else
            {
                playerMoveSpeed = 11.0f;
            }
            #endregion
            #region player bounds
            Position.X = MathHelper.Clamp(Position.X, 0, 650);
            Position.Y = MathHelper.Clamp(Position.Y, 65, graphicsInfo.Y - Height);
            #endregion
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }
        
        public void draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
        #region multiplier power code
        public void multiply(int multiplier)
        {

        }
        #endregion
        #region health and shield changes
        /// <summary>
        /// change the players health based on how many points are passed. 
        /// if health is subtracted then it will be taken off the shields 
        /// first.
        /// </summary>
        /// <param name="points"></param>
        public void changeHealth(int points)
        {
            //if the passed value is negative then check if there are shields left
            if (points < 0)
            {
                if (Shields <= 0)
                    Health += points;
                else
                    changeShields(points);
            }
            else if (points > 0)
            {
                Health += points;
            }
            validate();
        }

        /// <summary>
        /// change the players shields based on how many points are passed.
        /// this should only be called directly if a shield powerup is aquired
        /// </summary>
        /// <param name="points"></param>
        public void changeShields(int points)
        {
            Debug.WriteLine("playershields - " + Shields);
            Shields += points;
            validate();
        }

        /// <summary>
        /// checks health and shields and sets them to 0 or 100 if 
        /// the health/shield boundaries are exceded
        /// </summary>
        private void validate()
        {
            //check health
            if (Health > 100)
                Health = 100;

            if (Health < 0)
                Health = 0;

            //check shields
            if (Shields > 100)
                Shields = 100;

            if (Shields < 0)
                Shields = 0;
        }
        #endregion

        void Animate(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= 7)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }

            sourceRect = new Rectangle((frames * 115), 0, 114, 69);
        }
    }
}
