using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Bugs : SequenceBase
    {
        int fade = 5;
        int steps = 0;
        int lights = 0;
        bool redBlue = true;
        public override void init()
        {
            controller.lights.fade = 4f;
            ticksPerCall = 2;
            bugsColor[0] = Color.Red;
            bugsColor[1] = Color.Lime;
            bugsColor[2] = Color.Blue;
            bugsPosition[1] = 5;
            bugsPosition[2] = 13;
            
        }
        int flash = 0;
        Color[] pars = new Color[16];
        int[] bugsPosition = new int[4];
        int[] bugsLength = new int[4];
        int[] bugsBright = new int[4];
        Color[] bugsColor = new Color[4];

        int step = 0;
        bool odd = false;
        public override void go()
        {
            if (!controller.isBeat) return;
            for (int r = 0; r < 16; ++r)  pars[r] = Color.Black;
            for (int r = 0; r < 3; ++r)
            {
                if (coinFlip()) bugsPosition[r]++;
                if (coinFlip()) bugsPosition[r]--;
                if (bugsPosition[r] > 17) bugsPosition[r] = 17;
                if (bugsPosition[r] <-1) bugsPosition[r] = -1;

                if (coinFlip()) bugsLength[r]++;
                if (coinFlip()) bugsLength[r]--;
                if (bugsLength[r] > 4) bugsLength[r] = 4;
                if (bugsLength[r] < 1) bugsLength[r] = 1;

                if (coinFlip()) bugsBright[r]++;
                if (coinFlip()) bugsBright[r]--;
                if (bugsBright[r] > 4) bugsBright[r] = 4;
                if (bugsBright[r] < 0) bugsBright[r] = 0;

                for (int i = bugsPosition[r]; i < bugsPosition[r] + bugsLength[r]; ++i)
                {
                    mergePar(i,HSBColor.ShiftBrighness( bugsColor[r],bugsBright[r]*-50));
                }
            }

            for (int r = 0; r < 16; ++r) controller.lights.setRailBoth(r, pars[r]);
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
