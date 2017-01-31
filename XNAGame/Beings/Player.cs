using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNAGame.Beings;
using XNAGame.BeingTemplates;

namespace XNAGame
{
    /// <summary>
    /// This is the player. The very one that you control and do all the fun stuff with.
    /// </summary>
    public class Player : Being
    {
        /// <summary>
        /// The movement speed of this being.
        /// </summary>
        int moveSpeed = 200;

        /// <summary>
        /// The power of jumping for the player.
        /// </summary>
        int jumpPower = 400;

        /// <summary>
        /// Whether or not the player is on the ground.
        /// </summary>
        public bool isOnGround = true;

        /// <summary>
        /// The list of beings the player is potentially colliding with.
        /// </summary>
        List<Being> collisions = new List<Being>();

        //These are used for making sure that the player fires only once per click.
        //bool mouseDebounce = false;
        //bool isMouseLeftDown = false;

        //Determines what sprites to use for animations.
        bool isMoving = true;
        bool isGoingOtherWay = false;
        bool isInPit = false;

        /// <summary>
        /// Whether or not the player is in tutorial mode (in the first level, if
        /// you fall in the only pit, you get an otherwise hidden message).
        /// </summary>
        public static bool tutorialMode = false;

        /// <summary>
        /// The sound effect for jumping, a "clong". Because why not?
        /// </summary>
        SoundEffect jump = loader.Load<SoundEffect>("sfx\\jump");

        /// <summary>
        /// The sound effect for firing a weapon, ripping paper. Because why not?
        /// </summary>
        SoundEffect fireWeapon = loader.Load<SoundEffect>("sfx\\fire");

        /// <summary>
        /// The sound effect for hitting something, a scream.
        /// </summary>
        SoundEffect hitEnemy = loader.Load<SoundEffect>("sfx\\enemyCollision");

        /// <summary>
        /// The constant for when to go to the next frame (in ticks).
        /// </summary>
        long timeBtwnFrames = 2000000;

        /// <summary>
        /// The different sprites of the player walking.
        /// But it looks more like shuffling to me. Or maybe shimmying.
        /// </summary>
        Texture2D[] playerAnimations = new Texture2D[3];

        /// <summary>
        /// Creates a new player. (Parameterless) (I don't actually know why this is here.)
        /// </summary>
        public Player()
            : base(0, 0, "player_animStill")
        {
            for (int i = 0; i < 3; i++)
            {
                playerAnimations[i] = loader.Load<Texture2D>("player_anim" + (i + 1));
            }
        }

        /// <summary>
        /// Creates a new player.
        /// </summary>
        /// <param name="x">The starting x coordinate for the player.</param>
        /// <param name="y">The starting y coordinate for the player.</param>
        public Player(int x, int y)
            : base(x, y, Being.loader.Load<Texture2D>("sprites\\player_animStill"))
        {
            //Load all the walking textures.
            for (int i = 0; i < 3; i++)
            {
                playerAnimations[i] = loader.Load<Texture2D>("sprites\\player_anim" + (i + 1));
            }
        }

        public override void updateLogic(double delta)
        {
            //If on the ground, then reset the movement to zero.
            //Otherwise, just keep the y movement.
            if (!isOnGround)
                movement *= Vector2.UnitY;
            else
                movement = Vector2.Zero;

            //If the up key is pressed, and if on the ground, jump!
            if (Main.keyStates.IsKeyDown(Keys.W) || Main.keyStates.IsKeyDown(Keys.Up) || Main.keyStates.IsKeyDown(Keys.Space))
            {
                if (isOnGround)
                {
                    Movement += new Vector2(0, -jumpPower);
                    isOnGround = false;
                    jump.Play();
                }
            }
            //There's no need for a down now that gravity exists... That and using this tended to make the player fall out of the level.
            /*if (Main.keyStates.IsKeyDown(Keys.S) || Main.keyStates.IsKeyDown(Keys.Down))
            {
                Movement += new Vector2(0, moveSpeed);
                isOnGround = true;
            }//*/
            //Go to the left.
            if (Main.keyStates.IsKeyDown(Keys.A) || Main.keyStates.IsKeyDown(Keys.Left))
            {
                Movement += new Vector2(-moveSpeed, 0);
                isGoingOtherWay = true;
            }
            //Go to the right.
            if (Main.keyStates.IsKeyDown(Keys.D) || Main.keyStates.IsKeyDown(Keys.Right))
            {
                Movement += new Vector2(moveSpeed, 0);
                isGoingOtherWay = false;
            }

            if (Main.keyStates.IsKeyDown(Keys.T))
            {
                Main.addBeing(new eff_fly((int)Position.X-10, (int)Position.Y-10, "Testing the flying thingamajig!"));
            }

            //And if not on the ground, add some gravity.
            if (!isOnGround)
            {
                 Movement += GRAVITY;
            }

            //Mouse clicking (Firing weapons)
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                //This is to prevent weapons from forever spawning if the button is held down.
                /*if (!isMouseLeftDown)
                {
                    mouseDebounce = true;
                    isMouseLeftDown = true;
                }
                else
                    mouseDebounce = false;*/

                //Now here's where the firing happens.
                if (true)//mouseDebounce)
                {
                    //Play the SFX
                    fireWeapon.Play();
                    //Yell "Fire!"
                    Main.addBeing(new eff_fly((int)position.X, (int)position.Y, Text.getMsg("general", "playerComment_fireWeapon")));
                    //And fire each of the weapons you have.
                    foreach (Weapon w in Main.curSaveData.weapons)
                    {
                        w.fire((int)Position.X + 10, (int)Position.Y + 10, Mouse.GetState().X, Mouse.GetState().Y);
                    }
                }
            }
            else
            {
                //isMouseLeftDown = false;
            }

            //And finally, check if you're on the ground.
            checkIfOnGround();
        }

        /// <summary>
        /// Checks if the player is on the ground or not based on the Main.level int array map, and being collisions.
        /// </summary>
        private void checkIfOnGround()
        {
            collisions = Collision.getPossibleCollisions(this);
            if (collisions.Count != 0)
            {
                //string collisionNames = "Player Collisions:\n";
                foreach (Being b in collisions)
                {
                    //collisionNames += b.ToString() + "\n";
                    if (b is Wall)
                    {
                        if (Collision.getCollisionDir(this, b) == Collision.UP)
                        {
                            isOnGround = true;
                        }
                    }
                }
                //Text.displayMessage(collisionNames, 40, 100, 1, 1, 0, 1, 1, Text.WAVE_yonly);
            }
            else
                isOnGround = false;
            try
            {
                if (Main.level.levelMap[(int)(this.Position.X / Main.TileSize), (int)(this.Position.Y / Main.TileSize) + 1] == Level.WALL ||
                    Main.level.levelMap[(int)(this.Position.X / Main.TileSize) + 1, (int)(this.Position.Y / Main.TileSize) + 1] == Level.WALL)
                {
                    isOnGround = true;
                }
            }
            catch (IndexOutOfRangeException)
            {
                //If the position is outside of the map, then the player must have fallen into a pit.
                Movement = Vector2.Zero;
                isInPit = true;
                if (tutorialMode)
                {
                    Main.displayDialogue(Text.getMsg("general", "exception_tut_playerOutOfBounds"), onEnemyCollision);
                    hitEnemy.Play();
                }
                else
                {
                    Main.displayDialogue(Text.getMsg("general", "exception_playerOutOfBounds"), onEnemyCollision);
                    hitEnemy.Play();
                }
            }
        }

        /// <summary>
        /// Gives a weapon to the player to be used to add to the firing list.
        /// </summary>
        /// <param name="w">The weapon to add.</param>
        public void addWeapon(Weapon w)
        {
            Main.curSaveData.weapons.Add(w);
        }

        /// <summary>
        /// For dialogue box. Goes on to next level.
        /// </summary>
        /// <returns>True always. Because void was unacceptable.</returns>
        private bool onLevelCompleteDialogueOK_toNextLevel()
        {
            Main.level.curLevel++;
            Main.resetLevel();
            Main.curSaveData.curLevel = Main.level.curLevel;
            return true;
        }

        /// <summary>
        /// For dialogue box. Returns to main menu.
        /// </summary>
        /// <returns>True always.</returns>
        private bool onLevelCompleteDialogueOK_finishedGame()
        {
            Main.curGameState = Main.state_StartMenu;
            return true;
        }

        /// <summary>
        /// For dialogue box. Restarts the level.
        /// </summary>
        /// <returns>True always.</returns>
        private bool onEnemyCollision()
        {
            Main.resetLevel();
            Main.curSaveData.curScore -= 100;
            return true;
        }

        public override void move(float delta)
        {
            //Move
            base.move(delta);

            //Now check what to do based on the new position.
            if (collisions.Count != 0)
            {
                foreach (Being colliding in collisions)
                {
                    if (colliding is Wall)
                    {
                        if (Collision.getCollisionDir(this, colliding) == Collision.UP)
                        {
                            position.Y -= position.Y % (float)Main.TileSize;
                        }
                        if (Collision.getCollisionDir(this, colliding) == Collision.DOWN)
                        {
                            position.Y -= position.Y % (float)Main.TileSize - (float)Main.TileSize;
                            Movement = Vector2.Zero;
                        }
                        if (Collision.getCollisionDir(this, colliding) == Collision.LEFT)
                        {
                            position.X -= position.X % (float)Main.TileSize;
                        }
                        if (Collision.getCollisionDir(this, colliding) == Collision.RIGHT)
                        {
                            position.X -= position.X % (float)Main.TileSize - (float)Main.TileSize;
                        }
                    }

                    //Hit an enemy, now start over, with a 100 point penalty.
                    if (colliding is Enemy)
                    {
                        Main.displayDialogue(Text.getMsg("general", "player_death"), onEnemyCollision);
                        hitEnemy.Play();
                    }

                    //Take the item and execute it's effect
                    if (colliding is Item)
                    {
                        ((Item)colliding).takeEffect();
                        Main.removeBeing(colliding);
                    }

                    //If a next level exists, go on to the next level, otherwise return to main menu.
                    if (colliding is FinishLine)
                    {
                        if (System.IO.File.Exists("Resources\\LevelData\\" + (Main.level.curLevel + 1) + ".txt"))
                        {
                            Main.displayDialogue(Text.getMsg("general", "levelFinished"), onLevelCompleteDialogueOK_toNextLevel);
                            tutorialMode = false;
                        }
                        else
                        {
                            Main.displayDialogue(Text.getMsg("general", "levelFinished_finalLvl"), onLevelCompleteDialogueOK_finishedGame);
                            tutorialMode = false;
                            Main.curSaveData.curLevel = 1;
                        }
                    }
                }
            }

            //Wall detection
            if (Main.level.spaceContains((int)(this.Position.X), (int)this.Position.Y, Collision.DOWN) == Level.WALL)
            {
                position.Y -= position.Y % (float)Main.TileSize;
            }
            if (Main.level.spaceContains((int)this.Position.X, (int)this.Position.Y, Collision.UP) == Level.WALL)
            {
                position.Y -= position.Y % (float)Main.TileSize - Main.TileSize;
                Movement = Vector2.Zero;
            }
            if (Main.level.spaceContains((int)this.Position.X, (int)this.Position.Y, Collision.RIGHT) == Level.WALL)
            {
                position.X -= position.X % (float)Main.TileSize;
            }
            if (Main.level.spaceContains((int)this.Position.X, (int)this.Position.Y, Collision.LEFT) == Level.WALL)
            {
                position.X -= position.X % (float)Main.TileSize - Main.TileSize;
            }

            //Animate if moving.
            if (movement != Vector2.Zero && !isMoving)
            {
                isMoving = true;
            }
            else if (movement == Vector2.Zero)
            {
                isMoving = false;
            }

            //Scroll the scrollOffset vector if close to the borders of the window
            if (position.X - Main.DisplayWidth + 250 > -scrollOffset.X)
                scrollOffset.X = (int)-(position.X - Main.DisplayWidth + 250);
            if (position.X - 250 < -scrollOffset.X)
                scrollOffset.X = (int)-(position.X - 250);
            if (position.Y - Main.DisplayHeight + 200 > -scrollOffset.Y)
                scrollOffset.Y = (int)-(position.Y - Main.DisplayHeight + 200);
            if (position.Y - 200 < -scrollOffset.Y)
                scrollOffset.Y = (int)-(position.Y - 200);

            //For preventing the scrollOffset from going beyond the level boundaries.
            if (scrollOffset.X < -(Main.level.levelMap.GetUpperBound(0) + 1) * Main.TileSize + Main.DisplayWidth)
                scrollOffset.X = -(Main.level.levelMap.GetUpperBound(0) + 1) * Main.TileSize + Main.DisplayWidth;
            if (scrollOffset.Y < -(Main.level.levelMap.GetUpperBound(1) + 1) * Main.TileSize + Main.DisplayHeight)
                scrollOffset.Y = -(Main.level.levelMap.GetUpperBound(1) + 1) * Main.TileSize + Main.DisplayHeight;
            if (scrollOffset.X > 0)
                scrollOffset.X = 0;
            if (scrollOffset.Y > 0)
                scrollOffset.Y = 0;
        }

        public override void draw(SpriteBatch sb)
        {
            //This is a bunch of debugging stuff from pretty much the start of the creation of this.
            //I still find it useful, so I'm leaving it in here.
            /*if (Main.keyStates.IsKeyDown(Keys.W) || Main.keyStates.IsKeyDown(Keys.Up))
            {
                Text.displayMessage("Up!", 50, 30, 255, 255, 255, 255, 1, Text.NONE);
            }
            if (Main.keyStates.IsKeyDown(Keys.S) || Main.keyStates.IsKeyDown(Keys.Down))
            {
                Text.displayMessage("Down!", 50, 50, 255, 255, 255, 255, 1, Text.NONE);
            }
            if (Main.keyStates.IsKeyDown(Keys.A) || Main.keyStates.IsKeyDown(Keys.Left))
            {
                Text.displayMessage("Left!", 50, 90, 255, 255, 255, 255, 1, Text.NONE);
            }
            if (Main.keyStates.IsKeyDown(Keys.D) || Main.keyStates.IsKeyDown(Keys.Right))
            {
                Text.displayMessage("Right!", 50, 70, 255, 255, 255, 255, 1, Text.NONE);
            } */
            //Text.displayMessage("Player Positon:\nX: " + (int)Position.X + "\nY: " + (int)Position.Y, Main.DisplayWidth / 2, Main.DisplayHeight / 2, 1, 1, 1, 1, 1, Text.NONE);
            //Text.displayMessage("Scroll Offset:\nX: " + (int)scrollOffset.X + "\nY: " + (int)scrollOffset.Y, Main.DisplayWidth / 2 + 100, Main.DisplayHeight / 2 + 25, 1, 1, 1, 1, 1, Text.NONE);
            //Text.displayMessage("Player Movement:\nX: " + (int)Movement.X + "\nY: " + (int)Movement.Y + "\nOn ground?: " + isOnGround.ToString(), Main.DisplayWidth / 2 + 100, Main.DisplayHeight / 2 + 25, 1, 1, 1, 1, 1, Text.NONE);
            
            //string toDisplayWallBox = "PLAYER RECTANGLE\nTop: " + collisionBox.Top + "\nBottom: " +
                //collisionBox.Bottom + "\nLeft: " + collisionBox.Left + "\nRight: " + collisionBox.Right;
            //Text.displayMessage(toDisplayWallBox, 0, 400, 0, 255, 0, 255, 1, Text.NONE);

            //If it is in a pit, then don't draw a sprite (it is figuratively in the pit, so you wouldn't be able to see it)
            if (!isInPit)
            {
                if (isMoving)
                {
                    //Animating the movements.
                    if (isGoingOtherWay)
                    {
                        //Reflect the sprite if going the other way.
                        sb.Draw(playerAnimations[(int)((DateTime.Now.Ticks / timeBtwnFrames) % 3)], new Rectangle((int)(Position.X + scrollOffset.X), (int)(Position.Y + scrollOffset.Y), this.width, this.height), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                    {
                        sb.Draw(playerAnimations[(int)((DateTime.Now.Ticks / timeBtwnFrames) % 3)], new Rectangle((int)(Position.X + scrollOffset.X), (int)(Position.Y + scrollOffset.Y), this.width, this.height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
                else
                {
                    //Stationary.
                    if (isGoingOtherWay)
                    {
                        //Reflect the sprite if going the other way.
                        sb.Draw(this.sprite, new Rectangle((int)(Position.X + scrollOffset.X), (int)(Position.Y + scrollOffset.Y), this.width, this.height), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                    {
                        sb.Draw(this.sprite, new Rectangle((int)(Position.X + scrollOffset.X), (int)(Position.Y + scrollOffset.Y), this.width, this.height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}
