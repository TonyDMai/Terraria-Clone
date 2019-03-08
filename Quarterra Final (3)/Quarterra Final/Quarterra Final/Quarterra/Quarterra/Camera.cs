using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Quarterra
{
    class Camera
    {
        private Matrix transform; // a objet that is used to  scroll the world
       
        public Matrix Transform
        {
            get { return transform; } // returns the value of transform
        }

        private Vector2 center; // changes the centre of the screen
        private Viewport viewport; // changes the viewport of the map

        public Camera(Viewport newView)
        {
            viewport = newView; // updates the viewport
        }

        public void Update (Vector2 pos, int xOff, int yOff)
        {
            if (pos.X < viewport.Width / 2)
            { center.X = viewport.Width / 2; } // changes the centre of the screen
            else if (pos.X > xOff - (viewport.Width/2))
            {
                center.X = xOff - (viewport.Width / 2); // changes centre of the screen
            }
            else { center.X = pos.X; } // teh centre will be the position of the player


            if (pos.Y < viewport.Height / 2)
            { center.Y = viewport.Height / 2; } //change the centre of camera with regards to height
            else if (pos.Y > yOff - (viewport.Height / 2))
            {
                center.Y = yOff - (viewport.Height / 2); // same as above
            }
            else { center.Y = pos.Y; }
            transform = Matrix.CreateTranslation(new Vector3(-center.X + (viewport.Width / 2), //moves the camera
            -center.Y + (viewport.Height / 2), 0));
                
         }
    }
}
