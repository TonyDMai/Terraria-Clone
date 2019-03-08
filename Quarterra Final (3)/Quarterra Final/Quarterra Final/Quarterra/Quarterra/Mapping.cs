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
    class Mapping
    {
        private List<Blocks> allblocks = new List<Blocks>(); // a list for all the block types availible
        private int width, height; // represents the number of tiles in the width and height
        public List<Blocks> AllBlocks // gets and returns all the blocks
        {
            get { return allblocks; }
            set { allblocks = value; }


        }
        public int Width // gets and returns the width of eachblock
        {
            get { return width; }
            set { width = value; }

        }
        public int Height // gets and returns the height of the block
        {
            get { return height; }
            set { height = value; }
        }
        public Mapping() { }

        public void GenerateMap(int[,] MapDta, int size)
        {
            for (int x = 0; x < MapDta.GetLength(1); x++)
            {
                for (int y = 0; y < MapDta.GetLength(0); y++)
                {
                    int num = MapDta[y, x]; // takes what evers numbers in the array and stores it in an variable

                    if (num > 0) // if num is not 0 then a block will be drawn
                    { allblocks.Add(new Collision(num, new Rectangle(x * size, y * size, size, size))); } // add a new block to the list
                    width = (x + 1) * size; //calculates the width 
                    height = (y + 1) * size; // calculates the height
                }

              
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Blocks tile in allblocks)
            {
                tile.Draw(spriteBatch); //draw all blocks in the array
            }

        }
    }
}

