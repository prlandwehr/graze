using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graze
{
    class GRColors
    {
        private ArrayList colors;
        private Random rand;

        public GRColors()
        {
            colors = new ArrayList();
            rand = new Random();

            //colors.Add(Color.AliceBlue);
            //colors.Add(Color.AntiqueWhite);
            //colors.Add(Color.Aqua);
            //colors.Add(Color.Aquamarine);
            //colors.Add(Color.Azure);
            //colors.Add(Color.Beige);
            //colors.Add(Color.Bisque);
            colors.Add(Color.Black);
            //colors.Add(Color.BlanchedAlmond);
            //colors.Add(Color.Blue);
            //colors.Add(Color.BlueViolet);
            //colors.Add(Color.Brown);
            //colors.Add(Color.BurlyWood);
            //colors.Add(Color.CadetBlue);
            //colors.Add(Color.Chartreuse);
            //colors.Add(Color.Chocolate);
            //colors.Add(Color.Coral);
            //colors.Add(Color.CornflowerBlue);
            //colors.Add(Color.Cornsilk);
            //colors.Add(Color.Crimson);
            //colors.Add(Color.Cyan);
            colors.Add(Color.DarkBlue);
            colors.Add(Color.DarkCyan);
            //colors.Add(Color.DarkGoldenrod);
            colors.Add(Color.DarkGreen);
            //colors.Add(Color.DarkKhaki);
            colors.Add(Color.DarkMagenta);
            //colors.Add(Color.DarkOliveGreen);
            //colors.Add(Color.DarkOrange);
            colors.Add(Color.DarkOrchid);
            colors.Add(Color.DarkRed);
            //colors.Add(Color.DarkSalmon);
            //colors.Add(Color.DarkSeaGreen);
            //colors.Add(Color.DarkSlateBlue);
            //colors.Add(Color.DarkTurquoise);
            colors.Add(Color.DarkViolet);
            //colors.Add(Color.DeepPink);
            //colors.Add(Color.DeepSkyBlue);
            //colors.Add(Color.DodgerBlue);
            //colors.Add(Color.Firebrick);
            //colors.Add(Color.FloralWhite);
            //colors.Add(Color.ForestGreen);
            //colors.Add(Color.Fuchsia);
            ////colors.Add(Color.Gainsboro);
            //colors.Add(Color.GhostWhite);
            //colors.Add(Color.Gold);
            //colors.Add(Color.Goldenrod);
            //colors.Add(Color.Green);
            //colors.Add(Color.GreenYellow);
            //colors.Add(Color.Honeydew);
            ////colors.Add(Color.HotPink);
            //colors.Add(Color.IndianRed);
            //colors.Add(Color.Indigo);
            //colors.Add(Color.Ivory);
            //colors.Add(Color.Khaki);
            //colors.Add(Color.Lavender);
            //colors.Add(Color.LavenderBlush);
            //colors.Add(Color.LawnGreen);
            //colors.Add(Color.LemonChiffon);
            //colors.Add(Color.LightBlue);
            //colors.Add(Color.LightCoral);
            //colors.Add(Color.LightCyan);
            //colors.Add(Color.LightGoldenrodYellow);
            //colors.Add(Color.LightGreen);
            //colors.Add(Color.LightPink);
            //colors.Add(Color.LightSalmon);
            //colors.Add(Color.LightSeaGreen);
            //colors.Add(Color.LightSkyBlue);
            //colors.Add(Color.LightSteelBlue);
            //colors.Add(Color.LightYellow);
            //colors.Add(Color.Lime);
            //colors.Add(Color.LimeGreen);
            //colors.Add(Color.Linen);
            //colors.Add(Color.Magenta);
            //colors.Add(Color.Maroon);
            //colors.Add(Color.MediumAquamarine);
            //colors.Add(Color.MediumBlue);
            //colors.Add(Color.MediumOrchid);
            //colors.Add(Color.MediumPurple);
            //colors.Add(Color.MediumSeaGreen);
            //colors.Add(Color.MediumSlateBlue);
            //colors.Add(Color.MediumSpringGreen);
            //colors.Add(Color.MediumTurquoise);
            //colors.Add(Color.MediumVioletRed);
            //colors.Add(Color.MidnightBlue);
            ////colors.Add(Color.MintCream);
            ////colors.Add(Color.MistyRose);
            //colors.Add(Color.Moccasin);
            ////colors.Add(Color.NavajoWhite);
            //colors.Add(Color.Navy);
            ////colors.Add(Color.OldLace);
            //colors.Add(Color.Olive);
            //colors.Add(Color.OliveDrab);
            //colors.Add(Color.Orange);
            //colors.Add(Color.OrangeRed);
            //colors.Add(Color.Orchid);
            ////colors.Add(Color.PaleGoldenrod);
            ////colors.Add(Color.PaleGreen);
            ////colors.Add(Color.PaleTurquoise);
            //colors.Add(Color.PaleVioletRed);
            //colors.Add(Color.PapayaWhip);
            ////colors.Add(Color.PeachPuff);
            //colors.Add(Color.Peru);
            ////colors.Add(Color.Pink);
            //colors.Add(Color.Plum);
            ////colors.Add(Color.PowderBlue);
            //colors.Add(Color.Purple);
            //colors.Add(Color.Red);
            //colors.Add(Color.RosyBrown);
            //colors.Add(Color.RoyalBlue);
            //colors.Add(Color.SaddleBrown);
            ////colors.Add(Color.Salmon);
            //colors.Add(Color.SandyBrown);
            //colors.Add(Color.SeaGreen);
            ////colors.Add(Color.SeaShell);
            //colors.Add(Color.Sienna);
            //colors.Add(Color.SkyBlue);
            //colors.Add(Color.SlateBlue);
            //colors.Add(Color.Snow);
            //colors.Add(Color.SpringGreen);
            //colors.Add(Color.SteelBlue);
            //colors.Add(Color.Tan);
            //colors.Add(Color.Teal);
            //colors.Add(Color.Thistle);
            //colors.Add(Color.Tomato);
            //colors.Add(Color.Turquoise);
            //colors.Add(Color.Violet);
            //colors.Add(Color.Wheat);
            colors.Add(Color.White);
            //colors.Add(Color.WhiteSmoke);
            //colors.Add(Color.Yellow);
            //colors.Add(Color.YellowGreen);
        }

        public Color getColor()
        {
            int randomnum = rand.Next() % colors.Count;
            return (Color)colors[randomnum];
        }
    }
}
