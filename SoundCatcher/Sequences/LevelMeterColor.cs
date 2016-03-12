using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class LevelMeterColor : SequenceBase
    {
        Color[] pars = new Color[8];
        Color[] colors = new Color[8];
        Color black = Color.Black;
        public LevelMeterColor()
        {
            ticksPerCall = 2;
            for (int r = 0; r < 8; ++r)
            {
                colors[r] = HSBColor.ShiftHue(Color.Blue, 210 / -8 * r);
            }
            colors[0] = Color.Blue;
            colors[7] = Color.Red;
        }
        bool onBeat = false;
        int fade = 8;
        int colorMode = 0;
        int flurryColor = 0;
        public override void init()
        {
            controller.doFlurry = false;
            ticksPerCall = 1;
            onBeat = false;
            if (coinFlip()) onBeat = true;

            if (random.Next(2) == 0) fade = 8;
            
           
            colorMode = random.Next(4);
            
            black = Color.Black; 
            if (colorMode == 1 && coinFlip()) black = HSBColor.ShiftBrighness(controller.colors.odd, -200);
            controller.flurryColor = Color.White;
            controller.flurryColor2 = Color.White;
 
        }

        int pause = 0;
        Color c = Color.Black;
        
        public override void go()
        {

            if(controller.isBeat) if(++flurryColor >7) flurryColor=0;
            double intensity = 0;
            intensity = (controller.beatDetect.Amplitude );
            if (controller.isBeat) intensity += 55;
            if (intensity > 255) intensity = 255;
           // if (intensity > 200) pause = 6;
            //if (--pause > 0) intensity = 0;
            if (intensity < 50) intensity = 0;
 

            // fade current settings;
            for (int r = 0; r < fade; ++r)
            {
                pars[r] = HSBColor.ShiftBrighness(pars[r], -10 * (r+1));
                controller.lights.setRailPar(r, pars[r]);
                if (black != Color.Black)
                if(pars[r].GetBrightness() <= .1f) controller.lights.setRailPar(r, black);
            }
            if (fade==7)
            {
                pars[7] = HSBColor.ShiftBrighness(pars[7], -20);
                controller.lights.setRailPar(7, pars[7]);
            }
            //if (black != Color.Black) controller.lights.setRailAll(black);

            int par = (int)(intensity / 32);
            if (par == 0) return;
            for (int r = 0; r <= par; ++r)
            {
                pars[r] = getColor(r);
                if (pars[1] == Color.Black) pars[0] = Color.Black;
                controller.lights.setRailPar(r, pars[r]);
            }
            Color c2 = HSBColor.ShiftBrighness(getColor(par), -290 + (par * 32));
            c = HSBColor.ShiftBrighness(c, -5);
            if (c.GetBrightness() < c2.GetBrightness()) c = c2;

            controller.flurry.setAllRGB(c);
        }

        Color getColor(int r)
        {
            
            if (colorMode == 0)
            {
                Color color = Color.Lime;
                if (r > 3) color = Color.Yellow;
                if (r > 5) color = Color.Red;

                return color;
            }
            if (colorMode == 1)
            {
                return controller.colors.even;
            }
            if (colorMode == 2)
            {
                return colors[r];
            }

            return controller.colors.odd;
            
        }
    }
}
