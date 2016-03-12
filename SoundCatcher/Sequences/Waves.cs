using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Waves : SequenceBase
    {
        int fade = 5;
        int steps = 0;
        int lights = 0;
        int waveLength = 17;
        bool redBlue = true;
        public override void init()
        {
            controller.doFlurry = false;
            controller.lights.bFlipSiblingRail = coinFlip();
            ticksPerCall = 1;
            waveLength = 57;
            controller.MatchRails = true;
        }
        int flash = 0;
        Color[] pars = new Color[16];
        int[] blips = new int[64];

        int step = 0;
        int step2 = 0;
        bool odd = false;
        public override void go()
        {
           // if (random.Next(3) == 0) return;
            ++step2;
            if (controller.isBeat && controller.beatDetect.Step % 2 !=0) step2 = 0;
      
            if (--step <0 ) step = 280;

            int shift = (step > 255) ? 255 - (step - 255) : step;
            for (int r = 0; r < 16; ++r)
            {
                Color c =  HSBColor.ShiftHue(Color.Blue,(step * -1)+(r*2)+100);
                //Color c = Color.Blue;
                int wave = ((step2/2)+r);
                if (wave > 15) wave = 0;
                c = HSBColor.ShiftBrighness(c, -100+(wave)*5);
                controller.flurry.setRGBLeft(c);
                controller.flurry.setRGBRight(c);
                controller.lights.setRailBoth(r, c);
            }
        }

    }
}
