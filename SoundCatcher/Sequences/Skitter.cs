using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Skitter : SequenceBase
    {
        int fade = 5;
        int steps = 0;
        int lights = 0;
        bool redBlue = true;
        public override void init()
        {
            //controller.lights.fade = 4f;
            ticksPerCall = random.Next(2)+1;
            controller.doFlurry = false;
            controller.flurry.setAllRGB(Color.White);
        }
        int flash = 0;
        Color[] pars = new Color[16];
        int[] blips = new int[64];

        int step = 0;
        bool odd = false;
        int pause=0;
        public override void go()
        {
            if (controller.isBeat) pause = 6;
            if (controller.isBeat && controller.beatDetect.Step == 1)
            {
                controller.flurry.setAllRGB(Color.White);
                controller.flurryScenes.Go("");
            }

            if (--pause > 0) return;
           // if (random.Next(3) == 0) return;

            for (int r = 0; r < 16; ++r) pars[r] = Color.Black;
            odd = !odd;
            if(odd) if (++step > 16) step = 0;

            blips[step] = (random.Next(3)==0)?1:0;
            for (int r = 0; r < 16; ++r)
            {
                int index = r + step;
                if (index > 16) index -= 16;
                if (odd && r % 2 == 0) continue;
                if (!odd && r % 2 != 0) continue;
             
                controller.lights.setRailBoth(r, (blips[index] == 1)?Color.White:Color.Black);
                if(blips[index]==1) controller.lights.setRailBoth(r+1,  Color.Black);
            }
        }

    }
}