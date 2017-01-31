using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGame.BeingTemplates;

namespace XNAGame.Beings
{
    /// <summary>
    /// This class goes unused. This was just to test scores and the Item template.
    /// Fun fact, the e_sleeper was the tester for the Enemy template.
    /// </summary>
    class i_scoreTest : Item
    {
        /// <summary>
        /// Creates a new score tester. Worth 100 points.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public i_scoreTest(int x, int y)
            : base(x, y, "gem", 100, 0)
        {

        }

        public override void takeEffect()
        {
            //Give some points.
            Main.curSaveData.curScore += this.scoreValue;
        }
    }
}
