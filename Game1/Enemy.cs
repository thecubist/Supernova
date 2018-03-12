using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SuperNova
{
    class Enemy
    {
        #region Declarations & Properties
        public Animation EnemyAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public int Damage, Value;
        public bool shoot;
        public bool firing;
        public bool boss;
        float enemyMoveSpeed;
        public TimeSpan previousLaserSpawnTime;


        public int Width
        {
            get { return EnemyAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return EnemyAnimation.FrameHeight; }
        }

        public Vector2 EnemeyPosition
        {
            get { return Position; }
        }

        public bool Fire
        {
            get { return firing; }
        }
        #endregion

        public void Initialize(Animation animation, Vector2 position, int type)
        {
            EnemyAnimation = animation;
            Position = position;
            Active = true;
            switch (type)
            {
                case (0):
                    Health = 10;
                    Damage = 10;
                    enemyMoveSpeed = 4f;
                    Value = 100;
                    shoot = false;
                    boss = false;
                    break;
                case (1):
                    Health = 10;
                    Damage = 5;
                    enemyMoveSpeed = 6f;
                    Value = 200;
                    shoot = true;
                    firing = true;
                    boss = false;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            Position.X -= enemyMoveSpeed;
            EnemyAnimation.Position = Position;
            EnemyAnimation.Update(gameTime);

            if(Position.X < -Width || Health <= 0)
            {
                Active = false;
                firing = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            EnemyAnimation.Draw(spriteBatch);
        }

    }
}
