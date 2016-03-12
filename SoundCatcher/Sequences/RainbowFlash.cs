using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class RainboxFlash : SequenceBase
    {
        double fadeDepthIntesity = 0;
        public override void init()
        {
            ticksPerCall = 1;
            controller.lights.fade = 0;
            controller.lights.bFlipSiblingRail = (random.Next(2) == 0);

            fadeDepthIntesity = 1.6+(random.Next(10)/10);
            controller.MatchRails = true;
  
        }
        int flash = 0;
        int step = 0;
        public override void go()
        {
            int fadeDepth = (int)(1 / controller.beatDetect.beatLength * 300 * fadeDepthIntesity);
            for (int r = 0; r < 8; ++r)
            {
                Color c = getColor(step +(r * 33));
                c =HSBColor.ShiftBrighness(c,controller.fadeDepth);
                controller.lights.setRailPar(r, c);
                controller.flurryColor = c;
                controller.flurryColor2 = c;

            }
            step += 3;

        }
  
        Color getColor(int r)
        {
            return HSBColor.ShiftHue(Color.Blue,  (r%240) * -1);
        }
    }
}
