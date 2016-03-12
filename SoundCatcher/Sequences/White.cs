using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class White : SequenceBase
    {
        public override void init()
        {
            controller.doFlurry = false;
            controller.flurry.setRGBLeft(Color.FromArgb(255, 2,2,255));
            controller.flurry.setRGBRight(Color.FromArgb(255,2,2,255));
            controller.lights.setRailAll(Color.Black);
            controller.flurryScenes.Straight();
           

        }
        byte i,j = 0;
        public override void go()
        {
       
        }
    }
}
