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
   /*
    * Tony Doan, Vince Tesone, Mitchel Coakley
    * Title : Quarterra
    * Datw : June 24 , 2016
    * Purpose: This is a culmination of the stuff that we learned in class. This game (Terrarria alpha i guess) uses classes, drawing and arrays in order to achieve a basic gameplay for our game
     */
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /* Important Classes for each main component of the game */
        GraphicsDeviceManager graphics; // this controls the size of the game screen and  the resolution
        SpriteBatch spriteBatch; //thi0s controls how the p[rogram draws and displays the images on the screem
        SpriteFont playerfont; // This is a built in function that always text to be displayed on the screen
        Texture2D background; // Thiis is the background image for the world
        Rectangle backbounds; // this is boundingbox of the background image
        cButton btnplay; // this is class controls the spawning and clicking of buttons
        Texture2D Title; //This is the texture/ image of the title of the game
        Mapping map; //This is the class responsible for the generation of the map and world
        Player p1; // This is a class responsible for the motor controls of the player and attributes involved with the player
        Camera camera; // This is a class responsible for the centreing of the camera until you run into the edge of the world
        EnemyManager enemyM = new EnemyManager();// This is used as an array of enemies, it controls how the enemies spawn

        //sound 
        SoundEffect effect;


        bool reset = false; //This is a reset command when you load the game (helps prevents bugs)  
        string name; // this string is used to store one of the names thaat was randomly chosen from a sorted list
        static string hp; // this string is used to store and output the players new health

        static string[] data; // this array is used to store the data of the player when it is loaded from a file
        static string[] names; // This array is ued to store a list of names loaded from a file
        static string[] worldmap; // This array is used to store all the indexes and data used to generate the map
        static int[,] mapgen; // This double array is used to seperate the worldmap array into a grid so that a map can be generated
        enum GameState // this is used as various types of cases
        {   MainMenu, // This case is used so that the program starts off from a title screen
            Playing, // this case is used to switch the code over to the code for the actual game so that it wont be confused which code to run
        }       

        GameState CurrentGameState = GameState.MainMenu; // ets the current game mode to the mainmenu
        int screenWidth = 800, screenHeight = 600; // sets the parameters of the screen size
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this); // sets the value of the display to the screen that pops up
            Content.RootDirectory = "Content"; // Determines where all the files will be loaded from
        }

        protected override void Initialize()
        {
            map = new Mapping(); //initializes a new instance of the class for creating a map
            p1 = new Player(); // initializes a new instance of the class for creating the player and his weapon
            base.Initialize();
           

            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f); // checks how long it took to complete
        }

      
        protected override void LoadContent()
        {
            names = System.IO.File.ReadAllLines("names.txt"); // reads in the file and stores the text in a linear array
            Array.Sort(names); // sorts the array alphabetically
            Random rng = new Random(); // create a random number generator
            name = names[rng.Next(0, names.Length)]; // sets player name to be a random name chosen from the array
            reset = false; // sets reset to false.
            spriteBatch = new SpriteBatch(GraphicsDevice); // calls a new insatnce forsprite batch so that the program can draw images

            playerfont = Content.Load<SpriteFont>("health"); //loads ina spritetext file to be used for outputing text to an array
            Title = Content.Load<Texture2D>("Quarterra"); // loads in the title image from the content folder 
            background = Content.Load<Texture2D>("background"); //loads the background image in from the foler for the game
            
            graphics.PreferredBackBufferHeight = screenHeight; // sets the screen height to the value provided in the variable
            graphics.PreferredBackBufferWidth = screenWidth; // sets the screen width to the value provided in the variable
            graphics.ApplyChanges(); // applys new screen resolution

            IsMouseVisible = true; // allows the mouse cursor to be visible when inside the window

            effect = Content.Load<SoundEffect>("Run");

            backbounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); // sets a bounding box of the game to be that of the screen size
            btnplay = new cButton(Content.Load<Texture2D>("Button1"), graphics.GraphicsDevice); // creates a new start button with the image loaded from the content folder and using the graphics device as a bounding box
            btnplay.setPosition(new Vector2(350,300)); // changes the position of the button to the centre of the screen
            
            camera = new Camera(GraphicsDevice.Viewport); // creates a new camera instance so that the camera can scroll until the player reaches the end of the map
            worldmap = System.IO.File.ReadAllLines("world.txt"); // reads a file and stores all the map data to an array
            data = System.IO.File.ReadAllLines("save.txt"); //loads the player data from a file and stores it in an array

            mapgen = new int[worldmap.Length, worldmap[0].Length]; // creates a 2d array so that it can be passed into a the map gen class
            for (int j = 0; j < worldmap.Length; j++)
            {
                for (int i = 0; i < worldmap[j].Length; i++)
                {
                    try { mapgen[j, i] = int.Parse(worldmap[j].Substring(i, 1)); } catch { } // attempts to split up the old array into a 2d array so that the world can be generated
                }
            }
         
            Blocks.Content = Content; // sets the load section of blocks to the content foler

            map.GenerateMap(mapgen,50); // generates the map using the data in the array
            p1.Load(Content, data);// loads the player using the content data
             
            enemyM.Load(Content, map.Width); //loads the enemy using the content data
            
        }
     
        protected override void UnloadContent()
        {        
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                p1.savedata(); // saves the player data
                this.Exit(); //closes the game
            }
            MouseState mouse = Mouse.GetState(); //takes the current state of the mouse
           
            switch (CurrentGameState)
            {
                case GameState.MainMenu: // checks which mode the code should run
                        if (btnplay.isClicked == true)CurrentGameState = GameState.Playing; // changes the game mode
                        btnplay.Update(mouse); // updates the button press
                    
                    break;                    
                case GameState.Playing: //checks which game mode
                    if (reset == false) //checks if ame has been reseted yet or not
                    {
                        p1.setposx = int.Parse(data[1]); // sets player position to position when quitting
                        p1.setposy = int.Parse(data[2]);// sets player position to position when quitting
                        reset = true; // sets rest to true so that it doesnt bug
                    }
                        p1.Update(gameTime, mouse); // updates the player
                        enemyM.update(gameTime, p1, map); //update enemies on map      
                        break;                    
            }
                if (Keyboard.GetState().IsKeyDown(Keys.F11))
                {                graphics.ToggleFullScreen();                } // toggles fulscreen on press

            foreach (Blocks tile in map.AllBlocks)
            {
                p1.Collision(tile.BlockRekt, map.Height, map.Width); // checks block colision for every block
                camera.Update(p1.Pos, map.Width, map.Height); // updates the camera
            }
            hp = p1.hpcheck(); // updates the text above player head of the player health
          
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap; 
            base.Update(gameTime); //updates the game
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(); // starts drawing the game
            spriteBatch.Draw(background, backbounds, Color.White); // draws the background of the game
            spriteBatch.End(); // stops drawing
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend, //blends sprites together
                  SamplerState.PointClamp, 
     DepthStencilState.None,
    RasterizerState.CullCounterClockwise,
    null,
                camera.Transform); // uses camera as the scrolling point
            switch (CurrentGameState)
            {
                case GameState.MainMenu:

                    
                    btnplay.Draw(spriteBatch); // draws the button
                    var screenCenter = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2,GraphicsDevice.Viewport.Bounds.Height / 5); // sets the centre of screen
                    var textureCenter = new Vector2(Title.Width / 2,Title.Height / 2); // sets the centre of screen height
                    spriteBatch.Draw(Title, screenCenter, null, Color.White, 0f, textureCenter, 1f, SpriteEffects.None, 1f); //draws the title screen
                    spriteBatch.End(); // stops drawing
                    break;
                case GameState.Playing:
                    map.Draw(spriteBatch); //draws the map
                    p1.Draw(spriteBatch); //draws the player

                    spriteBatch.DrawString(playerfont, name + " " + hp, new Vector2(p1.rect.X, p1.rect.Y - 20), Color.White); // draws the updated player health
                    enemyM.Draw(spriteBatch); //draws the enemy
                    spriteBatch.End();                   
                    break;
            }                                 
            base.Draw(gameTime);
        }
    }
}
