using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGame
{
    /// <summary>
    /// This class controls every single message that goes through to the player.
    /// (Except for eff_fly)
    /// </summary>
    public static class Text
    {
        /// <summary>
        /// This holds a key and a value to match that key.
        /// In this case, it holds position and colour information as the key and the
        /// message string is the value behind the key.
        /// </summary>
        static Dictionary<float[], string> messages = new Dictionary<float[], string>();
        /// <summary>
        /// The font being used for the messages... and pretty much everything else.
        /// </summary>
        public static SpriteFont font;

        // Constants for the text effects.
        public const int NONE = 0;
        public const int SHAKE = 1;
        public const int WAVE = 2;
        public const int WAVE_yonly = 3;
        public const int WAVE_xonly = 4;
        public const int SHAKE_WAVE = 5; //The only real reason that these exist is because I was paranoid about stacking effects in the formatAndDisplayMessage method
        public const int SHAKE_WAVE_yonly = 6;
        public const int SHAKE_WAVE_xonly = 7;

        /// <summary>
        /// The speed of the wave.
        /// </summary>
        public const double waveSpeed = 1200000; // the lower, the faster
        /// <summary>
        /// The size of the wave.
        /// </summary>
        public const double waveSize = 3; // the higher, the bigger

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="sb">The SpriteBatch used for drawing.</param>
        public static void draw(SpriteBatch sb)
        {
            //Text.displayMessage("Messages: " + messages.Count, 40, 80, 1, 1, 1, 1, 1, 0);
            foreach (float[] info in messages.Keys)
            {
                string origMsg = messages[info];
                string[] msg = origMsg.Split('\n');
                Random r = new Random();
                Color textColour = new Color(info[2], info[3], info[4], info[5]);
                if (info[7] == NONE)
                {
                    sb.DrawString(font, origMsg, new Vector2(info[0], info[1]), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                }
                else
                {
                    for (int ln = 0; ln < msg.Length; ln++)
                    {
                        //For everything below here, almost every character is rendered individually
                        //If it's shaking, it'll display randomly within a 3x3 rectangle.
                        //If it's waving, it'll display in accordinance to the sine or cosine curve.
                        //If it's shake-waving, it'll do both at the same time.
                        switch (Convert.ToInt32(info[7]))
                        {
                            case SHAKE:
                                for (int i = 0; i < msg[ln].Length; i++)
                                {
                                    sb.DrawString(font, msg[ln].Substring(i, 1), new Vector2((float)(info[0] + r.NextDouble() * 3 + font.MeasureString(msg[ln].Substring(0, i)).X * info[6]), (float)(info[1] + r.NextDouble() * 3 + (ln * font.MeasureString(msg[ln]).Y * info[6]))), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                                }
                                break;
                            case WAVE:
                                for (int i = 0; i < msg[ln].Length; i++)
                                {
                                    double curTime = ((DateTime.Now.Ticks / waveSpeed - i));// -((info[0]) + getWidth(msg[ln].Substring(0, i)));
                                    int waveX = Convert.ToInt32(waveSize * -Math.Sin(curTime));// * Math.PI / 180.0);
                                    int waveY = Convert.ToInt32(waveSize * Math.Cos(curTime));// * Math.PI / 180.0);
                                    sb.DrawString(font, msg[ln].Substring(i, 1), new Vector2((float)(info[0] + waveX + font.MeasureString(msg[ln].Substring(0, i)).X * info[6]) + i, (float)(info[1] + waveY + (ln * font.MeasureString(msg[ln]).Y * info[6]))), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                                }
                                break;
                            case WAVE_yonly:
                                for (int i = 0; i < msg[ln].Length; i++)
                                {
                                    double curTime = ((DateTime.Now.Ticks / waveSpeed - i));// -((info[0]) + getWidth(msg[ln].Substring(0, i)));
                                    //int waveX = Convert.ToInt32(waveSize * -Math.Sin(curTime));// * Math.PI / 180.0);
                                    int waveY = Convert.ToInt32(waveSize * Math.Cos(curTime));// * Math.PI / 180.0);
                                    sb.DrawString(font, msg[ln].Substring(i, 1), new Vector2((float)(info[0] + font.MeasureString(msg[ln].Substring(0, i)).X * info[6]), (float)(info[1] + waveY + (ln * font.MeasureString(msg[ln]).Y * info[6]))), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                                }
                                break;
                            case WAVE_xonly:
                                for (int i = 0; i < msg[ln].Length; i++)
                                {
                                    double curTime = ((DateTime.Now.Ticks / waveSpeed - i));// -((info[0]) + getWidth(msg[ln].Substring(0, i)));
                                    int waveX = Convert.ToInt32(waveSize * -Math.Sin(curTime));// * Math.PI / 180.0);
                                    //int waveY = Convert.ToInt32(waveSize * Math.Cos(curTime));// * Math.PI / 180.0);
                                    sb.DrawString(font, msg[ln].Substring(i, 1), new Vector2((float)(info[0] + waveX + font.MeasureString(msg[ln].Substring(0, i)).X * info[6]), (float)(info[1] + (ln * font.MeasureString(msg[ln]).Y * info[6]))), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                                }
                                break;
                            case SHAKE_WAVE:
                                for (int i = 0; i < msg[ln].Length; i++)
                                {
                                    double curTime = ((DateTime.Now.Ticks / waveSpeed - i));// -((info[0]) + getWidth(msg[ln].Substring(0, i)));
                                    int waveX = Convert.ToInt32(waveSize * -Math.Sin(curTime));// * Math.PI / 180.0);
                                    int waveY = Convert.ToInt32(waveSize * Math.Cos(curTime));// * Math.PI / 180.0);
                                    sb.DrawString(font, msg[ln].Substring(i, 1), new Vector2((float)(info[0] + waveX + r.NextDouble() * 3 + font.MeasureString(msg[ln].Substring(0, i)).X * info[6]) + i, (float)(info[1] + waveY + r.NextDouble() * 3 + (ln * font.MeasureString(msg[ln]).Y * info[6]))), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                                }
                                break;
                            case SHAKE_WAVE_yonly:
                                for (int i = 0; i < msg[ln].Length; i++)
                                {
                                    double curTime = ((DateTime.Now.Ticks / waveSpeed - i));// -((info[0]) + getWidth(msg[ln].Substring(0, i)));
                                    //int waveX = Convert.ToInt32(waveSize * -Math.Sin(curTime));// * Math.PI / 180.0);
                                    int waveY = Convert.ToInt32(waveSize * Math.Cos(curTime));// * Math.PI / 180.0);
                                    sb.DrawString(font, msg[ln].Substring(i, 1), new Vector2((float)(info[0] + r.NextDouble() * 3 + font.MeasureString(msg[ln].Substring(0, i)).X * info[6]), (float)(info[1] + waveY + r.NextDouble() * 3 + (ln * font.MeasureString(msg[ln]).Y * info[6]))), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                                }
                                break;
                            case SHAKE_WAVE_xonly:
                                for (int i = 0; i < msg[ln].Length; i++)
                                {
                                    double curTime = ((DateTime.Now.Ticks / waveSpeed - i));// - ((info[0]) + getWidth(msg[ln].Substring(0, i)));
                                    int waveX = Convert.ToInt32(waveSize * -Math.Sin(curTime));// * Math.PI / 180.0);
                                    //int waveY = Convert.ToInt32(waveSize * Math.Cos(curTime));// * Math.PI / 180.0);
                                    sb.DrawString(font, msg[ln].Substring(i, 1), new Vector2((float)(info[0] + waveX + r.NextDouble() * 3 + font.MeasureString(msg[ln].Substring(0, i)).X * info[6]), (float)(info[1] + r.NextDouble() * 3 + (ln * font.MeasureString(msg[ln]).Y * info[6]))), textColour, 0, new Vector2(0, 0), info[6], SpriteEffects.None, 0);
                                }
                                break;
                        }
                    }
                }
            }
            messages.Clear();
        }

        /// <summary>
        /// Adds a message to the list of messages to be rendered.
        /// </summary>
        /// <param name="msg">The message to be displayed.</param>
        /// <param name="x">The x coordinate of the message.</param>
        /// <param name="y">The y coordinate of the message.</param>
        /// <param name="r">The red component of the colour of the message.</param>
        /// <param name="g">The green component of the colour of the message.</param>
        /// <param name="b">The blue component of the colour of the message.</param>
        /// <param name="a">The alpha component of the colour of the message.</param>
        /// <param name="scale">The desired size of the message.</param>
        /// <param name="effects">The effects the message should have according to the Text class's constants.</param>
        public static void displayMessage(string msg, int x, int y, float r, float g, float b, float a, float scale, int effects)
        {
            messages.Add(new float[8] { x, y, r, g, b, a, scale, effects }, msg);
        }

        /// <summary>
        /// Gets the width of the message, in pixels.
        /// </summary>
        /// <param name="msg">The string to measure.</param>
        /// <returns>The length of the string, in pixels.</returns>
        public static int getWidth(string msg)
        {
            return Convert.ToInt32(font.MeasureString(msg).X);
        }

        /// <summary>
        /// Retrieves a message from a file split by NULL characters.
        /// </summary>
        /// <param name="filename">The name of the file in the Resources\Text folder</param>
        /// <param name="id">The ID of the string</param>
        /// <returns>The string matching the ID in the file.
        /// See explanationOfThisFile in the general.txt file (the first one) in the text folder for details.</returns>
        public static string getMsg(string filename, string id)
        {
            try
            {
                StreamReader sr = new StreamReader(@"Resources\Text\" + filename + ".txt");
                bool good = true, starting = true;
                while (good)
                {
                    char ltr = Convert.ToChar(0);
                    if (ltr == '\0' || starting)
                    {
                        starting = false;
                        ltr = (char)sr.Read();
                        String curId = ltr.ToString();
                        while (ltr != '\0' && good)
                        {
                            curId += (ltr = (char)sr.Read());
                            if (ltr == 65535) good = false;
                        }
                        if (curId.Equals(id + "\0"))
                        {
                            String returningMsg = (ltr = (char)sr.Read()).ToString();
                            while (ltr != '\0' && ltr != (char)65535)
                            {
                                returningMsg += (ltr = (char)sr.Read());
                            }
                            sr.Close();
                            return returningMsg.Replace("\0", "").Replace(Convert.ToChar(65535).ToString(), "");
                        }
                        else
                        {
                            curId = "";
                        }
                    }
                    else if (ltr == 65535)
                    {
                        good = false;
                        sr.Close();
                    }
                }
                sr.Close();
            }
            catch (FileNotFoundException)
            {
                return "Cannot find file \"" + filename + ".txt\"\nin the Resources\\Text folder.";
            }
            catch (IOException)
            {
                return "IOException was thrown for some reason.";
            }
            catch (Exception)
            {
                return "Exception thrown.";
            }

            return "Message \"" + id + "\" not found in file " + filename;
        }

        /// <summary>
        /// Formats a message given with XML-like tags into its appropriate form.
        /// Default text colour is white.
        /// </summary>
        /// <param name="msg">The message with the XML-like tags.</param>
        /// <param name="x">The x coordinate of the message.</param>
        /// <param name="y">The y coordinate of the message.</param>
        public static void formatAndDisplayMessage(string msg, int x, int y)
        {
            //Basically, this'll go through the entire string and split the string into parts with its own specifications depending on the effect in the <> things.
            int curX = x, curY = y;
            float thisR = 1, thisG = 1, thisB = 1, thisA = 1, thisScale = 1;
            bool isShaking = false;
            int waveState = 0;
            int curEffects = 0;
            string curStr = "";
            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i] == '<')
                {
                    int braceNo = i;
                    string commandGet = "";
                    try
                    {
                        while (msg[i + 1] != '>')
                        {
                            try
                            {
                                commandGet += msg[i + 1];
                                i++;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        messages.Clear();
                    }
                    i++;
                    string[] commands = commandGet.Split(' ');

                    //Shake
                    if (commands[0].Equals("shake", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        isShaking = true;
                        switch (waveState)
                        {
                            case 0:
                                curEffects = SHAKE;
                                break;
                            case 1:
                                curEffects = SHAKE_WAVE;
                                break;
                            case 2:
                                curEffects = SHAKE_WAVE_yonly;
                                break;
                            case 3:
                                curEffects = SHAKE_WAVE_xonly;
                                break;
                        }
                    }
                    else if (commands[0].Equals("/shake", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        isShaking = false;
                        switch (waveState)
                        {
                            case 0:
                                curEffects = NONE;
                                break;
                            case 1:
                                curEffects = WAVE;
                                break;
                            case 2:
                                curEffects = WAVE_yonly;
                                break;
                            case 3:
                                curEffects = WAVE_xonly;
                                break;
                        }
                    }


                    //Waving text
                    else if (commands[0].Equals("wave", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        waveState = 1;
                        if (isShaking)
                            curEffects = SHAKE_WAVE;
                        else
                            curEffects = WAVE;
                        if (commands.Length == 2)
                        {
                            if (commands[1].Equals("yonly", StringComparison.OrdinalIgnoreCase))
                            {
                                waveState = 2;
                                curEffects += 1;
                            }
                            else if (commands[1].Equals("xonly", StringComparison.OrdinalIgnoreCase))
                            {
                                waveState = 3;
                                curEffects += 2;
                            }
                        }
                    }
                    else if (commands[0].Equals("/wave", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        waveState = 0;
                        if (isShaking)
                            curEffects = SHAKE;
                        else 
                            curEffects = NONE;
                    }


                    //Custom colours!
                    else if (commands[0].Equals("col", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        thisR = Int32.Parse(commands[1].Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255F;
                        thisG = Int32.Parse(commands[1].Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255F;
                        thisB = Int32.Parse(commands[1].Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255F;
                        thisA = Int32.Parse(commands[1].Substring(6, 2), System.Globalization.NumberStyles.HexNumber) / 255F;
                    }
                    else if (commands[0].Equals("/col", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        thisR = 1;
                        thisG = 1;
                        thisB = 1;
                        thisA = 1;
                    }


                    else if (commands[0].Equals("scale", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        thisScale = float.Parse(commands[1].Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator));
                    }
                    else if (commands[0].Equals("/scale", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX += getWidth(curStr);
                        curStr = "";
                        thisScale = 1;
                    }

                    else
                    {
                        Debug.output(commandGet);
                    }
                }

                else if (msg[i] == '\n')
                {
                    messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                    curX = x;
                    curY += (int)font.MeasureString(curStr).Y;
                    curStr = "";
                }
                else
                {
                    curStr += msg[i];
                    if (i == msg.Length - 1)
                    {
                        messages.Add(new float[8] { curX, curY, thisR, thisG, thisB, thisA, thisScale, curEffects }, curStr);
                        curX = x;
                        curY += (int)font.MeasureString(curStr).Y;
                        curStr = "";
                    }
                }

            }
        }
    }
}
