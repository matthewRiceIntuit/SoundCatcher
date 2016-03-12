using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class SinCos : SequenceBase
    {
        int fade = 5;
        int steps = 0;
        int lights = 0;
        bool redBlue = true;
        public override void init()
        {
            controller.lights.fade = 1.1f;
            ticksPerCall = 1;
            step = 0;
            controller.doFlurry = false;
            controller.MatchRails = true;

        }
        int flash = 0;
        Color[] pars = new Color[16];
 
        double step = 0;
        bool odd = false;
        int clr = 0;
        Color clr1 = Color.Blue;
        Color clr2 = Color.Blue;

        public override void go()
        {
            //if (controller.isBeat && controller.beatDetect.Step == 1) step = 0;

            if (step > 186) step = 0;
            if (--clr < 0) clr = 280;
            int clr2 = clr + 100;
            if (clr2 > 280) clr2 -= 280;

            int x= (int)(8+ Math.Sin( step/controller.beatDetect.beatLength )*8);
            x += 8; if (x > 15) x -= 8;
            controller.lights.setRailAll(Color.Black);
            
            Color c = HSBColor.ShiftHue(Color.Blue, (clr * -1)  + 100);
            Color c2 = HSBColor.ShiftHue(Color.Blue, (clr2 * -1) + 100);
            
            controller.flurry.setRGBLeft(c);
            controller.lights.setRailBoth(x,c);
            
            controller.flurry.setRGBRight(c2);
            controller.lights.setRailBoth(15 - x, c2);
            ++step;
            return;
        }

 
    }
}
