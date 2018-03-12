using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace SuperNova
{
    class ExplosionManager
    {
        List<Explosion> explostions;
        Texture2D explosionTexture;
        Vector2 graphicsInfo;
        SpriteFont alertFont;
        

        public void Initialize(Texture2D texture, GraphicsDevice Graphics)
        {
            graphicsInfo.X = Graphics.Viewport.Width;
            graphicsInfo.Y = Graphics.Viewport.Height;
            explostions = new List<Explosion>();
            explosionTexture = texture;
            
        }

        public void Load(ContentManager content)
        {
            alertFont = content.Load<SpriteFont>("labelFont");
        }    

        public void clean()
        {
            explostions.Clear();
        }

        public void addExplosion(Vector2 enemyPosition, Sound explosionSound)
        {
            Animation explosionAnimation = new Animation();
            explosionAnimation.Intialize(explosionTexture, enemyPosition, 32, 32, 8, 4, Color.White, 1.0f, true);
            Explosion explosion = new Explosion();
            explosion.Initialize(explosionAnimation, enemyPosition);

            explostions.Add(explosion);
            explosionSound.playSound();
        }

        public void updateExplosions(GameTime gameTime)
        {
            for (var i = 0; i < explostions.Count; i++)
            {
                explostions[i].Update(gameTime);

                if (!explostions[i].Active)
                {
                    explostions.Remove(explostions[i]);
                }
            }
        }

        public void DrawExplosions(SpriteBatch spriteBatch)
        {
            foreach (var i in explostions)
            {
                i.Draw(spriteBatch);
            }
        }

    }
}
