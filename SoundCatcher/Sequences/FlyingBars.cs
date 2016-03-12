using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class FlyingBars : SequenceBase
    {
        Color color = Color.White;
        int flip = 0;
        bool redBlue = false;
        bool reverse = false;
        public override void init()
        {
            ticksPerCall = 1;
            controller.lights.fade = 2f;
            controller.lights.bFlipSiblingRail = coinFlip();
            controller.flurryColor = controller.colors.even;
            controller.flurryColor2 = controller.colors.odd;
            redBlue = coinFlip();
            reverse = coinFlip();

            length =  random.Next(6) + 1;
            if (reverse) ++length;
            
        }
     
        int[] pars = new int[16];
        int length = 6;
        int fade = 2;
        int position = 0;
        int step = 0;
        int pause = 0;
        public override void go()
        {
            if (controller.isBeat)   pause = 7;
           
            if (--pause > 0) return;
            for (int r = 0; r < 16; ++r) pars[r] = 255;
            for (int r = position; r < position+(length * fade); ++r)
            {
                if ((float)r / (float)fade <0) continue;
                if (r / fade>= 16) continue;
                pars[r/fade] -= 255/fade;
            }

            for (int r = 0; r < 8; ++r)
            {
                if ( reverse ) pars[r] = 255 - pars[r];
                Color c =(redBlue)?Color.FromArgb(255,255-pars[r],0,32) : HSBColor.ShiftBrighness(Color.White, (pars[r]) * -1);
                
                controller.lights.setRailPar(r, c);
                controller.flurryColor = c;
                controller.flurryColor2 = c;
            }
            ++position;
            if (position > 8*fade)
            {
                if (++step >= 3) step = 0;
                position = length * fade * -1;
            }
            
        }
    }
}
