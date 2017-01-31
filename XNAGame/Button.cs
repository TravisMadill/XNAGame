using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace XNAGame
{
    /// <summary>
    /// An extension of Being.
    /// Creates an object that reacts to mouse clicks within its specified region
    /// </summary>
    public class Button : Being
    {
        /// <summary>
        /// Whether or not this button is being pressed right now.
        /// </summary>
        protected bool pressed;

        /// <summary>
        /// The string to display inside this button.
        /// </summary>
        protected string btnText;

        /// <summary>
        /// A check for if the mouse button was pressed before released.
        /// (It'll make more sense in use)
        /// </summary>
        bool clickCheck;

        /// <summary>
        /// The sprite for the other button state, an inverted version of the same button.
        /// </summary>
        Texture2D oppBtnState;

        /// <summary>
        /// The current colour of the text within the button.
        /// </summary>
        Color textColour = Color.Black;

        /// <summary>
        /// The region that the button can be clicked in.
        /// </summary>
        protected Rectangle clickRegion;

        /// <summary>
        /// The sound effect of a button clicking. For realism, y'know.
        /// </summary>
        SoundEffect click = loader.Load<SoundEffect>("sfx\\click");

        /// <summary>
        /// Creates a new instance of a button
        /// </summary>
        /// <param name="x">The x coordinate of the button</param>
        /// <param name="y">The y coordinate of the button</param>
        /// <param name="btnText">The text within the button</param>
        public Button(int x, int y, string btnText)
            : base(x, y, "button_n")
        {
            oppBtnState = loader.Load<Texture2D>("sprites\\button_p");
            this.btnText = btnText;
            clickRegion = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Recalculates the clickable region if the position vector was changed.
        /// </summary>
        public void recalculateClickRegion()
        {
            clickRegion = new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }

        public override void updateLogic(double delta)
        {
            //Get the current mouse state
            MouseState ms = Mouse.GetState();

            //If the button is down...
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (clickRegion.Contains(new Point(ms.X, ms.Y)))
                {
                    if (this.sprite.Equals(loader.Load<Texture2D>("sprites\\button_n")))
                    {
                        this.swapSprites(ref oppBtnState); //...change the sprite to the pressed one...
                        textColour = Color.White; //...change the text colour...
                    }
                }
                clickCheck = true; //...and set the clickCheck to true.
            }
            if (ms.LeftButton == ButtonState.Released && clickCheck)
                //The point of clickCheck is whether or not the last update registered was a button down, and now if it's released, so the event doesn't constantly trigger.
            {
                if (this.sprite.Equals(loader.Load<Texture2D>("sprites\\button_p")))
                {
                    this.swapSprites(ref oppBtnState); //Swap the inverted sprite with the normal one...
                    textColour = Color.Black; //...change the text colour...
                }
                if (clickRegion.Contains(new Point(ms.X, ms.Y)))
                {
                    this.pressed = true; //...and say that the button has been clicked.
                }
            }
            if (ms.LeftButton == ButtonState.Released)
            {
                clickCheck = false;
            }
        }

        /// <summary>
        /// Checks if the button was pressed.
        /// </summary>
        /// <returns>Whether or not the button was clicked.</returns>
        public bool wasPressed()
        {
            if (this.pressed)
            {
                click.Play();
                this.pressed = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Draws the current button sprite and its text.
        /// </summary>
        /// <param name="sb">The SpriteBatch being used for drawing.</param>
        public override void draw(SpriteBatch sb)
        {
            Vector2 textOffset = new Vector2(width / 2, height / 2);
            sb.Draw(sprite, position, Color.White);
            sb.DrawString(Text.font, btnText, Position + textOffset, textColour, 0, Text.font.MeasureString(btnText) / 2, 1, SpriteEffects.None, 0);
        }
    }
}
