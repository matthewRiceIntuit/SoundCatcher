using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class OverLapping : SequenceBase
    {
        public override void init()
        {
            ticksPerCall = 3;
            controller.lights.fade = 0;// 1.1f;
           
        }
        Color[] pars = new Color[8];
        int flash = 0;
        int step = -1;
        int direction = 7;
        int direction2 = 0;
        Color color1 = Color.Blue;
        Color color2 = Color.Red;
        bool waitForBeat = true;
        public override void go()
        {
            for (int r = 0; r < 8; ++r) pars[r] = Color.Black;
            if (waitForBeat && !controller.isBeat)
            {
                for (int r = 0; r < 8; ++r) controller.lights.setRailPar(r, pars[r]);
                return;
            }
            waitForBeat = false;

            for (int r = 0; r < 8; ++r) pars[r] = Color.Black;

            ++step;
            if (step < 13)
            {
                for (int r = Math.Max(0,step-2); r <= step; ++r)
                {
                    if (r > 7) break;
                    MixColor(r,color1);
                    MixColor(7 - r, color2);
                }
                for (int r = 0; r < 8; ++r) controller.lights.setRailPar(r, pars[r]);
                return;
            }

           
       
            step = -1;
            waitForBeat = true;
            if (direction > 0)
            {
                direction = 0;
                color1 = Color.Blue;
                controller.flurryColor = color1;
                controller.flurryColor2 = color2;
            }
            else
            {
                color1 = Color.Lime;
                direction = 7;
                controller.flurryColor2 = color1;
                controller.flurryColor = color2;
            }

         

   
        }

        void MixColor(int par, Color c)
        {
            int R = Math.Min(pars[par].R + c.R, 255);
            int G = Math.Min(pars[par].G + c.G, 255);
            int B = Math.Min(pars[par].B + c.B, 255);
            pars[par] = Color.FromArgb(R, G, B);
        }
  
    }
}
