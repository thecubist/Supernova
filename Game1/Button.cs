using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
namespace SuperNova
{
    class Button
    {

        Texture2D texture, over;
        Vector2 position;
        Rectangle rectangle;
        GamePadState gps;
        MouseState mouse;
        Color colour = new Color(30, 144, 255);
        bool mouseUsedLast;
        public static Vector2 size;
        int cx, cy, mx, my;
        bool mouseOver = false;
        public void loadButton(ContentManager content, GraphicsDevice graphics)
        {
            size = new Vector2(graphics.Viewport.Width / 6, graphics.Viewport.Height / 30);
        }

        public Button()
        {

        }

        public void initialzie(ContentManager content, GraphicsDevice graphics, Vector2 position, Texture2D texture, Texture2D over)
        {
            this.texture = texture;
            this.over = over;
            setPosition(position);
            size = new Vector2(graphics.Viewport.Width / 6, graphics.Viewport.Height / 30);
        }

        bool down;
        public bool isClicked;

        public void Update(Pointer pointer, Sound slSND)
        {
            mouse = Mouse.GetState();
            gps = GamePad.GetState(PlayerIndex.One);
            mouseUsedLast = pointer.MouseUsedLast;
            mx = pointer.MousePosX;
            my = pointer.MousePosY;
            cx = pointer.ControllerPosX;
            cy = pointer.ControllerPosY;
            Rectangle mouseRectangle = new Rectangle(0, 0, 1, 1);
            Rectangle controllerRectangle = new Rectangle(0, 0, (int)size.X, 1);
            rectangle = new Rectangle((int)position.X, (int)position.Y,(int)size.X, (int)size.Y);

            if (mouseUsedLast == true)
            {
                mouseRectangle = new Rectangle(mx, my, 10, 10);
            }
            else
            {
                controllerRectangle = new Rectangle(cx, cy, 10, 10);
            }

            if (mouseRectangle.Intersects(rectangle) || controllerRectangle.Intersects(rectangle))
            {
                if (colour.A == 255) down = false;
                if (colour.A == 0) down = true;
                if (down) colour.A += 3; else colour.A -= 3;
                if(clicked(slSND)) isClicked = true;
                mouseOver = true;
            }
            else if (colour.A < 255)
            {
                colour.A += 3;
                isClicked = false;
            }
            else { mouseOver = false; }
        }
        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (mouseOver == true)
            {
                spriteBatch.Draw(texture, rectangle, colour);
            }
            else
            {
                spriteBatch.Draw(over, rectangle, colour);
            }
        }
        public bool clicked(Sound slSND)
        {
            if (mouse.LeftButton == ButtonState.Pressed || gps.Buttons.A == ButtonState.Pressed)
            {
                slSND.playSound();
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
