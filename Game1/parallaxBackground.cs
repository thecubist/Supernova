using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperNova
{
    class parallaxBackground
    {
        Texture2D texture;
        Vector2[] positions;
        float speed;
        int bgHeight, bgWidth;
        Rectangle rectangle1, rectangle2;

        public void initialize(ContentManager content, String texturePath, int screenWidth, int screenHeight, float speed)
        {
            bgHeight = screenHeight;
            bgWidth = screenWidth;
            texture = content.Load<Texture2D>(texturePath);
            this.speed = speed;
            positions = new Vector2[screenWidth / texture.Width + 1];
            rectangle1 = new Rectangle(0, 0, texture.Width, texture.Height);
            rectangle2 = new Rectangle(texture.Width, 0, texture.Width, texture.Height);

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }

        public void update()
        {
            if (rectangle1.X + texture.Width <= 0)
                rectangle1.X = rectangle2.X + texture.Width;
            if (rectangle2.X + texture.Width <= 0)
                rectangle2.X = rectangle1.X + texture.Width;

            rectangle1.X -= (int)speed;
            rectangle2.X -= (int)speed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < positions.Length; i++)
            {
                //Rectangle rectBg = new Rectangle((int)positions[i].X, (int)positions[i].Y, bgWidth, bgHeight);
                //spriteBatch.Draw(texture, rectBg, Color.White);

                spriteBatch.Draw(texture, rectangle1, Color.White);
                spriteBatch.Draw(texture, rectangle2, Color.White);
            }
        }
    }
}
