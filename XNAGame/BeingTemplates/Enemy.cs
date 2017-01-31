using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGame.Beings;

namespace XNAGame.BeingTemplates
{
    /// <summary>
    /// This class is a subclass of Being, which allows easy access to the being
    /// list and class recognition ("if(Being is Enemy)" is the entire if statement).
    /// </summary>
    public abstract class Enemy : Being
    {
        /// <summary>
        /// Enemies tend to speak their mind. Depending on if randomness feels like it.
        /// </summary>
        public Random commentRNG = new Random();

        /// <summary>
        /// Creates a new instance of an enemy.
        /// </summary>
        /// <param name="x">The x coordinate of the enemy.</param>
        /// <param name="y">The y coordinate of the enemy.</param>
        /// <param name="spriteName">The name of the texture in the sprites folder.</param>
        public Enemy(int x, int y, string spriteName)
            : base(x, y, spriteName)
        {

        }

        /// <summary>
        /// Gets an enemy type depending on its ID.
        /// </summary>
        /// <param name="id">The ID of the enemy.</param>
        /// <returns>The enemy corresponding to the given ID.</returns>
        public static Enemy getEnemyID(int id)
        {
            switch (id)
            {
                case 0:
                    return new e_sleeper(0, 0);
                case 1:
                    return new e_walker(0, 0);
                default:
                    Debug.output("Unknown Enemy ID: " + id + ". Returning e_sleeper instead.");
                    return new e_sleeper(0, 0);
            }
        }
    }
}
