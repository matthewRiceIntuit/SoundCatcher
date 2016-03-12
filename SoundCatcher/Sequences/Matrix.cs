using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Matrix : SequenceBase
    {
        public override void init()
        {

            controller.doFlurry = false;
            
            ticksPerCall = 5;
            controller.lights.fade = 0;
            gradient(15);
            gradient(0);
            gradient(7);

            for (int r = 0; r < 20; ++r)
            {
                highlites[r] = random.Next(50);
            }
        
        }

        int[] highlites = new int[20];
        int[] steps = new int[30];
        int step = -1;
        double highlight = 0;
        public override void go()
        {
            if(++step>=30) step=0;
            highlight -= 1.05;
            if (highlight < 0) highlight = 16;
            
            if (step == 0) gradient(19);
            if (step == 9) gradient(0);
            if (step == 19) gradient(9);
            int i = step;
            for (int r = 0; r < 16; ++r)
            {
                Color c = HSBColor.ShiftBrighness(Color.Lime, -265 + steps[i] );
                if ((int)highlight == r) c = Color.Lime;
               // c = HSBColor.ShiftBrighness(c, );
                controller.lights.setRailBoth(r, c);
                if (++i >= 30) i = 0;
            }
        }

        private void gradient(int step)
        {
            int fade = 40 +  random.Next(60);
            int intensity = 255;
            for (int r = step; r <= step + 8; ++r)
            {
                steps[r] = intensity;
                intensity -= fade;
                if (intensity < 0) intensity = 0;
            }
        }


    }
}
