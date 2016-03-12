using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class OddEven : SequenceBase
    {
        int colorChangeMode = 0;
        int fadeValue = 0;
        int railPar = 0;
        Random _r = new Random();
        bool isEven = true;

        public override void init()
        {
            colorChangeMode = _r.Next(3);
            colorChangeMode = 0;

            fadeValue = 0;
            if (_r.Next(5) == 0)
            {
                if (_r.Next(2) == 0) fadeValue = -8;
                else fadeValue = -4;
            }

        }
        Color fade = Color.Black;

        public override void go()
        {
            Color color = fade;
            if (controller.isBeat)
            {
                controller.lights.setRailAll(Color.Black);
                if (colorChangeMode == 0 && controller.beatDetect.Step == 1)
                    color = (isEven) ? odd():even();
                if (colorChangeMode == 1)
                {
                    color = ((controller.beatDetect.Step > 2) ? even() : odd());
                }
                if (colorChangeMode == 2)
                {
                    color = ((controller.beatDetect.Step % 2 == 0) ? even() : odd());
                }
 
                
                fade = color;
            }
 
            if (fadeValue!=0)
            {
                fade = HSBColor.ShiftBrighness(fade, fadeValue);
            }

            if (isEven)
            {
                controller.lights.setRailEven(fade);
                controller.flurryColor = controller.colors.even;
                controller.flurryColor2 = controller.colors.odd;

            }
            else
            {
                controller.lights.setRailOdd(fade);
                controller.flurryColor = controller.colors.odd;
                controller.flurryColor2 = controller.colors.even;
            }
        } 
        

        Color even()
        {
            isEven = true;
            return controller.colors.even;
        }
        Color odd()
        {
            isEven = false;
            return controller.colors.odd;
        }

        
    }
}
