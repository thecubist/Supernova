using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperNova
{
    class Planet
    {
        #region Declarations & Properties
        public Animation planetAnimation;
        public Vector2 Position;
        public bool Active;
        float planetMoveSpeed;


        public int Width
        {
            get { return planetAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return planetAnimation.FrameHeight; }
        }

        public Vector2 planetPosition
        {
            get { return Position; }
        }
        #endregion

        public void Initialize(Animation animation, Vector2 position, float moveSpeed, int type)
        {

            planetAnimation = animation;
            Position = position;
            Active = true;
            switch (type)
            {
                case (0):
                    
                    planetMoveSpeed = moveSpeed;
                    
                    break;
                case (1):
                    
                    planetMoveSpeed = moveSpeed;
                    
                    break;
            }
            planetMoveSpeed = moveSpeed;
        }

        public void Update(GameTime gameTime)
        {
            Position.X -= planetMoveSpeed;
            planetAnimation.Position = Position;
            planetAnimation.Update(gameTime);

            if (Position.X < -Width)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            planetAnimation.Draw(spriteBatch);
        }

    }
}
  
