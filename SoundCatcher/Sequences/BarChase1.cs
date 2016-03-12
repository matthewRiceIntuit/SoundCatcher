using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class BarChase1 : SequenceBase
    {
        public override void init()
        {
            //controller.lights.fade = 1.1f;
        }
        int step = 2;
        Color[] pars = new Color[8];
        public override void go()
        {
            doFade();
            if (!controller.isBeat) return;
            Done = false;
            for (int r = 0; r < step; ++r)
            {
                pars[r] = controller.colors.even;
                controller.flurryColor = controller.colors.even;
                controller.flurryColor2 = controller.colors.odd;
            }
            for (int r = step; r < 8; ++r)
            {
                pars[r] = Color.Black;// ColorHelper.Dim(controller.colors.even);
            }
            ++step;
            if (++step > 8)
            {
                step =2;
                Done = true;
            }
            doFade();
        }
        void doFade()
        {
            for (int r = 0; r < 8; ++r)
            {
                controller.lights.setRailPar(r,HSBColor.ShiftBrighness(pars[r],controller.fadeDepth));
            }
        }
    }
}
