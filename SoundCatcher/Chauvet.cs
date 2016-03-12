using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher
{
    class Chauvet
    {
        public int railLeft = 53;
        public int railRight = 79;
        public bool bFlipSiblingRail = true;
        public Graphics g = null;
        public byte level = 255;


        public Color[] colors = new Color[512];
        public Color[] colorsGoal = new Color[512];
        public int[] diffR = new int[512];
        public int[] diffG = new int[512];
        public int[] diffB = new int[512];

        public float fade = 0;//1.1f;

        public Chauvet()
        {
        }

        public void setRGB(int channel, Color color)
        {

            colorsGoal[channel] = color;
            if (fade > 0)
            {
                if (colors[channel] != colorsGoal[channel])
                {
                    int R = color.R - (int)((colorsGoal[channel].R - colors[channel].R) / fade);
                    int G = color.G - (int)((colorsGoal[channel].G - colors[channel].G) / fade);
                    int B = color.B - (int)((colorsGoal[channel].B - colors[channel].B) / fade);
                    color = Color.FromArgb(R,G,B);
                }
            }
            colors[channel] = color;

            DmxController.setDmxValue(channel, color.R);
            DmxController.setDmxValue(channel + 1, color.G);
            DmxController.setDmxValue(channel + 2, color.B);

            if (g != null)
            {
                g.FillRectangle(new SolidBrush(color), 0, channel * 12, 10, 10);
            }
        }

        public Color getPAR(int par)
        {
            int channel = 0;
            if (par > 7)
            {
                channel = railRight + 1 + ((par - 8) * 3);
            }
            else
            {
                channel = railLeft + 1 + (par * 3);
            }
            return Color.FromArgb(DmxController.getDmxValue(channel),
                                  DmxController.getDmxValue(channel+1),
                                  DmxController.getDmxValue(channel+2));
            
        }
        public void setPAR(int channel, Color color)
        {
            setRGB(channel, color);

            DmxController.setDmxValue(channel + 3, 0);
            DmxController.setDmxValue(channel + 4, 0);
            DmxController.setDmxValue(channel + 5, 0);
            DmxController.setDmxValue(channel + 6, level);

 
        }
        public void writePAR(int channel, Color color)
        {
            setPAR(channel, color);
            DmxController.write();
        }

        public void setRailPar( int par, Color color)
        {

            setRGB(railLeft+ 1 + (par * 3), color);
            if (railRight > 0)
            {
                setRailSiblingPar(par, color);
            }
            DmxController.setDmxValue(railLeft, level);
        }
        public void setRailSiblingPar(int par, Color color)
        {
            if (railRight > 0)
            {
                if (bFlipSiblingRail)
                {
                    setRGB(railRight + 1+(21 - (par * 3)), color);
                }
                else
                {
                    setRGB(railRight + 1 +(par * 3), color);
                }
            }
            DmxController.setDmxValue(railRight, level);
        }

        public void setRailAll(Color color)
        {
            for (int r = 0; r < 8; ++r)
            {
                setRailPar(r, color);
            }
        }

        public void setRailOdd(Color color)
        {
            for (int r = 0; r < 8; r+=2)
            {
                setRailPar(r, color);
            }
        }

        public void setRail(Color color,string str)
        {
            for (int r = 0; r < 8; ++r)
            {
                if (str.Length < r + 1) return;
                char c = str[r];
                if (c == '#')
                {
                    setRailPar(r, color);
                }
                else if (c == '-')
                {
                    setRailPar(r, HSBColor.ShiftBrighness(color,-200));
                }
                else
                {
                    setRailPar(r, Color.Black);
                }
            }
        }

        public void setRailEven(Color color)
        {
            for (int r = 0; r < 8; r += 2)
            {
                setRailPar(r + 1, color);
            }
        }

        public void write()
        {
            DmxController.write();
        }


        internal void setRailParLeft(int par, Color color)
        {
            setRGB(railLeft + 1 + (par * 3), color);
        }
        internal void setRailParRight(int par, Color color)
        {
            if (bFlipSiblingRail)
            {
                setRGB(railRight + 1 + (21 - (par * 3)), color);
            }
            else
            {
                setRGB(railRight + 1 + (par * 3), color);
            }
        }

        internal void setRailBoth(int par, Color color)
        {
            if (par < 0 || par > 15) return;
            if (bFlipSiblingRail) par = 15 - par;
            if(par<8)
                setRGB(railRight + 1 + (21 - (par * 3)), color);
            else
                setRGB(railLeft + 1 + ((par-8) * 3), color);
        }
    }
}
