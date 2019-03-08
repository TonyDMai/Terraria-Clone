using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarterra
{
    class cButton
    {
        Texture2D texture; // texture of the button
        Vector2 position; //position of the button
        Rectangle rectangle; // hitbox of the rectangle

        Color colour = new Color(255, 255, 255, 255); // color tint of the button, in this case transparent

        public Vector2 size; // szie of the button

        public cButton(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture; // sets the texture to that of the one loaded
            size = new Vector2(graphics.Viewport.Width / (8/3), graphics.Viewport.Height/ 10); //sets size of button according to resolution
            rectangle = new Rectangle((graphics.Viewport.Bounds.Width/3) - (texture.Width/11), graphics.Viewport.Bounds.Height / 2 - (texture.Height / 2),
                (int)size.X, (int)size.Y); //changes the hitbox of the button
        }

        bool down;//boolean  controlling the glow of the button
        public bool isClicked = false; // boolean for if button is clicked
        public void Update(MouseState mouse)
        {          
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1); // updates hitbox of the rectangle

            if (mouseRectangle.Intersects(rectangle)) // cehcks when the hitboxesintersect
            {
                if (colour.A == 255) down = false;  //makes button glow
                if (colour.A == 0) down = true; //makes button glow
                if (down) colour.A += 3;  else colour.A -= 3; //changes button color                                                          
                if (mouse.LeftButton == ButtonState.Pressed)  isClicked = true;     // cehcks if button is clicked                          
            }
            else if (colour.A < 255)
            {
                colour.A += 3;
                isClicked = false;
            }   
        }

        public void setPosition (Vector2 newPosition) //updates posiition of button
        {
            position = newPosition;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, colour); // draws the button
        }
    }
}
