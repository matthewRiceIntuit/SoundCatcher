using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class ColorWaves2 : SequenceBase
    {
        public override void init()
        {
            controller.doFlurry = false;
            controller.lights.bFlipSiblingRail = coinFlip();
            ticksPerCall = 1;
            controller.MatchRails = true;

        }
        Color[] pars = new Color[16];
        int[] blips = new int[64];

        double step = 0;
        double speed = 1;
        int newSpeed = 1;
        int newWave = 20;
        double waveLength = 10;
        int direction = 1;
        public override void go()
        {
            if (controller.isBeat && controller.beatDetect.Step == 1)
            {
                newWave = random.Next(20)+10;
                newSpeed = 8;// random.Next(3) + 3;
                speed=0;
                direction *= -1;
                newSpeed *= direction;
                controller.flurry.move();
            }
            if (waveLength > newWave) waveLength-=.1;
            if (waveLength < newWave) waveLength+=.1;
            if (speed > newSpeed) speed -= .5;
            if (speed < newSpeed) speed += .5;

            step -= speed;
            if (step <0 ) step = 280;
            if (step > 280) step = 0;

            for (int r = 0; r < 16; ++r)
            {
                int shift = (int)(step+ (r* waveLength));
                while (shift > 280) shift -= 280;
                Color c = HSBColor.ShiftHue(Color.Red, shift);
                controller.flurry.setEvenRGB(c);
                controller.flurry.setOddRGB(c);
                controller.lights.setRailBoth(r, c);
            }
        }

    }
}
