using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGame
{
    /// <summary>
    /// This class controls all dialogue boxes.
    /// Dialogue boxes prompt the player about something, and perform a specified
    /// series of events upon clicking the OK button.
    /// </summary>
    public class DialogueBox : Being
    {
        /// <summary>
        /// The message to display.
        /// </summary>
        string displayMsg;
        /// <summary>
        /// The size of the dialogue box (i.e. This box can be stretched)
        /// </summary>
        Rectangle boxSize;
        /// <summary>
        /// The OK button being.
        /// </summary>
        Button okayBtn = new Button(0, 0, "OK");
        /// <summary>
        /// The method that is passed from the event calling the dialogue box
        /// to be executed once the OK button is pressed.
        /// </summary>
        Func<bool> desiredActions;

        /// <summary>
        /// Creates a new instance of a dialogue box.
        /// </summary>
        /// <param name="msg">The message to display in the dialogue box.</param>
        /// <param name="desiredActionsOnOK">The method to execute when the OK button is pressed.</param>
        public DialogueBox(string msg, Func<bool> desiredActionsOnOK)
            : base(0, 0, "dialogueBox")
        {
            this.displayMsg = msg;
            this.desiredActions = desiredActionsOnOK;
            //Calculates the size of the box based on the message size
            boxSize = new Rectangle(Main.DisplayWidth / 2 - (int)Text.font.MeasureString(msg).X / 2 - 30, Main.DisplayHeight / 2 - (int)Text.font.MeasureString(msg).Y / 2 - 30, (int)Text.font.MeasureString(msg).X + 30, (int)Text.font.MeasureString(msg).Y + 30);
            okayBtn.Position = new Vector2(boxSize.Right - okayBtn.width, boxSize.Bottom);
        }

        public override void updateLogic(double delta)
        {
            //Recalculates the OK button click area since the button was positioned differently when the box was created.
            okayBtn.recalculateClickRegion();
            //Do the logic here, since the main update loop won't include buttons otherwise.
            okayBtn.updateLogic(delta);
            if (okayBtn.wasPressed()) //Once the button is pressed...
            {
                desiredActions(); //...execute the desired actions...
                Main.removeBeing(this); //...get rid of this being...
                Main.dialogueIsUp = false; //...and report to the main update loop that it's okay to resume normal activities.
            }
        }

        public override void draw(SpriteBatch sb)
        {
            //Draw the box, button, and the text
            sb.Draw(this.sprite, boxSize, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            Text.formatAndDisplayMessage(displayMsg, boxSize.X + 15, boxSize.Y+15);
            okayBtn.draw(sb);
        }
    }
}
