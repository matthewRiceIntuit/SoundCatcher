using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Tiers2 : SequenceBase
    {
        Color[] pars = new Color[16];
        double min, max;
        int colorSpeed;
        Color colorBar  = Color.Black;
        Color colorHead = Color.Black;

        public override void init()
        {
            controller.doFlurry = false;
            //controller.lights.bFlipSiblingRail = coinFlip();
            ticksPerCall = 1;
            if (coinFlip())
            {
                colorHead = controller.colors.even;
                colorBar = controller.colors.odd;

            }
            else
            {
                colorHead = controller.colors.odd;
                colorBar = controller.colors.even;
            }
        }

        Boolean bDirection = true;
        int step = 0;
        public override void go()
        {
            if (bDirection)
            {
                ++step;
                if (step > 255) bDirection = false;
            }
            else
            {
                --step;
                if (step < 5) bDirection = true;
            }

            if (controller.isBeat && controller.beatDetect.Step == 1)
            {
                controller.flurry.moveSlow();
            } 
            
            controller.lights.setRailAll(HSBColor.ShiftBrighness( colorBar,step) );
            controller.flurry.setAllRGB(HSBColor.ShiftBrighness(colorHead, 255 - step));
           
        }

    }
}
