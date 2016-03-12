using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher
{
    class ColorHelper
    {

        public static   Color[] heat = null;
        
        public static Color getHeatColor(int value)
        {
            if (value > 255) value = 255;
            if(heat==null)
            {
                heat = new Color[256];
                
                for (int r = 0; r < 128; ++r)
                {

                    Color color = HSBColor.ShiftHue(Color.FromArgb(0, 0, 255), (float)r / 2);
                    heat[r] = color;
                }
                for (int r = 0; r < 128; ++r)
                {
                    Color color = HSBColor.ShiftHue(Color.FromArgb(255, 0,0), (float)r/3);
                    heat[r + 128] = color;
                }
            }
            return heat[value];
        }

        public static Color getHeatSpectrum(int colorIntensity)
        {
            int red = colorIntensity;
            if (red > 255) red = 255;
            int green = colorIntensity - 255;
            if (green > 255) green = 255;
            if (green < 0) green = 0;
            int blue = colorIntensity - 512;
            if (blue > 255) blue = 255;
            if (blue < 0) blue = 0;
            return Color.FromArgb(red, green, blue);
        }


        // Initialize color array with gradient from color c1 to c2 to c3 to c4
        private static Color[] init_colors(Color c1, Color c2, Color c3, Color c4)
        {
            Color[] colors = new Color[256];
            for (int i = 0; i < 85; i++)
            {
                double f = ((double)i) / 32;
                colors[i] = Color.FromArgb(
                    clamp((int)(c1.R + (f * (c2.R - c1.R)))),
                    clamp((int)(c1.G + (f * (c2.G - c1.G)))),
                    clamp((int)(c1.B + (f * (c2.B - c1.B))))
                );
            }
            for (int i = 85; i < 170; i++)
            {
                double f = ((double)(i - 87)) / 32;
                colors[i] = Color.FromArgb(
                    clamp((int)(c2.R + (f * (c3.R - c2.R)))),
                    clamp((int)(c2.G + (f * (c3.G - c2.G)))),
                    clamp((int)(c2.B + (f * (c3.B - c2.B))))
                );
            }
            for (int i = 170; i < 256; i++)
            {
                double f = ((double)(i - 174)) / 32;
                colors[i] = Color.FromArgb(
                    clamp((int)(c3.R + (f * (c4.R - c3.R)))),
                    clamp((int)(c3.G + (f * (c4.G - c3.G)))),
                    clamp((int)(c3.B + (f * (c4.B - c3.B))))
                );
            }
            return colors;
        }
        
        public static int clamp(int i)
        {
            if (i < 0) return (0);
            if (i > 255) return (255);
            return (i);
        }
        // Initialize array for color gradient: white - orange - red - black
        static Color[] colorsFireRed = null;
        public static Color getFireColorRed(int value)
        {
            if(colorsFireRed==null)
            {
                Color c1 = Color.FromArgb(0, 0, 0);		// BLACK		
                Color c2 = Color.FromArgb(255, 0, 0);	// RED
                Color c3 = Color.FromArgb(255, 170, 0);	// ORANGE
                Color c4 = Color.FromArgb(255, 255, 255);	// WHITE
               colorsFireRed = init_colors(c1, c2, c3, c4);
            }
            if(value>255) value = 255;
            return colorsFireRed[value];
        }

        // Initialize array for color gradient: white - green - blue - black
        static Color[] colorsFireBlue = null;
        public static Color getFireColorBlue(int value)
        {
            if (colorsFireBlue == null)
            {

                Color c1 = Color.FromArgb(0, 0, 0);		// BLACK		
                Color c2 = Color.FromArgb(255, 0, 255);	// RED
                Color c3 = Color.FromArgb(100,255, 100);	// ORANGE
                Color c4 = Color.FromArgb(255, 255, 255);	// WHITE
                colorsFireBlue = init_colors(c1, c2, c3, c4);
            }
            if (value > 255) value = 255;
            return colorsFireBlue[value];
        }

        public static Color getFlashColor(Color color,int intensity)
        {


            if (intensity > 127)
            {
                int white = intensity - 127;
                white = white * white / 80;
                int r = Math.Min(color.R + white, 255);
                int g = Math.Min(color.G + white, 255);
                int b = Math.Min(color.B + white, 255);
                color = Color.FromArgb(r, g, b);
            }
            else
            {

                color = HSBColor.ShiftBrighness(color, -255 + (intensity*2));
            }

            return color;
        }

        internal static Color Dim(Color color)
        {
            int r = color.R / 16;
            int g = color.G / 16;
            int b = color.B / 16;
            return Color.FromArgb(r, g, b);
        }
    }
}
