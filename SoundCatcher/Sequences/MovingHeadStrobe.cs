using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class MovingHeadStrobe : SequenceBase
    {
        public override void init()
        {
            controller.doFlurry = false;
            ticksPerCall = 2;
            c = controller.colors.odd;
            controller.flurry.bFlipSiblingFlurry = true;
            controller.flurry.setEvenRGB(c);
            controller.flurry.movePosition(controller.flurry.flurryLeft,65, 70, 0);
            controller.flurry.movePosition(controller.flurry.flurryRight, 65, 70, 0);
            controller.flurry.movePosition(controller.flurry.flurryLeft2, 65, 70, 0);
            controller.flurry.movePosition(controller.flurry.flurryRight2, 65, 70, 0);
        }   
   
        
        bool odd=true;
        bool colorOdd = true;
        bool strobe = true;
        Color c= Color.Black;
        int pan,tilt;
        int strobeCount = 0;
        public override void go()
        {
            controller.lights.setRailAll(Color.Black);
            if (controller.isBeat)// && controller.beatDetect.Step==1)
            {
                if (controller.beatDetect.Step == 1)
                {
                    colorOdd = !colorOdd;
                    if (colorOdd) c = controller.colors.odd;
                    else c = controller.colors.even;
                }
  
                
                odd=!odd;
                strobeCount = random.Next(4) + 3;
            }

            controller.flurry.setOddRGB(Color.Black);
            controller.flurry.setEvenRGB(Color.Black);
            strobe = !strobe;
            if (strobe == true && --strobeCount>0)
            {
                if (odd)
                {
                    controller.flurry.setRGBLeft(c);
                    controller.flurry.setRGBRight(Color.Black);
                }
                else
                {
                    controller.flurry.setRGBLeft(Color.Black);
                    controller.flurry.setRGBRight(c);
                }
            }
            }

    }
}
