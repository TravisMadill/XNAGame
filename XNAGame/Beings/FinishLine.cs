using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGame.Beings
{
    /// <summary>
    /// This class represents the continue arrow. Originally, I planned to actuall resemble a finish line,
    /// but... art.
    /// This class doesn't actually do anything, but I needed something for the Player class to identify
    /// that it touched the finish line to move on.
    /// </summary>
    class FinishLine:Being
    {
        /// <summary>
        /// Creates a level end sign.
        /// </summary>
        /// <param name="x">The x coordinate of the finish line.</param>
        /// <param name="y">The y coordinate of the finish line.</param>
        public FinishLine(int x, int y)
            : base(x, y, "finishSign")
        {

        }

        public override void updateLogic(double delta)
        {
        }
    }
}
