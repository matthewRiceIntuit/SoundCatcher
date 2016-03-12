using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Blobs : SequenceBase
    {
        int[] blobPosition = new int[3];
        int[] blobState = new int[3];
        Color[] blobColor = new Color[3];
        Color[] pars = new Color[8];
        int blobs = 3;
        bool erratic = false;

        int flurrySeq = 0;

        public override void init()
        {
            blobColor[0] = Color.Red;
            blobColor[1] = Color.Blue;
            blobColor[2] = Color.Lime;

            blobPosition[0] = 0;
            blobPosition[1] = 3;
            blobPosition[2] = 6;

            blobState[0] = 0;
            blobState[1] = 1;
            blobState[2] = 2;

        }
 
        public override void go()
        {
            if ((int)controller.beatFadeVal==0)
            {
                for (int r = 0; r < blobs; ++r)
                {
                    ++blobState[r];

                   // if (blobState[r] == 1 && (!erratic || random.Next(2) == 0)) ++blobState[r];

                    if (blobState[r] > 3)
                    {
                        setPar(blobPosition[r], blobColor[r], -355);
                        blobPosition[r] = random.Next(10) - 1;
                        blobState[r] = 0;
                    }
                    
                }
                if (++flurrySeq > 2) flurrySeq = 0;
                controller.flurryColor = blobColor[flurrySeq];
                controller.flurryColor2 = blobColor[flurrySeq];
            }

            for (int r = 0; r < 8; ++r) pars[r] = Color.Black;

            for (int i = 0; i < 3; ++i)
            {
                if (blobState[i] == 0)
                {
                    drawBlob(blobPosition[i], blobColor[i], controller.beatDetect.beatLength - controller.beatFadeVal);
                }
                if (blobState[i] == 1)
                {
                    drawBlob(blobPosition[i], blobColor[i], 0);
                }
                if (blobState[i] == 2)
                {
                    drawBlob(blobPosition[i], blobColor[i], controller.beatFadeVal);
                }
  
            }

        }

        void drawBlob(int pos, Color c, double fade)
        {
            fade = (int)(1 / controller.beatDetect.beatLength * -400 * fade);
            setPar(pos, c,fade - 100);
            setPar(pos+1, c, fade);
            setPar(pos+2, c, fade - 100);
     
        }

        void setPar(int pos,Color c,double fade)
        {
            if (pos <0 || pos>7) return;
            c = HSBColor.ShiftBrighness(c, (float)(fade));
            pars[pos] = Color.FromArgb(255,
                c.R + pars[pos].R,
                c.G + pars[pos].G,
                c.B + pars[pos].B);
            controller.lights.setRailPar(pos,pars[pos]);
        }
        

    }
}
