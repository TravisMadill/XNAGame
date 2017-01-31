using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using XNAGame.Beings;
using XNAGame.BeingTemplates;

namespace XNAGame
{
    /// <summary>
    /// This class represents save data that can be output to an XML file.
    /// This was supposedly the "correct" way to save, according to Microsoft.
    /// </summary>
    public class SaveData
    {
        /// <summary>
        /// The name of the save's file name.
        /// </summary>
        static string saveFileName = "data.sav";

        /// <summary>
        /// Represents a new game.
        /// </summary>
        public static SaveStructure newSave;

        /// <summary>
        /// The types being used in the XML serialisation process.
        /// </summary>
        static Type[] xmlTypesUsing = new Type[] { typeof(Weapon), typeof(w_rocket), typeof(w_laser) };

        /// <summary>
        /// Creates a new SaveStructure.
        /// </summary>
        public struct SaveStructure
        {
            public int curLevel;
            public int curScore;
            public List<Weapon> weapons;
        }

        /// <summary>
        /// Saves a game to the filename specified in the class constants.
        /// </summary>
        /// <param name="save">The SaveStructure to save.</param>
        /// <returns>A string that says if the save worked or not.</returns>
        public static string saveGame(SaveStructure save)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(SaveStructure), xmlTypesUsing);
            
            //If a save exits already, delete the file to replace the new save file with.
            if (File.Exists("Resources\\" + saveFileName))
            {
                File.Delete("Resources\\" + saveFileName);
            }
            StreamWriter writer = new StreamWriter("Resources\\" + saveFileName);
            try
            {
                xmlSerial.Serialize(writer, save);
                writer.Close();
                return Text.getMsg("general", "menuStatus_saveFinish");
            }
            catch (InvalidOperationException e)
            {
                writer.Close();
                return Text.getMsg("general", "menuStatus_saveFail").Replace("%s", e.InnerException.Message);
            }
        }

        /// <summary>
        /// Loads a game from the filename constant.
        /// </summary>
        /// <returns>The SaveStructure created from the file.</returns>
        public static SaveStructure loadGame()
        {
            try
            {
                XmlSerializer xmlSerial = new XmlSerializer(typeof(SaveStructure), xmlTypesUsing);
                if (File.Exists("Resources\\" + saveFileName))
                {
                    StreamReader reader = new StreamReader("Resources\\" + saveFileName);
                    SaveStructure save = (SaveStructure)xmlSerial.Deserialize(reader);
                    reader.Close();
                    return save;
                }
                return newSave; //If a save doesn't exist, then just return a new save
            }
            catch (InvalidOperationException)
            {
                return newSave;
            }
        }
    }
}
