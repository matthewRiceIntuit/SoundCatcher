using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Slugs : SequenceBase
    {
        int fade = 5;
        int steps = 0;
        int lights = 0;
        bool redBlue = true;
        public override void init()
        {
            controller.lights.fade = 0f;
            ticksPerCall = 1;
            slugColor[0] = Color.Red;
            slugColor[1] = Color.Lime;
            slugColor[2] = Color.Blue;
            slugColor[3] = Color.Red;
            slugPosition[0] = -1;
            slugPosition[2] = 13;
            slugDirection[0] = 1;
            slugState[0] = 0;
            slugStates[0] = 10;
            
        }
        int flash = 0;
        Color[] pars = new Color[17];
        int[] slugPosition = new int[4];
        int[] slugDirection = new int[4];
        int[] slugState = new int[4];
        int[] slugStates = new int[4];

        Color[] slugColor = new Color[16];

        int step = 0;
        bool odd = false;
        public override void go()
        {

            for (int r = 0; r < 16; ++r) controller.lights.setRailBoth(r, Color.Black);
            
            if (slugPosition[0] >17 || slugPosition[0]<-1){
                if (coinFlip())
                {
                    slugDirection[0] = 1;
                    slugPosition[0] = 0;
                }
                else
                {
                    slugDirection[0] = -1;
                    slugPosition[0] = 15;
                }
            }
            for (int r = 0; r <16;++r) pars[r]=Color.Black ;
            slugState[0] += 1;
            if (slugState[0] >= slugStates[0])
            {
                slugState[0] = 0;
                slugPosition[0] += slugDirection[0];
                controller.lights.setRailBoth(slugPosition[0] - slugDirection[0],Color.Red);
            }
            else
            {
                float fade = 64.0f / (float)slugStates[0] * (float)slugState[0];
                controller.lights.setRailBoth(slugPosition[0] - slugDirection[0], HSBColor.ShiftBrighness(Color.Red, -200 - fade));
            }
            controller.lights.setRailBoth(slugPosition[0], Color.Red);
        }

        void mergePar(int pos, Color c)
        {
            if (pos < 0 || pos > 15) return;
             pars[pos] = Color.FromArgb(255,
                Math.Min(c.R + pars[pos].R,255),
                Math.Min(c.G + pars[pos].G,255),
                Math.Min(c.B + pars[pos].B,255));
        }

    }
}
