using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class RunWayChase3 : SequenceBase
    {
        int dim = 128;
        public override void init()
        {
            ticksPerCall = 5;
            Random _r = new Random();
            if (_r.Next(2) >= 1) dim *= -1; 
        }

        int step = 0;
        public override void go()
        {
            Color color1 = controller.colors.odd;
            Color color2 = controller.colors.even;
            if (dim > 0)
            {
                color1 = controller.colors.even;
                color2 = controller.colors.odd;
            }
 
            for (int r = 0; r < 8; ++r)
            {
                controller.lights.setRailPar(r, color1);
            }
            controller.lights.setRailPar(step % 8, color2);
            controller.flurryColor = color2;
            controller.flurryColor2 = color1;
            ++step;
        }
    }
}
