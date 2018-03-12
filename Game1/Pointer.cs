using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace SuperNova
{
    class Pointer
    {
        #region declarations
        Texture2D cursor;
        MouseState mouse;
        int viewportWidth, viewportHeight, mx, my, cx = 400, cy = 300, previousMouseX, previousMouseY, currentMouseX, currentMouseY;
        int mouseSpeed = 2;
        GamePadState gps;
        bool mouseLastUsed;

        public int MousePosX
        {
            get { return mx; }
        }

        public int MousePosY
        {
            get { return my; }
        }

        public int ControllerPosX
        {
            get { return cx; }
        }

        public int ControllerPosY
        {
            get { return cy; }
        }

        public bool MouseUsedLast
        {
            get { return mouseLastUsed; }
        }
        #endregion
        public void LoadContent(ContentManager Content)
        {
            cursor = Content.Load<Texture2D>("pointer");
        }
        public void Update()
        {
            mouse = Mouse.GetState();
            gps = GamePad.GetState(PlayerIndex.One);

            currentMouseX = mouse.X;
            currentMouseY = mouse.Y;
            mx = currentMouseX;
            my = currentMouseY;

            if (currentMouseX != previousMouseX)
            {
                previousMouseX = currentMouseX;
                mouseLastUsed = true;
            }
            else if(currentMouseY != previousMouseY)
            {
                previousMouseY = currentMouseY;
                mouseLastUsed = true;
            }
            //if( !(currentMouseY != previousMouseY) && !(currentMouseX != previousMouseX))
            else
            {
                cx += (int) gps.ThumbSticks.Left.X * mouseSpeed;
                cy += (int) (-(gps.ThumbSticks.Left.Y * mouseSpeed));//inversion required due to inverted Y axis bug on controller

                if (gps.ThumbSticks.Left.X < 0f)
                {
                    //Debug.WriteLine("left");
                    cx -= mouseSpeed;
                    mouseLastUsed = false;
                }
                if (gps.ThumbSticks.Left.X > 0f)
                {
                    //Debug.WriteLine("right");
                    cx += mouseSpeed;
                    mouseLastUsed = false;
                }
                if (gps.ThumbSticks.Left.Y > 0f)  //(gps.DPad.Up == ButtonState.Pressed)
                {
                    //Debug.WriteLine("up");
                    cy -= mouseSpeed;
                    mouseLastUsed = false;
                }
                if (gps.ThumbSticks.Left.Y < 0f)  //(gps.DPad.Down == ButtonState.Pressed)
                {
                    //Debug.WriteLine("down");
                    cy += mouseSpeed;
                    mouseLastUsed = false;
                }
            }

            if (mouseLastUsed == true)
            {
                Rectangle mouseRectangle = new Rectangle(mx, my, 1, 1);
            }
            else
            {
                Rectangle mouseRectangle = new Rectangle(cx, cy, 1, 1);
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, MouseState mouse, GraphicsDevice graphics)
        {
            viewportWidth = graphics.Viewport.Width;
            viewportHeight = graphics.Viewport.Height;

            if (mouseLastUsed == true) //if the mouse has moved
            {
                #region bounds
                if (mouse.X < viewportWidth) //if the mouse position is not more than the width of the screen
                    mx = mouse.X; //mouse x is used
                else if (mouse.X > viewportWidth) //if the mouse position is greater than the viewport width
                    mx = viewportWidth - cursor.Width; //mouse rectangle is stuck on the right of the screen (allowing texture to be seen too)
                else if (mouse.X < 0)
                    mx = cursor.Width;
                if (mouse.Y < viewportHeight) //if the mouse position is not more than the height of the screen
                    my = mouse.Y; //mouse y is used
                else if (mouse.Y < 0)
                    my = 0;
                else if (mouse.X < 0)
                    mx = 0;
                else
                    my = viewportHeight - cursor.Height;
                #endregion
                spriteBatch.Draw(cursor, new Rectangle(mx, my, 15, 16), Color.White);
            }
            else
            {
                #region bounds
                if (cx > viewportWidth) //if the mouse position is greater than the viewport width
                    cx = viewportWidth - cursor.Width; //mouse rectangle is stuck on the right of the screen (allowing texture to be seen too)
                else if (cy > viewportHeight)
                    cy = viewportHeight - cursor.Height;
                else if (cy < 0)
                    cy = 0;
                #endregion
                spriteBatch.Draw(cursor, new Rectangle(cx, cy, 15, 16), Color.White);
            }
            //if (mouse.Y < viewportHeight) //if the mouse position is not more than the height of the screen
            // my = mouse.Y; //mouse y is used
            // else
            //my = viewportHeight - cursor.Height; //mouse is stuck to bottom of the screen (showing texture too)
        }

    }
}
