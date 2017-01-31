using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAGame.BeingTemplates;

namespace XNAGame.Beings
{
    /// <summary>
    /// It's an enemy because no one likes tutorials in video games.
    /// (Not me at least).
    /// Anyway, this draws a bunch of strings everywhere saying how to play the game
    /// if it is part of the level rendering List.
    /// </summary>
    public class e_tutorial : Enemy
    {
        Texture2D keys;
        Texture2D arrows;
        Texture2D pitArrow;

        public e_tutorial()
            : base(0, 0, "nothing")
        {
            //Get the textures to be rendered for the tutorial.
            keys = loader.Load<Texture2D>("sprites\\tut_keys");
            arrows = loader.Load<Texture2D>("sprites\\tut_clickHereArrows");
            pitArrow = loader.Load<Texture2D>("sprites\\tut_arrowToDeath");
            Player.tutorialMode = true;
        }

        public override void updateLogic(double delta)
        {

        }

        public override void draw(SpriteBatch sb)
        {
            //The coordinates were all trial and error, and are specifically made for the first level.
            Text.formatAndDisplayMessage(Text.getMsg("general", "tut_move"), 50 + (int)scrollOffset.X, 800 + (int)scrollOffset.Y);
            sb.Draw(keys, new Rectangle(50 + (int)scrollOffset.X, 800 + (int)scrollOffset.Y, 80, 80), Color.White);
            Text.formatAndDisplayMessage(Text.getMsg("general", "tut_fireWeapon"), 140 + (int)scrollOffset.X, 425 + (int)scrollOffset.Y);
            sb.Draw(arrows, new Rectangle(165 + (int)scrollOffset.X, 450 + (int)scrollOffset.Y, 60, 60), Color.White);
            Text.formatAndDisplayMessage(Text.getMsg("general", "tut_avoidDeath"), 800 + (int)scrollOffset.X, 750 + (int)scrollOffset.Y);
            sb.Draw(pitArrow, new Rectangle(850 + (int)scrollOffset.X, 800 + (int)scrollOffset.Y, 40, 50), Color.White);
            Text.formatAndDisplayMessage(Text.getMsg("general", "tut_shinyObj"), 1150 + (int)scrollOffset.X, 760 + (int)scrollOffset.Y);
            Text.formatAndDisplayMessage(Text.getMsg("general", "tut_pause"), 1500 + (int)scrollOffset.X, 500 + (int)scrollOffset.Y);
            Text.formatAndDisplayMessage(Text.getMsg("general", "tut_finishLine"), 1960 + (int)scrollOffset.X, 800 + (int)scrollOffset.Y);
        }
    }
}
