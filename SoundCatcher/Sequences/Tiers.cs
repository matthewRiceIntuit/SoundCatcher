using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Tiers : SequenceBase
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

        Boolean bStep = true;
        public override void go()
        {

            if (controller.isBeat && controller.beatDetect.Step == 1)
            {
                controller.flurry.moveSlow();
                bStep = !bStep;
            } 
            
            if (bStep)
            {
                controller.lights.setRailAll( colorBar );
                controller.flurry.setAllRGB( Color.Black );
            }
            else
            {
                controller.lights.setRailAll( Color.Black );
                controller.flurry.setAllRGB( colorHead );
            }
           
        }

    }
}
