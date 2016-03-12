using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class RunWayChase2 : SequenceBase
    {
        int dim = 128;
        public override void init()
        {
            ticksPerCall = 5;
            Random _r = new Random();
            if (_r.Next(2) >= 1) dim *= -1;
            controller.flurryColor = controller.colors.odd;
            controller.flurryColor2 = controller.colors.even;

        }

        int step = 0;
        public override void go()
        {

            Color color = controller.colors.odd;
            if(dim>0) color= HSBColor.ShiftBrighness(controller.colors.odd, -200);
            for (int r = 0; r < 8; ++r)
            {
                controller.lights.setRailPar(r, color);
            }
            if(dim>0)
                controller.lights.setRailPar(step % 8, controller.colors.odd);
            else
                controller.lights.setRailPar(step % 8, Color.Black);

            ++step;
        }
    }
}
