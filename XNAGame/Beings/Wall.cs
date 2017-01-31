using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XNAGame;

namespace XNAGame.Beings
{
    /// <summary>
    /// Walls! And floors! And they probably don't have ears either.
    /// </summary>
    class Wall : Being
    {
        /// <summary>
        /// Creates a new wall being at the specified location
        /// </summary>
        /// <param name="x">The x-coordinate of the wall</param>
        /// <param name="y">The y-coordinate of the wall</param>
        public Wall(int x, int y)
            : base(x, y, Being.loader.Load<Texture2D>("sprites\\wall")) { }

        public override void updateLogic(double delta)
        {
            //The walls don't really do anything... besides exist.
        }
    }
}
