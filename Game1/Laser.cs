using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SuperNova
{
    class Laser
    {
        #region Declarations 
        public Animation LaserAnimation;
        float LaserMoveSpeed;
        public Vector2 Position;
        int Damage = 10;
        public bool Active;
        public bool Friendly;

        public int Width
        {
            get { return LaserAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return LaserAnimation.FrameHeight; }
        }
        public int DMG
        {
            get { return Damage; }
        }
        #endregion 

        public void Initialize(Animation animation, Vector2 position, float laserSpeed)
        {
            LaserAnimation = animation;
            Position = position;
            Active = true;
            LaserMoveSpeed = laserSpeed;
        }

        public void Update(GameTime gameTime)
        {
            if (Friendly == true)
            {
                Position.X += LaserMoveSpeed;
            }
            else Position.X -= LaserMoveSpeed;
            LaserAnimation.Position = Position;
            LaserAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            LaserAnimation.Draw(spriteBatch);
        }

    }
}
