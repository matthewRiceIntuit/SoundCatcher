using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class MovingHeadOnly : SequenceBase
    {
        public override void init()
        {
            controller.doFlurry = false;
           
            ticksPerCall = 1;
            controller.flurry.bDelayMoves =true;
            controller.flurry.delayTicks = random.Next(4) + 3;
            controller.flurry.setAllRGB(controller.colors.even);

            controller.flurry.sequenceSpeed = random.Next(10)+3;
            controller.flurry.sequenceColor = controller.colors.even;
            controller.flurry.sequenceColor2 = controller.colors.odd;
            if (coinFlip() ) controller.flurry.sequenceColor2 = Color.Black;

            controller.flurry.bSequence = true;
            controller.flurry.sequenceLength = random.Next(3);

            Console.WriteLine("Colors: " + controller.colors.even + "  :   " + controller.colors.odd);
           
        }   
   
        
        public override void go()
        {

            if (controller.isBeat)// && controller.beatDetect.Step==1)
            {
               if(random.Next(8)==0) controller.flurry.moveFast();
            }

           
        }

    }
}
