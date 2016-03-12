using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class RunWayChase : SequenceBase
    {
        public override void init()
        {
            ticksPerCall = 1;
            //controller.lights.bFlipSiblingRail = random.Next(2) == 0;
            controller.flurryColor = controller.colors.even;
        }
        int step = 0;
        public override void go()
        {
            int r= step;
            setRail((r ) % 8, HSBColor.ShiftBrighness(controller.colors.even, -254));
            setRail((r + 1) % 8, HSBColor.ShiftBrighness(controller.colors.even, -200));
            setRail((r + 2) % 8,  controller.colors.even);
            setRail((r + 3) % 8, Color.Black);
            setRail((r + 4) % 8, HSBColor.ShiftBrighness(controller.colors.even, -254));
            setRail((r + 5) % 8, HSBColor.ShiftBrighness(controller.colors.even, -200));
            setRail((r + 6) % 8, controller.colors.even);
            setRail((r + 7) % 8, Color.Black);
            ++step;
            
        }
        void setRail(int i, Color c)
        {
            if (i > 8) return;
            controller.lights.setRailPar(i,c);

        }
    }
}
