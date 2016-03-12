using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class EqDrops : SequenceBase
    {
        Color[] pars = new Color[16];
        double min, max;
        int colorSpeed;
        int mode = 0;
        int colorStep = 0;
        Color[] colors = new Color[3];
        public override void init()
        {
            //controller.doFlurry = false;
            //controller.lights.bFlipSiblingRail = coinFlip();
            ticksPerCall = 1;
            min = 255;
            max = 0;
            for (int r = 0; r < 16; ++r) pars[r] = Color.Black;

            colorSpeed = random.Next(9);
            mode =  random.Next(2);
            colors[0] = Color.Red;
            colors[1] = Color.Yellow;
            colors[2] = Color.Blue;


        }

        int step = 280;
        Color c;
        double[] bandsFade = new double[30];

        public override void go()
        {
            //if (controller.isBeat) return;
  
            int band=0;
            int bandCount = 0;
            int bandSize = 11;
            double maxBand = 0;
            double[] bands = new double[30];
          
            for (int i = 10; i < controller.beatDetect.last_fftArray.Length; i+=11)
            {
                bands[band] = controller.beatDetect.last_fftArray[i] * controller.beatDetect.last_fftArray[i];
                if (bands[band] > maxBand) maxBand = bands[band];
                ++band;
            }

            for (int r = 0; r <= band; ++r)
            {
                bands[r] = bands[r] / 3000 * 255;
                if (bands[r] < 220) bands[r] = 0;
                bandsFade[r]-=21;
                if (bands[r] > bandsFade[r]) bandsFade[r] = bands[r];
            }
            
            
            
            
            // if (random.Next(3) == 0) return;
            if (colorSpeed < 7)
            {
                step -= colorSpeed;
                if (step < 0) step = 280;


                 c = HSBColor.ShiftHue(Color.Blue, (step * -1) + 100);
            }
            else
            {
                if (controller.isBeat) if (--colorStep < 0) colorStep = 2;
                c = colors[colorStep];
            }

            controller.flurryColor = c;
            controller.flurryColor2 = c;
            if (mode == 0)
            {
                for (int x = 0; x < 4; x++) //_fftArray.Length
                {

                    controller.lights.setRailBoth(8 + x, HSBColor.ShiftBrighness(c, (float)(-255.0 + bandsFade[x * 2 + 7])));
                    controller.lights.setRailBoth(15 - x, HSBColor.ShiftBrighness(c, (float)(-255.0 + bandsFade[x * 2 + 7])));
                    controller.lights.setRailBoth(7 - x, HSBColor.ShiftBrighness(c, (float)(-255.0 + bandsFade[x * 2 + 7])));
                    controller.lights.setRailBoth(x, HSBColor.ShiftBrighness(c, (float)(-255.0 + bandsFade[x * 2 + 7])));
                }
            }
            else
            {
                for (int x = 0; x < 8; x++) //_fftArray.Length
                {

                    controller.lights.setRailBoth(15 - x, HSBColor.ShiftBrighness(c, (float)(-255.0 + bandsFade[x + 8])));
                    controller.lights.setRailBoth(x, HSBColor.ShiftBrighness(c, (float)(-255.0 + bandsFade[x + 8])));
                }
            }
           
        }

    }
}
