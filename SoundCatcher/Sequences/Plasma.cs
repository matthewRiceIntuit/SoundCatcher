using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Plasma : SequenceBase
    {
 
         int paletteShift = 0;
         Color[] palette = new Color[256];
         int[,] cls;


         public override void init()
         {
             Random _r = new Random();
             int style = _r.Next(3);
             if (style == 0)
             {
                 for (int r = 0; r <= 127; ++r)
                 {
                     palette[r] = Color.FromArgb(0, 0, r / 16);
                     palette[r + 127] = Color.FromArgb(0, 0, 127 - r / 16);
                 }
             }
             if (style == 1)
             {
                 for (int r = 0; r <= 127; ++r)
                 {
                     palette[r] = Color.FromArgb(4 - r / 32, 0, r / 16);
                     palette[r + 127] = Color.FromArgb(r / 32, 0, 8 - r / 16);
                 }
             }
             if (style == 2)
             {
                 for (int r = 0; r < 255; ++r)
                 {
                     palette[r] = HSBColor.ShiftHue(Color.Blue,  r*-1);
                 }
             }
         }
        public override void go()
        {

            int w = 8;
            int h = 8;

            if (cls == null)
            {
                cls = new int[w, h];
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        cls[x, y] = (int)(
                        (127.5 + +(127.5 * Math.Sin(x)))
                        + (127.5 + +(127.5 * Math.Sin(y)))
                        + (127.5 + +(127.5 * Math.Sin(Math.Sqrt((x * x + y * y)))))
                        ) / 3;
                    }
                }
                //for (int r = 0; r <= 127; ++r)
                //{
                //    palette[r] = Color.FromArgb(4 - r/32, 0, r/16);
                //    palette[r + 127] = Color.FromArgb(r/32, 0, 8 - r/16);
                //}

            }
            Color[,] colors = new Color[w, h];           
            paletteShift +=1;//= Convert.ToInt32(Environment.TickCount/1000 );
            for (int y = 0; y < h; y += 1)
            {
                Color color = palette[(cls[3, y] + paletteShift) % 255];
                controller.lights.setRailPar(y,color);

            }

        }

    }
}
