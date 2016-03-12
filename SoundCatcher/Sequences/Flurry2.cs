using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;
using SoundCatcher.Objects;

namespace SoundCatcher.Sequences
{
    class Flurry2 : SequenceBase
    {
        public Flurry2()
        {
            ConfigParams.Add(new ConfigParam("FadeStyle",2,"A more aggressive, pulsing fade")); 
        }

       
        int fadeValue = 0;
        bool isEven = true;
        

        public override void init()
        {
            
            fadeValue=0;
            if (random.Next(4) != 0)
            {
                if (random.Next(2) == 0) fadeValue = -8;
                else fadeValue = -4;
            }
            controller.flurry.bFlipSiblingFlurry = !(random.Next(6) ==0);
            controller.flurry.bIndependentMiddle = coinFlip();
            controller.flurry.bSequence = false;
            controller.flurry.bDelayMoves = false;
            if (random.Next(4) == 0)
            {
                controller.flurry.doSequence();
                int c =random.Next(2);
                if (random.Next(2) == 0) controller.flurry.sequenceColor = controller.flurryColor2;
 
            }
            
        }
        
        Color color,color2 = Color.Black;
        public override void go()
        {

            color = controller.flurryColor;
            color2 = controller.flurryColor2;
            
       

            if (controller.isBeat)
            {
                controller.flurry.bStrobe = random.Next(70);
            }
            if (controller.isBeat && controller.beatDetect.Step == 1)
            {
                controller.flurry.move();
            }

            if (fadeValue!=0)
            {
                color = HSBColor.ShiftBrighness(color, fadeValue);
                color2 = HSBColor.ShiftBrighness(color2, fadeValue);
            }

            controller.flurry.setEvenRGB(color);
            controller.flurry.setOddRGB(color2);
            controller.flurry.strobe();

           
         }

    }
}
