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
    class EnemyManager
    {
        private Enemy[] EnemyList = new Enemy[10]; //creates an array of enemies
        public Vector2 knockback = new Vector2(40, -10); //knockback vector, how the enemy will react if hit
        public Random RNG = new Random();// random number generate for monster spawn
        private int width; // width of the enemy
        public EnemyManager() { } // null constructor
        public void update(GameTime gameTime,Player p1,Mapping map)
        {
            for (int I = 0; I < EnemyList.Length; I++)
            {

                EnemyList[I].Update(gameTime, p1, map); //updates the  enemys
                foreach (Blocks tile in map.AllBlocks)
                {
                    EnemyList[I].Collision(tile.BlockRekt, map.Height, map.Width); //checks collision of enemy
                    
                }
                MouseState mouse = Mouse.GetState(); // cehcks the status of mouse
                Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 10, 10); // updates mouse hitbox

                if (p1.swRectangle.Intersects(EnemyList[I].enemyRekt)) // if hitboxes collide
                {
                    EnemyList[I].health -= p1.dmg; // decreases enemy health
                    if (EnemyList[I].health <= 0)
                    {
                        EnemyList[I].pos = new Vector2(RNG.Next(0, width), 10); //respawns the enemy if its dead
                        EnemyList[I].health = 50; //resets its health
                    }
                    else
                    {
                        if (p1.faceright == true)
                        {
                           EnemyList[I].velocity.X = knockback.X; //applies knockback if enemy is hit
                        }
                        else if (p1.faceright == false)
                        {
                            EnemyList[I].velocity.X -= knockback.X; //applies knockback if enemy is hit
                        }
                        EnemyList[I].velocity.Y = knockback.Y; //knockback
                    }
                }
                if (EnemyList[I].enemyRekt.Intersects(p1.rect))
                {
                    p1.currenthealth -= 1; // decrease player health if the slime hits the player

                }
               
                
            }
        }
        public void Load(ContentManager Content,int wid)
        {
            this.width = wid; //sets width
            MouseState mouse = Mouse.GetState();//update the mouse state
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 10, 10); //updates the mouse hitbox

            for (int i = 0; i < EnemyList.Length; i++)
            {
              
                EnemyList[i] = new Enemy(); //creates a new enemy
                EnemyList[i].Load(Content); // loads the enemy sprite
                EnemyList[i].pos = new Vector2(RNG.Next(0,width),10); // updates the position of the enemy, spawns it at a random position
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int I = 0; I < EnemyList.Length; I++)
            {
                spriteBatch.Draw(EnemyList[I].texture, EnemyList[I].enemyRekt, EnemyList[I].color); //draws the enemy
            }
        }

    }
    class Enemy 
    {
       
        public Texture2D texture; // enemy image
 
        public Vector2 pos = new Vector2(0, 0); //default position of the slime
        public Vector2 velocity; //velocity of the slime
        public Color[] color1 = new Color[5] { Color.AliceBlue, Color.Violet, Color.Red, Color.Yellow, Color.Gainsboro }; // used for changing the color of the slime
        public Random RNG = new Random(); // random number generator
        public Color color; //color of the slime
        public Rectangle enemyRekt; // hitbox of the slime
        public int health = 50; // health of the slime
        bool jump = true; // slimes ability to jump

        public Enemy()
        {
            color = color1[RNG.Next(0,5)]; //changes the color of the slime 
        }
        public void Update(GameTime gameTime,Player p1, Mapping map)
        {
        
         
       
            pos += velocity; //updates the position of the slime
            enemyRekt = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height); //updates enemy hitbox
            Input(gameTime, p1); // moves the slime
            if (velocity.Y < 10) // 10 is terminal velocity
            { velocity.Y += 1f; }
           


        }
        public void Input(GameTime gameTime,Player p1)
        {

            if (p1.Pos.X > pos.X + (enemyRekt.Width/2))
            { velocity.X = 2;}  // moves right if player is to the right
            else
            { velocity.X = -2;} // otherwise move left
            if (jump) // changes position and velocity so that he can jump
            {
                velocity.Y = -14f;

                pos.Y -= 5f;

                jump = false;
            }
            MouseState mouse = Mouse.GetState(); // gets mouse state
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 10, 10); //updates the mouse hitboxx
          
           



        }
        public void Collision(Rectangle rectangle, int Xoff, int Yoff)
        {
            // checks collission with block types, prevents slime from falling through the world
            if (enemyRekt.TouchingTopOf(rectangle))
            {
                velocity.Y = 0f;
                enemyRekt.Y = rectangle.Y - enemyRekt.Height;

               
            }
            if (enemyRekt.TouchingLeftOf(rectangle))
            {
                pos.X = rectangle.X - enemyRekt.Width - 2;
                jump = true;
                
            }
            if (enemyRekt.TouchingRightOf(rectangle))
            {
                pos.X = rectangle.X + rectangle.Width + 2;
                jump = true;
            }
            if (enemyRekt.TouchingBottomOf(rectangle))
            {
                velocity.Y = 1f;
               
            }
            
          
        }
        public virtual void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Slime"); //loads the slime texture
        }

        public int changehealth // updates andd returns the health of the enemy
        {
            get { return health; }
            set { health += value; }
        }

    }
}
