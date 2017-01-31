using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNAGame.Beings;

namespace XNAGame.BeingTemplates
{
    /// <summary>
    /// This class embodies all collectables in the game.
    /// They're also really shiny.
    /// </summary>
    public abstract class Item : Being
    {
        /// <summary>
        /// The value of this item, in score points.
        /// </summary>
        protected int scoreValue;

        /// <summary>
        /// The type of this item.
        /// 0: An object worth points and nothing else
        /// 1: A weapon.
        /// </summary>
        protected int itemType;

        /// <summary>
        /// The random number generator for showing the gleam sprite or random comments.
        /// </summary>
        static Random r = new Random();

        /// <summary>
        /// The texture used for the shiny gleam that comes out occasionally.
        /// </summary>
        static Texture2D gleam = loader.Load<Texture2D>("sprites\\gleam");

        /// <summary>
        /// Creates a new item type.
        /// </summary>
        /// <param name="x">The x coordinate of the item.</param>
        /// <param name="y">The y coordinate of the item.</param>
        /// <param name="spriteName">The name of the sprite in the sprites folder.</param>
        /// <param name="scoreValue">The value of this item, in score points.</param>
        /// <param name="itemType">The type of this item.</param>
        public Item(int x, int y, string spriteName, int scoreValue, int itemType)
            : base(x, y, spriteName)
        {
            this.scoreValue = scoreValue;
            this.itemType = itemType;
        }

        public override void updateLogic(double delta)
        {
            if (r.Next(10000) < 10)
            {
                if (r.Next(100) > 10)
                {
                    Main.addBeing(new Beings.eff_fly((int)Position.X+10, (int)Position.Y+10, gleam));
                }
                else
                {
                    Main.addBeing(new Beings.eff_fly((int)Position.X, (int)Position.Y, Text.getMsg("general", "gleam_" + itemType + "_" + r.Next(6)).Replace("%s", scoreValue.ToString())));
                }
            }
        }

        /// <summary>
        /// What happens when the item is taken.
        /// </summary>
        public abstract void takeEffect();

        /// <summary>
        /// Gets an enemy type depending on its ID.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <returns>The item corresponding to the given ID.</returns>
        public static Item getItemID(int id)
        {
            switch (id)
            {
                case 0:
                    return new i_scoreTest(0, 0);
                case 1:
                    return new i_shinyObject(0, 0);
                case 2:
                    return new i_laser(0, 0);
                default:
                    Debug.output("Unknown Item ID: " + id + ". Returning i_scoreTest instead.");
                    return new i_scoreTest(0, 0);
            }
        }
    }
}
