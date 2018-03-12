using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperNova
{
    class Animation
    {
        #region Declerations

        Texture2D spriteStrip;
        float scale;
        int elapsedTime, FrameTime, FrameCount, currentFrame;
        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destRect = new Rectangle();
        public int FrameWidth, FrameHeight;
        public bool active, Looping;
        public Vector2 Position;
        private List<Rectangle> frames = new List<Rectangle>();

        #endregion

        public void Intialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frameTime, Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.FrameCount = frameCount;
            this.FrameTime = frameTime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            elapsedTime = 0;
            currentFrame = 0;

            active = true;

            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            destRect = new Rectangle(
                (int)Position.X - (int)(FrameWidth * scale) / 2,
                (int)Position.Y - (int)(FrameHeight * scale) / 2,
                (int)(FrameWidth * scale),
                (int)(FrameHeight * scale));

            for (int x = 0; x < frameCount; x++)
            {
                frames.Add(new Rectangle((FrameWidth * x), 0, FrameWidth, FrameHeight));
            }
        }

        public void Update(GameTime gameTime)
        {
            if (active == false) return;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(elapsedTime > FrameTime)
            {
                currentFrame++;
                currentFrame = currentFrame % FrameCount;
                if(Looping == false)
                {
                   active = false;
                }
              
                elapsedTime -= FrameTime;
            }

            sourceRect = frames[currentFrame];
            destRect = new Rectangle((int)Position.X, (int)Position.Y, FrameWidth, FrameHeight);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(spriteStrip, destRect, sourceRect, color);
            }
        }
    }
}
