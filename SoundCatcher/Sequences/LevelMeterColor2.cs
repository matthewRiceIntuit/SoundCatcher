using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class LevelMeterColor2 : SequenceBase
    {
        Color[] pars = new Color[8];
        Color[] colors = new Color[8];

        public LevelMeterColor2()
        {
            ticksPerCall = 12;
            for (int r = 0; r < 8; ++r)
            {
                colors[r] = HSBColor.ShiftHue(Color.Blue, 210 / -8 * r);
            }
            colors[0] = Color.Blue;
            colors[7] = Color.Red;
            controller.flurryColor = Color.White;
            controller.flurryColor2 = Color.White;

        }
        bool onBeat = false;
        int fade = 8;
        int colorMode = 0;

        public override void init()
        {
            onBeat = false;
            if (random.Next(2) == 0) onBeat = true;
            fade = 7;
            if (random.Next(2) == 0) fade = 8;
            fade = 7;

            colorMode = random.Next(4);
            colorMode = 0;

        }

        public override void go()
        {


            int intensity = 0;
            if (onBeat == false) intensity = (int)controller.beatDetect.Intensity;
            if (controller.isBeat) intensity = 255;
            if (intensity > 255) intensity = 255;
            if (intensity < 180) intensity = 0;

            // fade current settings;
            for (int r = 0; r < fade; ++r)
            {
                pars[r] = HSBColor.ShiftBrighness(pars[r], -10 * r);
                controller.lights.setRailPar(r, pars[r]);
            }
            if (fade == 7)
            {
                pars[7] = HSBColor.ShiftBrighness(pars[7], -20);
                controller.lights.setRailPar(7, pars[7]);
            }

            int par = (int)(intensity / 32);
            for (int r = 0; r <= par; ++r)
            {
                pars[r] = getColor(255, r);
                if (pars[1] == Color.Black) pars[0] = Color.Black;
                controller.lights.setRailPar(r, pars[r]);
            }

        }

        Color getColor(int intensity, int r)
        {
            if (colorMode == 0)
            {
                Color color = Color.Lime;
                if (r > 3) color = Color.Yellow;
                if (r > 5) color = Color.Red;

                return HSBColor.ShiftBrighness(color, -255 + intensity);
            }
            if (colorMode == 1)
            {
                return controller.colors.even;
            }
            if (colorMode == 2)
            {
                return HSBColor.ShiftBrighness(colors[r], -255 + intensity);
            }

            return controller.colors.odd;

        }
    }
}
