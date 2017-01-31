using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Still need everything
namespace XNAGame
{
    /// <summary>
    /// A class originally intended for debugging purposes, but became just
    /// a shorter version of saying "System.Diagnostics.Debug.WriteLine"
    /// (I mean, look at how long that is compared to Debug.output)
    /// </summary>
    public class Debug
    {
        /// <summary>
        /// Outputs the specified message to the console window.
        /// </summary>
        /// <param name="message">The message to output.</param>
        public static void output(object message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
