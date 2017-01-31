using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNAGame
{
    /// <summary>
    /// This is the main class that controls everything in regards to the game.
    /// It handles all beings, the level data, and the save data.
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// The title of the game that will be displayed in the title bar.
        /// (Creative name, I know, but I couldn't think of anything snazzy.)
        /// </summary>
        string gameTitle = "Run & Shoot Things!";

        /// <summary>
        /// The current state of the keyboard to be used by beings that accept keyboard input,
        /// so the state can be unified between all beings and to ensure no conflicts occur.
        /// </summary>
        public static KeyboardState keyStates;
        /// <summary>
        /// The level data currently being used.
        /// </summary>
        public static Level level = new Level();
        /// <summary>
        /// The current state of the game, be it a menu or the gameplay itself.
        /// </summary>
        public static int curGameState = state_StartMenu;
        /// <summary>
        /// Constants set to the window dimensions of the window.
        /// </summary>
        public static int DisplayWidth, DisplayHeight;
        /// <summary>
        /// The current player information. Can be used by any being that needs player information
        /// to react to something.
        /// </summary>
        public static Player player;
        /// <summary>
        /// The structure that is used to save the game.
        /// </summary>
        public static SaveData.SaveStructure curSaveData;

        /// <summary>
        /// The list of beings that will be rendered and drawn.
        /// </summary>
        public static List<Being> beings = new List<Being>();
        /// <summary>
        /// The list of beings to add after updating is done, so as to not modify
        /// the list while it is being used.
        /// </summary>
        static List<Being> toAddList = new List<Being>();
        /// <summary>
        /// The list of beings to remove after updating is done, so as to not modify
        /// the list while it is being used.
        /// </summary>
        static List<Being> toRemoveList = new List<Being>();

        /// <summary>
        /// The constant that reflects that the current game state is in game mode.
        /// </summary>
        public const int state_Game = 0;
        /// <summary>
        /// The constant that reflects that the current game state is in the main menu.
        /// </summary>
        public const int state_StartMenu = 1;
        /// <summary>
        /// The constant that reflects that the current game state is in the pause menu.
        /// </summary>
        public const int state_PauseMenu = 2;
        /// <summary>
        /// The size of the tiles in this game. (40 pixels by 40 pixels)
        /// </summary>
        public const int TileSize = 40;

        /// <summary>
        /// Whether or not a dialogue box is up, and this determines whether or not to continue updating.
        /// (Being forced to read something and having enemies run into you would hardly be fair)
        /// </summary>
        public static bool dialogueIsUp = false;

        /// <summary>
        /// A button that doesn't do anything. It's just used for button width and height calculations.
        /// </summary>
        public static Button genericButton;
        /// <summary>
        /// The button that is used for starting the game.
        /// </summary>
        public Button startGame;
        /// <summary>
        /// The button that is used for loading a saved game.
        /// </summary>
        public Button loadGame;
        /// <summary>
        /// The button that is used for saving a game.
        /// </summary>
        public Button saveGame;
        /// <summary>
        /// The button that is used for exiting the game.
        /// </summary>
        public Button exitGame;
        /// <summary>
        /// The button that is used for resuming gameplay.
        /// </summary>
        public Button unpauseGame;
        /// <summary>
        /// The button that is used for returning to the main menu.
        /// </summary>
        public Button returnToMainMenu;

        /// <summary>
        /// The background music. Wind blowing through some trees with birds chirping.
        /// Quite soothing, actually.
        /// </summary>
        //public Song bgNoise;

        /// <summary>
        /// The text that is rendered in the menus to reflect certain events.
        /// </summary>
        public string menu_StatusText = Text.getMsg("general", "menuStatus_start");

        /// <summary>
        /// This game's logo.
        /// </summary>
        public Texture2D logo;


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.Window.Title = gameTitle;
            DisplayWidth = Window.ClientBounds.Width; // 800 pixels
            DisplayHeight = Window.ClientBounds.Height; // 600 pixels

            //Sets the default framerate
            TargetElapsedTime = new TimeSpan(10000000L / 60L);

            //You'd be surprised how long it took me to finally set this...
            //Anyway, makes the mouse not invisible (which is an annoying thing that's set to false by default)
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loads the main font (Buxton Sketch, if you're curious) (I just saw it in my fonts and thought it looked kind of neat)
            Text.font = this.Content.Load<SpriteFont>("fonts\\mainFont");

            //Load the game's logo
            logo = this.Content.Load<Texture2D>("sprites\\logo");

            //Loads and then plays the wind sounds to be played.
            //bgNoise = this.Content.Load<Song>("sfx\\wind");
            //MediaPlayer.Play(bgNoise);

            //This allows the ContentManager to be available from anywhere needed
            Being.loader = this.Content;

            //Initialise the save data
            SaveData.newSave = new SaveData.SaveStructure { curLevel = 1, curScore = 0, weapons = new List<XNAGame.BeingTemplates.Weapon>() { new Beings.w_rocket() } };
            curSaveData = SaveData.newSave;

            //Finally, create the buttons and their positions. The positions were all determined by trial and error.
            genericButton = new Button(0, 0, "Null");
            startGame = new Button(DisplayWidth / 2 - genericButton.width / 2, DisplayHeight / 2 - genericButton.height / 2 - 50, Text.getMsg("general", "btn_startGame"));
            loadGame = new Button(DisplayWidth / 2 - genericButton.width / 2, DisplayHeight / 2 - genericButton.height / 2, Text.getMsg("general", "btn_loadGame"));
            saveGame = new Button(DisplayWidth / 2 - genericButton.width * 3 / 2, DisplayHeight / 2 - genericButton.height / 2, Text.getMsg("general", "btn_saveGame"));
            exitGame = new Button(DisplayWidth / 2 - genericButton.width / 2, DisplayHeight / 2 + 30, Text.getMsg("general", "btn_exitGame"));
            unpauseGame = new Button(DisplayWidth / 2 - genericButton.width / 2, DisplayHeight / 2 - genericButton.height - 30, Text.getMsg("general", "btn_resumeGame"));
            returnToMainMenu = new Button(DisplayWidth / 2 + genericButton.width / 2, DisplayHeight / 2 - genericButton.height / 2, Text.getMsg("general", "btn_returnToMenu"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                MediaPlayer.Stop();
                this.Exit();
            }
            //I added this for convenience 
            if (keyStates.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                this.Exit();
            }

            //Update the current keyboard state
            keyStates = Keyboard.GetState();

            //If a dialogue box is up, allow it to do its stuff overriding everything to give this priority.
            if (dialogueIsUp)
            {
                foreach (Being b in beings)
                {
                    if (b is DialogueBox)
                    {
                        b.updateLogic(0);
                        break; //Once it comes up, there's no need to stay in this loop.
                    }
                }
            }
            else //Everything else is just a big switch statement
            {
                switch (curGameState)
                {
                    case state_StartMenu:
                        startGame.updateLogic(0);
                        loadGame.updateLogic(0);
                        exitGame.updateLogic(0);
                        //I think that this stuff is relatively self-explanitory
                        if (startGame.wasPressed())
                        {
                            level.loadLevel(curSaveData.curLevel);
                            curGameState = state_Game;
                        }
                        if (loadGame.wasPressed())
                        {
                            curSaveData = SaveData.loadGame();
                            if (curSaveData.Equals(SaveData.newSave))
                            {
                                menu_StatusText = Text.getMsg("general", "menuStatus_loadError");
                            }
                            else
                            {
                                level.loadLevel(curSaveData.curLevel);
                                curGameState = state_Game;
                                menu_StatusText = "";
                            }
                        }
                        if (exitGame.wasPressed())
                        {
                            this.Exit();
                        }
                        Text.displayMessage(menu_StatusText, 200, 500, 1, 1, 1, 1, 1, Text.NONE);
                        break;
                    case state_PauseMenu:
                        unpauseGame.updateLogic(0);
                        saveGame.updateLogic(0);
                        exitGame.updateLogic(0);
                        returnToMainMenu.updateLogic(0);
                        //I think that this stuff is relatively self-explanitory
                        if (unpauseGame.wasPressed())
                        {
                            curGameState = state_Game;
                            menu_StatusText = "";
                        }
                        if (saveGame.wasPressed())
                        {
                            menu_StatusText = SaveData.saveGame(curSaveData);
                        }
                        if (exitGame.wasPressed())
                        {
                            this.Exit();
                        }
                        if (returnToMainMenu.wasPressed())
                        {
                            curGameState = state_StartMenu;
                            menu_StatusText = "";
                        }
                        //Just for decoration's sake
                        Text.displayMessage("PAUSED", DisplayWidth / 2 - 60, DisplayHeight / 2 - 20, 1, 1, 1, 1, 2, Text.NONE);
                        Text.displayMessage(menu_StatusText, 200, 500, 1, 1, 1, 1, 1, Text.NONE);
                        break;
                    case state_Game:
                        //This was a convienient debugging feautre, and I decided to leave it in. Why not?
                        if (keyStates.IsKeyDown(Keys.R)) { resetLevel(); }
                        //To return to the menu
                        if (keyStates.IsKeyDown(Keys.Back)) { curGameState = state_PauseMenu; }

                        //Allows each and every being to do what they need to do this frame
                        foreach (Being b in beings) { b.updateLogic(gameTime.ElapsedGameTime.TotalMilliseconds); }
                        //...and changes their position based on their movement, too.
                        foreach (Being b in beings) { b.move((float)gameTime.ElapsedGameTime.TotalMilliseconds); }

                        //After performing logic, add beings that were requested to be added.
                        beings.AddRange(toAddList);
                        toAddList.Clear();//...and clear that list so they don't get added more than once.

                        //Remove beings that were requested to be removed.
                        if (toRemoveList.Count > 0) { foreach (Being b in toRemoveList) { beings.Remove(b); } }
                        toRemoveList.Clear();
                        break;
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //I decided to leave some old stuff for the sake of seeing the progress this went through.
            GraphicsDevice.Clear(Color.Black);

            //MouseState s = Mouse.GetState();
            spriteBatch.Begin(); //Prepare for drawing
            //Text.formatAndDisplayMessage(Text.getMsg("general", "annoying_Cursor"), s.X, s.Y); //Since I didn't know about making the mouse visible, this supplimented as my cursor until then
            //Text.formatAndDisplayMessage(Text.getMsg("general", "debug_testMsg"), 40, 40);

            switch (curGameState)
            { //Now draw everything that's needed for the player to see.
                case state_StartMenu:
                    spriteBatch.Draw(logo, new Vector2(175, 30), Color.White);
                    startGame.draw(spriteBatch);
                    loadGame.draw(spriteBatch);
                    exitGame.draw(spriteBatch);
                    break;
                case state_PauseMenu:
                    unpauseGame.draw(spriteBatch);
                    saveGame.draw(spriteBatch);
                    exitGame.draw(spriteBatch);
                    returnToMainMenu.draw(spriteBatch);
                    break;
                case state_Game:
                    //Draw the background starting from the edge of the screen to give the illusion of the background scrolling
                    for (int i = (int)Being.scrollOffset.X; i < DisplayWidth; i += level.background.Width)
                    {
                        spriteBatch.Draw(level.background, new Rectangle(i, 0, level.background.Width, DisplayHeight), Color.White);
                    }
                    //Then draw every being in the List.
                    foreach (Being b in beings)
                    {
                        b.draw(spriteBatch);
                    }

                    //Display a score counter
                    Text.displayMessage(Text.getMsg("general", "scoreCounter").Replace("%s", curSaveData.curScore.ToString()), DisplayWidth / 2 - 100, 40, 0, 0, 0, 1, 1, 0);
                    Text.displayMessage("Beings: " + beings.Count, 40, 60, 1, 1, 1, 1, 1, Text.NONE);
                    break;
            }

            //Draw all the text that was requested to be displayed
            Text.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// This allows a dialogue box to pop up and prompt the player about something.
        /// And you can send an entire method to execute once the OK button has been
        /// clicked. (Thank stackoverflow for that)
        /// </summary>
        /// <param name="msg">The message the dialogue box should display.</param>
        /// <param name="desiredActions">The actions to do when the OK button is clicked.</param>
        public static void displayDialogue(string msg, Func<bool> desiredActions)
        {
            dialogueIsUp = true;
            addBeing(new DialogueBox(msg, desiredActions));
        }

        /// <summary>
        /// Adds a being to be rendered and updated.
        /// </summary>
        /// <param name="b">The being to add</param>
        public static void addBeing(Being b)
        {
            toAddList.Add(b);
        }

        /// <summary>
        /// Removes a being from the main being list.
        /// </summary>
        /// <param name="b">The being to remove.</param>
        public static void removeBeing(Being b)
        {
            toRemoveList.Add(b);
        }

        /// <summary>
        /// Resets the level.
        /// </summary>
        public static void resetLevel()
        {
            beings.Clear();
            Main.player = null;
            level.levelMap = null;
            level.loadLevel(level.curLevel);
        }
    }
}
