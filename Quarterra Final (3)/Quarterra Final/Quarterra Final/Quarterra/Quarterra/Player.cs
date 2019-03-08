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
    class Player
    {
        public int currenthealth = 100; // player attribute, current hp
        public int maxhealth = 100; //player attribute, max hp
        public int dmg = 10; // player attributee, damage
        int timer = 0; // timer for player regen
        private Texture2D texture; // player sprite
        private Vector2 pos = new Vector2(0, 0); // position vector of the player on the map
        private Vector2 velocity; // player vector for their velocity
        private Rectangle playerRekt; // player hitbox
        private bool Jump = false; //statement for whether the player has jumped or not
        private Rectangle kappa; // idk mwhat this is for what mitchel coded
        private int count = 0; // int value used for animating the players walking animation
        float elapsed; // checks how much time has elapsed
        float delay = 200f; // this is a delay >_> kinda obvious but
        public bool leftright = true; // used in walking animation
        public bool faceright = true; //checks what directtion player is walking int

        public Rectangle swRectangle; //hitbox of the sword
        public Texture2D swTexture; // image of the sword
        Vector2 swOrigin; // point of origin of the sword
        Vector2 swPosition; // original sword position
        float rotation = 2.3f; // controls what dirction the sword is facing and the rotation

        public Vector2 Pos
        {
            get { return pos; } // returns the position of the player

        }

        public Player() //null constructor of the player class
        {
        }
        public void Load(ContentManager Content, string[] data)
        {

            texture = Content.Load<Texture2D>("Player"); //loads playeer sprite from file
            swTexture = Content.Load<Texture2D>("betasword"); // loads sword texture from file
            currenthealth = int.Parse(data[0]); // sets the current health of the player 


        }
        public void Update(GameTime gameTime, MouseState mouse)
        {
            timer++; // timer used for player regen, timer is incremented
            if (timer == 100)
            {
                if (currenthealth < maxhealth)
                {
                    currenthealth++; // heals player, increments the player helth
                }
                timer = 0; // resets the timer
                
            }
            pos += velocity; // updates player position
            playerRekt = new Rectangle((int)pos.X, (int)pos.Y, texture.Width / 6, texture.Height); // updates the player hitbox to the new position

            Input(gameTime); // calls the player movement method
            if (velocity.Y < 10) // 10 is terminal velocity
            { velocity.Y += 1f; } // adds float 1 to the y velocity

            swPosition = new Vector2(pos.X + 25, pos.Y + 50); // updates sword position
            swRectangle = new Rectangle((int)swPosition.X, (int)swPosition.Y, (int)swTexture.Width, (int)swTexture.Height); //updates sword hitbox
            swOrigin = new Vector2(swRectangle.Width, swRectangle.Height); // changes the origin of the sword

            if (mouse.LeftButton == ButtonState.Pressed) // update attack animation
            {

                if (faceright == true)
                {
                  //  rotation = 2f;
                    swPosition.X += 10; // changes sword position
                    swRectangle.X += 20; // changes sword hitbox

                }
                else if (faceright == false)
                {
                //    rotation = -0.7f;
                    swPosition.X -= 10;
                    swRectangle.X -= 20;                
                }
            }
        }
        private void Input(GameTime gameTime)
        {

            /*(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;*/

            /*
             * The following code is for player movement and player movement animation, changes the sword rotation
             */
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            { velocity.X = 5; rotation = 2.3f; if (leftright) { } else { leftright = true; count = 0; } faceright = true; } // if computer is lagging, player should move at same speed
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            { velocity.X = -5; rotation = -0.7f; if (leftright) { elapsed = 0; leftright = false; count = 3; } faceright = false; }
            else { velocity.X = 0; }

           

            if (elapsed >= delay && leftright == false) { if (count >= 5 || count < 3) { count = 3; } else { count++; } elapsed = 0; }
            if (elapsed >= delay && leftright == true) { if (count >= 2) { count = 0; } else { count++; } elapsed = 0; }
            kappa = new Rectangle(45 * count, 0, 45, 75);
            if (velocity.X == 0 && leftright)
            { kappa = new Rectangle(45, 0, 45, 75); }
            else if (velocity.X == 0) { kappa = new Rectangle(180, 0, 45, 75); }
            else { elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds; }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && Jump == false) // jumping animation
            {
                velocity.Y = -14f; //changes velocity for running
                pos.Y -= 5f; //changes position
                Jump = true; //sets jump to true so that player can only jump once til he hits the floor
            }
        }
        public void Collision(Rectangle rectangle, int Xoff, int Yoff)
        {
            /*
             * checks for collision by checking each side of the rectangle
             */
            if (playerRekt.TouchingTopOf(rectangle)) 
            {
                velocity.Y = 0f;
                playerRekt.Y = rectangle.Y - playerRekt.Height;

                Jump = false; // sets jump to false so player can jump again
            }
            if (playerRekt.TouchingLeftOf(rectangle))
            {
                pos.X = rectangle.X - playerRekt.Width - 2;

            }
            if (playerRekt.TouchingRightOf(rectangle))
            {
                pos.X = rectangle.X + rectangle.Width + 2;
            }
            if (playerRekt.TouchingBottomOf(rectangle))
            {
                velocity.Y = 1f;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, playerRekt, kappa, Color.White); //draws the player
            spriteBatch.Draw(swTexture, swPosition, null, Color.White, rotation, swOrigin, 4f, SpriteEffects.None, 0);
            // draws the sword
        }

        public Rectangle rect
        {
            get { return playerRekt; } // obtains player hitbox
        }

        public string hpcheck() //checks hp to see if u died
        {
            if (currenthealth <= 0)
            {
                pos.Y += 75;
                return "You have died";
            }
            else
                return currenthealth.ToString() + "/" + maxhealth.ToString(); //displays current hp

        }
        string[] save = new string[3]; // array for savin player data
        public void savedata()
        {  // stores the player data in an array so that it can be written into a file
            save[0] = currenthealth.ToString(); 
            save[1] = pos.X.ToString();
            save[2] = pos.Y.ToString();

            System.IO.File.WriteAllLines("Save.txt", save); // saves the data into a file
        }

        public float setposx //obtains and sets th x position of player
        {
            get { return pos.X; }
            set { pos.X = value; }
        }
        public float setposy //obtains and sets the y position of player
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }
    }
}

