using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperNova
{
    class Powerup
    {
        #region Declarations & Properties
        public Animation powerupAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        float powerupMoveSpeed;
        int powerType;


        public int Width
        {
            get { return powerupAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return powerupAnimation.FrameHeight; }
        }

        public Vector2 powerupPosition
        {
            get { return Position; }
        }

        public int Type
        {
            get { return powerType; }
            set { this.powerType = value; }
        }
        #endregion

        public void Initialize(Animation animation, Vector2 position, float moveSpeed)
        {

            powerupAnimation = animation;
            Position = position;
            Active = true;
            Health = 1;
            powerupMoveSpeed = moveSpeed;
        }

        public void Update(GameTime gameTime)
        {
            Position.X -= powerupMoveSpeed;
            powerupAnimation.Position = Position;
            powerupAnimation.Update(gameTime);

            if (Position.X < -Width || Health <= 0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            powerupAnimation.Draw(spriteBatch);
        }

    }
}
