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
    class Blocks
    {
        protected Texture2D BlockSprite; // texture of the block
        protected Rectangle RektBlock; //hitbox of the box
        public Rectangle BlockRekt // returns and sets the hitbox of the box
        {
            get { return RektBlock; }
            set { RektBlock = value; }

        }

        private static ContentManager content;

        public static ContentManager Content //loads the content
        {
            get { return content; }
            set { content = value; }

        }

        public void Draw(SpriteBatch spriteBatch) //draws the block
        {
            spriteBatch.Draw(BlockSprite, BlockRekt, Color.White);

        }

        
            
    }

    class Collision : Blocks
    {
        public Collision(int i ,Rectangle Rekt)
        {
            BlockSprite = Content.Load<Texture2D>("Texture" + i);
            this.RektBlock = Rekt;
        }


    }
}
