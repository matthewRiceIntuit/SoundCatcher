using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class LevelMeterColor5 : SequenceBase
    {
        Color[] pars = new Color[8];
        Color[] colors = new Color[8];

        public LevelMeterColor5()
        {
            for(int r=0;r<8;++r)
            {
                colors[r] = HSBColor.ShiftHue(Color.Blue, 210 / -8 * r);
            }
            colors[0] = Color.Blue;
            colors[7] = Color.Red;
        }

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
                pars[r] = HSBColor.ShiftBrighness(pars[r], -10 * r);
                controller.lights.setRailPar(r, pars[r]);
            }

            int par = (int)(intensity / 32);
            for (int r = 0; r <= par; ++r)
            {
                pars[r] = getColor(255, r);
                if (r < 6 && par == r)
                {
                    int value = (int)(intensity - (r * 32)) * 8;
                    pars[r] = getColor(value, r);
                    controller.lights.setRailPar(r, pars[r]);
                    if (pars[r + 1].GetBrightness() > pars[r].GetBrightness()) pars[r] = getColor(255, r);
                    if (par == 0)
                    {
                        if (pars[0].GetBrightness() < .009) pars[0] = getColor(5, 0);
                    }
                }
                controller.lights.setRailPar(r, pars[r]);
            }
        }

        Color getColor(int intensity, int r)
        {
            return HSBColor.ShiftBrighness(colors[r], -255 + intensity);
        }
    }
}
