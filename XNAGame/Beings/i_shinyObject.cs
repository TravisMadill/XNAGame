using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAGame.BeingTemplates;

namespace XNAGame.Beings
{
    /// <summary>
    /// This item gives 100 points to the player if the player collects it.
    /// All games require you to collect something shiny, right?
    /// </summary>
    class i_shinyObject : Item
    {
        /// <summary>
        /// Creates a new shiny object to collect.
        /// </summary>
        /// <param name="x">The x coordinate of this shiny object.</param>
        /// <param name="y">The y coordinate of this shiny object.</param>
        public i_shinyObject(int x, int y)
            : base(x, y, "shinyObj", 100, 0)
        {

        }

        public override void takeEffect()
        {
            //Get some points. And say "cha-ching"... because they're valuable. 100 points!
            Main.addBeing(new eff_fly((int)position.X, (int)position.Y, Text.getMsg("general", "playerComment_getShinyObj")));
            Main.curSaveData.curScore += scoreValue;
        }

        public override void draw(SpriteBatch sb)
        {
            //This rotates.
            if (isOnScreen(position, width, height))
                sb.Draw(sprite, new Rectangle((int)(position.X + scrollOffset.X), (int)(position.Y + scrollOffset.Y), this.width, this.height), null, Color.White, (float)(DateTime.Now.Millisecond / (Math.PI*50)), new Vector2(this.width / 2, this.height / 2), SpriteEffects.None, 0);
        }
    }
}
