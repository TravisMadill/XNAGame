using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAGame.BeingTemplates;

namespace XNAGame
{
    /// <summary>
    /// This class allows all the level data to be stored in one place.
    /// It contains a 2D array of integers that specify what is at what
    /// position. Mainly just if a wall is at a spot or not.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// The current level the player is at.
        /// </summary>
        public int curLevel;
        /// <summary>
        /// The current map of the level.
        /// </summary>
        public int[,] levelMap;
        /// <summary>
        /// The background texture to be used as the background.
        /// </summary>
        public Texture2D background;

        //These are constants that represent different things in the map
        public const int NONE = 0;
        public const int WALL = 1;
        public const int PLAYER = 2;
        public const int ENEMY = 3;
        public const int ITEM = 4;
        public const int FINISHLINE = 5;
        public const int TUTORIAL = 6;

        /// <summary>
        /// Loads a new level to the main being List.
        /// </summary>
        /// <param name="level">The level number.</param>
        public void loadLevel(int level)
        {
            curLevel = level;
            StreamReader r = new StreamReader(@"Resources\LevelData\" + level + ".txt");
            background = Being.loader.Load<Texture2D>("sprites\\" + r.ReadLine());
            string fileInfo = r.ReadToEnd();
            string[] levelRows = fileInfo.Split('\n');
            levelMap = new int[levelRows[0].Split(',').Length, levelRows.Length];
            for (int y = 0; y < levelRows.Length; y++)
            {
                string[] levelRowTiles = levelRows[y].Split(',');
                for (int x = 0; x < levelRowTiles.Length; x++)
                {
                    int levelTileID = 0;
                    string levelTileSubID = "";
                    if (levelRowTiles[x].Contains('-'))
                    {
                        levelTileID = Convert.ToInt32(levelRowTiles[x].Split('-')[0]);
                        levelTileSubID = levelRowTiles[x].Split('-')[1];
                    }
                    else
                        levelTileID = Convert.ToInt32(levelRowTiles[x]);
                    switch (levelTileID)
                    {
                        case NONE:
                            levelMap[x, y] = NONE;
                            break;
                        case WALL:
                            levelMap[x, y] = WALL;
                            Main.addBeing(new Beings.Wall(x*Main.TileSize, y*Main.TileSize));
                            break;
                        case PLAYER:
                            levelMap[x, y] = NONE;
                            Main.addBeing((Main.player = new Player(x * Main.TileSize, y * Main.TileSize)));
                            try
                            {
                                foreach (Weapon w in Main.curSaveData.weapons)
                                {
                                    Main.player.addWeapon(w);
                                }
                            }
                            catch (InvalidOperationException) { }
                            break;
                        case ENEMY:
                            levelMap[x, y] = NONE;
                            Enemy e = Enemy.getEnemyID(Convert.ToInt32(levelTileSubID));
                            e.Position = new Vector2(x * 40, y * 40);
                            Main.addBeing(e);
                            break;
                        case ITEM:
                            levelMap[x, y] = NONE;
                            Item i = Item.getItemID(Convert.ToInt32(levelTileSubID));
                            i.Position = new Vector2(x * 40, y * 40);
                            Main.addBeing(i);
                            break;
                        case FINISHLINE:
                            levelMap[x, y] = NONE;
                            Main.addBeing(new Beings.FinishLine(x * 40, y * 40));
                            break;
                        case TUTORIAL:
                            levelMap[x, y] = NONE;
                            Main.addBeing(new Beings.e_tutorial());
                            break;
                        default:
                            Debug.output("I dunno what " + levelRowTiles[x] + " means!");
                            break;
                    }
                }
            }

            r.Close();
        }

        /// <summary>
        /// Returns what a space in the level map contains.
        /// </summary>
        /// <param name="x">The x coordinate of the being.</param>
        /// <param name="y">The y coordinate of the being.</param>
        /// <param name="dir">The direction to check</param>
        /// <returns>The object in the space represented by this class's constants.</returns>
        public int spaceContains(int x, int y, int dir)
        {
            switch (dir)
            {
                    //Basically, get the centre of the tile that you're tyring to check and return what's there.
                case Collision.UP:
                    x += Main.TileSize / 2;
                    y += 0;
                    break;
                case Collision.DOWN:
                    x += Main.TileSize / 2;
                    y += Main.TileSize;
                    break;
                case Collision.LEFT:
                    x += 0;
                    y += Main.TileSize / 2;
                    break;
                case Collision.RIGHT:
                    x += Main.TileSize;
                    y += Main.TileSize / 2;
                    break;
            }
            try
            {
                return levelMap[x/40, y/40];
            }
            catch (IndexOutOfRangeException)
            {
                return WALL;
            }
        }
    }
}
