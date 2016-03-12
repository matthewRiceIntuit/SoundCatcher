using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class LevelMeterColor4 : SequenceBase
    {
        Color[] pars = new Color[8];


        bool onBeat = false;
        public override void init()
        {
            onBeat = false;
            if (random.Next(2) == 0) onBeat = true;
        }

        public override void go()
        {


            float intensity = 0;
            if (onBeat == false) intensity = controller.beatDetect.Intensity;
            if (controller.isBeat) intensity = 255;
            if (intensity > 255) intensity = 255;
            if (intensity < 180) intensity = 0;
 
            // fade current settings;
            for (int r = 0; r < 8; ++r)
            {
                pars[r] = HSBColor.ShiftBrighness(pars[r], -10 * (8-r));
                controller.lights.setRailPar(r, pars[r]);
            }

            int par = (int)(intensity / 32);
            for (int r = 0; r <= par; ++r)
            {
                pars[r] = getColor(255, r);
                if (r == 0)
                {
                    int value = (int)(intensity - (r * 32)) * 8;
                    pars[r] = getColor(value, r);
                    controller.lights.setRailPar(r, pars[r]);
                    if (pars[1].GetBrightness() > pars[0].GetBrightness()) pars[0] = pars[1];

                    //pars[r - 1] = HSBColor.ShiftBrighness(pars[r],(255-blue)/-2);

                    //controller.lights.setRailPar(r-1, getColor(blue));


                }
                controller.lights.setRailPar(r, pars[r]);
                //controller.lights.setRailPar(r, ColorHelper.getFireColorBlue(intensity));
            }

        }

        Color getColor(int intensity, int r)
        {
            return HSBColor.ShiftBrighness(controller.colors.even, -255 + intensity);
        }
    }
}
