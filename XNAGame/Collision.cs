using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGame.Beings;

namespace XNAGame
{
    /// <summary>
    /// This class is the collision centre of operations.
    /// Everything involving things touching other things are handled here.
    /// (That did not sound right at all)
    /// This class was much larger before (actually justifying using a class for using just a class),
    /// but most of the other methods didn't work.
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// The collision constant for the tile above.
        /// </summary>
        public const int UP = 0;
        /// <summary>
        /// The collision constant for the tile below.
        /// </summary>
        public const int DOWN = 1;
        /// <summary>
        /// The collision constant for the tile to the left.
        /// </summary>
        public const int LEFT = 2;
        /// <summary>
        /// The collision constant for the tile to the right.
        /// </summary>
        public const int RIGHT = 3;

        /// <summary>
        /// Checks whether or not a being collides with another being.
        /// It's up to the being to decide what to do with the information.
        /// </summary>
        /// <param name="thisBeing">The being to check.</param>
        /// <returns>A Being List of possible collisions.</returns>
        public static List<Being> getPossibleCollisions(Being thisBeing)
        {
            //Basically, if the hitboxes intersect, then it's added to the list.
            List<Being> beings = new List<Being>();
            Rectangle thisBeingBounds = thisBeing.HitBox;
            foreach (Being b in Main.beings)
            {
                if (!b.Equals(thisBeing))
                {
                    if (b.HitBox.Intersects(thisBeingBounds))
                        beings.Add(b);
                }
            }
            return beings;
        }

        /// <summary>
        /// Determines the direction of collision of two beings in relation to the second one being specified.
        /// </summary>
        /// <param name="b1">The colliding being.</param>
        /// <param name="b2">The being that is colliding with the first one.</param>
        /// <returns>The direction represented by one of the collision constants.</returns>
        public static int getCollisionDir(Being b1, Being b2)
        {
            int xDiff = b1.HitBox.X - b2.HitBox.X;
            int yDiff = b1.HitBox.Y - b2.HitBox.Y;

            if (Math.Abs(xDiff) > Math.Abs(yDiff))
            {
                if (xDiff > 0)
                    return RIGHT;
                else
                    return LEFT;
            }
            else
            {
                if (yDiff > 0)
                    return DOWN;
                else
                    return UP;
            }
        }
    }
}
