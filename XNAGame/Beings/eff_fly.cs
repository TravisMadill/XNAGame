using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGame.Beings
{
    /// <summary>
    /// This being is kind of like a particle effect, where something flies off something and disappears.
    /// This is essentially the same thing, but with text and sprites.
    /// </summary>
    class eff_fly : Being
    {
        /// <summary>
        /// Whether or not this fly effect will display text or a sprite.
        /// </summary>
        bool displayingText;

        /// <summary>
        /// The string to display (if applicable).
        /// </summary>
        string toDisplay_msg;

        /// <summary>
        /// The starting time this being was created.
        /// </summary>
        long curTicks;

        /// <summary>
        /// Depending on the size of the string being displayed, display the message for longer.
        /// </summary>
        bool displayLonger = false;

        /// <summary>
        /// Creates a new instance of the fly effect with a message string.
        /// </summary>
        /// <param name="x">The x coordinate to fly from.</param>
        /// <param name="y">The y coordinate to fly from.</param>
        /// <param name="msgToDisplay">The string to display. The length of this string determines how long this stays on screen.</param>
        public eff_fly(int x, int y, string msgToDisplay)
            : base(x, y, "nothing")
        {
            displayingText = true;
            toDisplay_msg = msgToDisplay;
            //This was determined more-or-less through trial and error.
            Movement = new Vector2(-100, -100);
            curTicks = DateTime.Now.Ticks;
            if (msgToDisplay.Length > 30)
                displayLonger = true;
        }

        /// <summary>
        /// Creates a new instance of the fly effect with a sprite texture, like a spark.
        /// </summary>
        /// <param name="x">The x coordinate to fly from.</param>
        /// <param name="y">The y coordinate to fly from.</param>
        /// <param name="spriteToDisplay">The texture to display.</param>
        public eff_fly(int x, int y, Texture2D spriteToDisplay)
            : base(x, y, spriteToDisplay) //Sets the being's sprite to make things somewhat simpler
        {
            displayingText = false;
            Movement = new Vector2(-100, -100);
            Position -= new Vector2(spriteToDisplay.Width, spriteToDisplay.Height);
        }

        public override void updateLogic(double delta)
        {
            if (displayLonger)
            {
                if (DateTime.Now.Ticks - curTicks > 20000000L) //Once 2 seconds has passed, disappear it.
                    Main.removeBeing(this);
            }
            else
            {
                if (movement.X < 10 && movement.X > -10 && movement.Y < 10 && movement.Y > -10)
                {
                    Main.removeBeing(this); //Otherwise, once the being has stopped, disappear it.
                }
            }

            //Slowing down the movement by 5 seemed reasonable.
            if (movement.X > 0)
                movement.X -= 5;
            else
                movement.X += 5;
            if (movement.Y > 0)
                movement.Y -= 5;
            else
                movement.Y += 5;
        }

        public override void draw(SpriteBatch sb)
        {
            //Displaying the actual text at an angle was something not something I put in the Text class, since that would involve more trig than I'd like, and the text doesn't need to be animated anyway.
            if (displayingText && Position.Y + scrollOffset.Y > 200)
            {
                //Because of the background colours, the colour needed to change to something more readable if the effect was starting in the sky region background (since that's the only background I had)
                sb.DrawString(Text.font, toDisplay_msg, Position + scrollOffset, Color.White, -0.3f, Text.font.MeasureString(toDisplay_msg)/2, 0.8f, SpriteEffects.None, 0);
            }
            else if (displayingText)
            {
                sb.DrawString(Text.font, toDisplay_msg, Position + scrollOffset, Color.Gray, -0.3f, Text.font.MeasureString(toDisplay_msg) / 2, 0.8f, SpriteEffects.None, 0);
            }
            else //But otherwise, draw the sprite if text isn't being shown.
                base.draw(sb);
        }
    }
}
