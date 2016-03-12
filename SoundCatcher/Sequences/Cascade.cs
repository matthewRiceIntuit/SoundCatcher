using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Cascade : SequenceBase
    {
        Color color = Color.White;
        int flip = 0;
        bool redBlue = false;
        bool reverse = false;
        public override void init()
        {
            ticksPerCall = 1;
            controller.lights.bFlipSiblingRail = false;
            controller.flurryColor = controller.colors.even;
            controller.flurryColor2 = controller.colors.odd;
            redBlue = coinFlip();
            reverse = coinFlip();
            reset();
        }

        Color[] pars = new Color[16];
        int length;
        int position; 
        int step;

        void reset()
        {
            position = 0;
            length = 9;
            step = length;
        }

        public override void go()
        {
            for (int r = 0; r < 8; ++r)
            {
                pars[r] = Color.Black;
            }

            if (--step < 0)
            {
                ++position;
                length-=2;
                step = length;
            }

            if (position < 8)
                pars[7-position] = Color.White;
            for (int r = 6 - position; r >= 0; --r)
            {
                pars[r] = HSBColor.ShiftBrighness(Color.White, r * -50);
          
            }
        
            for (int r = 0; r < 8; ++r)
            {
                controller.lights.setRailPar(r, pars[r]);
            }
            if (position >10) reset();
        }
    }
}
