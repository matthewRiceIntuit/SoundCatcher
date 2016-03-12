using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class BlackOut : SequenceBase
    {
        Color c1,c2;

        public override void init()
        {
            ticksPerCall = 2;

        }

        double red = 0f;
        double inc = .01f;
        int move = 700;
        public override void go()
        {
            red += inc;
            if(red >20) inc=-.01f;
            if (red < 0) { inc = .01f; red = 0; }

            Color color = Color.FromArgb((red > 10) ? (int)red - 10 : 0, 0, 32);

            controller.lights.setRailAll(color);
            if(++move>400)
            {
                move = 0;
                controller.flurry.moveSlow();
            }
            controller.flurry.bStrobe = -1;
            controller.flurryColor = color;
            controller.flurryColor2 = color;

            controller.flurry.setAllRGB(color);
             
        }

    }
}
