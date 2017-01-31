using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace XNAGame
{
    /// <summary>
    /// This class represents every object that is used in the game.
    /// (The name object was taken, so I looked up a thesaurus entry for object)
    /// This class is abstract so that all the objects can be gathered more or less
    /// as one in the main class to be rendered and updated all together.
    /// It more or less makes for easier object management.
    /// 
    /// Most being's suffixes tell what they are labelled as:
    /// e: Enemy,
    /// eff: Effect,
    /// w: Weapon,
    /// i: Item
    /// </summary>
    public abstract class Being
    {
        /// <summary>
        /// The current position of this being.
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The current sprite being used for this being.
        /// </summary>
        protected Texture2D sprite;


        /// <summary>
        /// The movement to apply to this being just before drawing this being
        /// in pixels/sec.
        /// </summary>
        protected Vector2 movement = new Vector2(0, 0);

        /// <summary>
        /// The width of the sprite of this being
        /// </summary>
        public int width;
        /// <summary>
        /// The height of the sprite of this being
        /// </summary>
        public int height;

        /// <summary>
        /// The loader used for loading the images and textures
        /// </summary>
        public static ContentManager loader;

        /// <summary>
        /// The scrolling offset of the screen to allow larger stages to fit
        /// in the window.
        /// </summary>
        public static Vector2 scrollOffset;

        /// <summary>
        /// The gravity constant for beings that actually move.
        /// Equal to (0, 9.806).
        /// (We've gotta have realism somewhere, right?)
        /// </summary>
        public static Vector2 GRAVITY = new Vector2(0, 9.806f);

        /// <summary>
        /// Creates a new being. Adding this to the being List in the main
        /// class will allow it to be rendered and apply logic every frame.
        /// </summary>
        /// <param name="x">The x coordinate of this being</param>
        /// <param name="y">The y coordinate of this being</param>
        /// <param name="sprite">The sprite to use for this being</param>
        protected Being(int x, int y, Texture2D sprite)
        {
            this.position = new Vector2(x, y);
            this.sprite = sprite;
            this.width = sprite.Width;
            this.height = sprite.Height;
        }

        /// <summary>
        /// Creates a new being. Adding this to the being List in the main
        /// class will allow it to be rendered and apply logic every frame.
        /// </summary>
        /// <param name="x">The x coordinate of this being</param>
        /// <param name="y">The y coordinate of this being</param>
        /// <param name="spriteName">The location of the sprite in the content folder</param>
        protected Being(int x, int y, string spriteName)
        {
            this.position = new Vector2(x, y);
            this.sprite = loader.Load<Texture2D>("sprites\\" + spriteName);
            this.width = this.sprite.Width;
            this.height = this.sprite.Height;
        }

        /// <summary>
        /// This is called for this being every frame in order to perform various tasks based
        /// on events from the last frame.
        /// The abstract means that this method must be used in inheriting classes.
        /// </summary>
        /// <param name="delta">The time since the last frame in milliseconds.</param>
        public abstract void updateLogic(double delta);

        /// <summary>
        /// Moves the being based on the movement vector and the time between frames.
        /// </summary>
        /// <param name="delta">The time since the last frame in milliseconds.</param>
        public virtual void move(float delta)
        {
            position += (movement * delta) / 1000;
        }

        /// <summary>
        /// Draws the being if it is on screen. (Can be overridden)
        /// The virtual means that what happens here is a default, but
        /// can be overridden by other beings. (Such as drawing text instead of a sprite)
        /// </summary>
        /// <param name="sb">The SpriteBatch used for drawing the being.</param>
        public virtual void draw(SpriteBatch sb)
        {
            if (isOnScreen(position, width, height))
                sb.Draw(sprite, position + scrollOffset, Color.White);
        }

        /// <summary>
        /// Checks if a rectangle is within the window.
        /// </summary>
        /// <param name="pos">The position of the top left part of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        /// <returns>True if this rectangle is within the screen borders, false otherwise.</returns>
        public bool isOnScreen(Vector2 pos, int width, int height)
        {
            return new Rectangle((int)pos.X, (int)pos.Y, width, height).Intersects(
                new Rectangle((int)-scrollOffset.X, (int)-scrollOffset.Y,
                    Main.DisplayWidth - (int)scrollOffset.X, Main.DisplayHeight - (int)scrollOffset.Y));
        }

        /// <summary>
        /// Gets or sets this being's current position vector.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets or sets the current movement vector.
        /// </summary>
        public Vector2 Movement
        {
            get { return movement; }
            set { movement = value; }
            
        }
        
        /// <summary>
        /// Gets the rectangle that this being is located in. Typically used for collisions
        /// and such, hence the name "Hit box".
        /// </summary>
        public Rectangle HitBox
        {
            get { return new Rectangle((int)(position.X + scrollOffset.X), (int)(position.Y + scrollOffset.Y), width, height); }
        }

        /// <summary>
        /// Swaps the current sprite of this being with a new one,
        /// and replaces the one given with the one replaced.
        /// </summary>
        /// <param name="spriteToSwap">The sprite to swap the current one with.</param>
        public void swapSprites(ref Texture2D spriteToSwap)
        {
            Texture2D temp = this.sprite;
            this.sprite = spriteToSwap;
            spriteToSwap = temp;
        }
    }
}
