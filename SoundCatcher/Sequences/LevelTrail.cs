using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class LevelTrail : SequenceBase
    {
        Color[] pars = new Color[16];
        double min, max;
        int colorSpeed;
        int mode = 0;
        int colorStep = 0;
        Color[] colors = new Color[3];
        public override void init()
        {
            controller.doFlurry = false;
            //controller.lights.bFlipSiblingRail = coinFlip();
            ticksPerCall = 3;
            min = 255;
            max = 0;
            for (int r = 0; r < 16; ++r) pars[r] = Color.Black;

            colorSpeed = random.Next(9)+6;
            mode = random.Next(5);
            colors[0] = Color.Red;
            colors[1] = Color.Yellow;
            colors[2] = Color.Blue;

            controller.MatchRails = true;


        }

        int step = 280;
        Color c;
        public override void go()
        {

           // if (random.Next(3) == 0) return;
            if (colorSpeed < 7)
            {
                step -= colorSpeed;
                if (step < 0) step = 280;


                 c = HSBColor.ShiftHue(Color.Blue, (step * -1) + 100);
            }
            else
            {
                if (controller.isBeat) if (--colorStep < 0) colorStep = 2;
                c = colors[colorStep];
            }
            for (int r = 1; r < 16; ++r)
            {
                pars[r-1 ] = pars[r];
            }

           
            double amp = controller.beatDetect.Amplitude;
            min += 1;
            if (amp < min) min = amp;
            
            max -= 1;
            if (amp > max) max = amp;

            amp -= min * 1.1;
            amp = amp * 255 / max; 
            
            pars[15] = HSBColor.ShiftBrighness(c,(float)(-255.0 + amp ));

            
            if (mode == 0)
            {
                for (int r = 0; r < 8; ++r)
                {
                    controller.lights.setRailBoth(7-r, pars[15 - r]);
                    controller.lights.setRailBoth(8+r , pars[15 - r]);
                }
            }
            if (mode == 1)
            {
                for (int r = 0; r < 8; ++r)
                {
                    controller.lights.setRailBoth(r, pars[15 - r]);
                    controller.lights.setRailBoth(15 - r, pars[15 - r]);
                }
            }
            
            if (mode ==2)
            {
                for (int r = 0; r < 4; ++r)
                {
                    controller.lights.setRailBoth(3-r, pars[15 - r]);
                    controller.lights.setRailBoth(4+r, pars[15 - r]);
                    controller.lights.setRailBoth(11 - r, pars[15 - r]);
                    controller.lights.setRailBoth(12 + r, pars[15 - r]);
                }
            }
            if (mode > 2)
            {
                for (int r = 0; r < 4; ++r)
                {
                    controller.lights.setRailBoth(r, pars[15 - r]);
                    controller.lights.setRailBoth(7 - r, pars[15 - r]);
                    controller.lights.setRailBoth(15 - r, pars[15 - r]);
                    controller.lights.setRailBoth(8 + r, pars[15 - r]);
                }
            }

            controller.flurry.setRGBLeft(c);
            controller.flurry.setRGBRight(c);
            if (controller.isBeat) controller.flurry.moveFast();
           
        }

    }
}
