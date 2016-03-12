using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class CellAtomota : SequenceBase
    {
        int fade = 5;
        int steps = 0;
        int lights = 0;
        bool redBlue = true;
        Color color1;
        Color color2;
        public override void init()
        {

            controller.doFlurry = false;
            
            fade = (random.Next(50) + 15) * -1;
            lights = random.Next(8)+1;
            steps = random.Next(12)+1;
            ticksPerCall = 1;
            controller.lights.fade = 0;
            redBlue = coinFlip();
            for (int r = 0; r < state.Length; ++r)
            {
                stateAges[r] = 0;
                if (random.Next(2) == 0) { state[r] = 1; }
                //state[r] = 1;
            }
            state[13] = 2;
            stateAges[13] = 255;
            color1 = controller.colors.even;
            color2 = controller.colors.odd;
        }
        int flash = 0;
        int[] state = new int[22];
        int[] stateAges = new int[22];
        Color[] colors = new Color[16];
        int step = 0;
        Color flurry = Color.Black;
        bool odd = true;
        public override void go()
        {

            if (controller.isBeat && random.Next(8)==0)
            {
                controller.flurry.move();
                odd = !odd;
                Color c = (odd) ? color1 : color2;
                controller.flurry.setAllRGB(c);
            }
 

            int[] nextState = (int[])state.Clone();
            for (int r = 1; r < state.Length-1; ++r)
            {
                if (state[r] == 1)
                {
                    stateAges[r]+=5;
                    if (stateAges[r] > 34)
                    {
                        stateAges[r] = 255;
                        if (state[r - 1] == 0)
                        {
                            nextState[r - 1] = 1;
                            stateAges[r - 1] = 1;
                        }
                        if (state[r + 1] == 0)
                        {
                            nextState[r + 1] = 1;
                            stateAges[r + 1] = 1;
                        }
                    }

                    if (random.Next(200) == 0)
                    {
                        nextState[r] = 2;
                     }
                }
                if (state[r] == 2)
                {
                    stateAges[r]-=33;
                    if (stateAges[r] < 0)
                    {
                        stateAges[r] = 0;
                        nextState[r] = 0;
                        continue;
                    }
                    if(stateAges[r] < 120)
                    {
                        if (state[r - 1] == 1)
                        {
                            nextState[r - 1] = 2;
                            stateAges[r - 1] = 255;
                        }
                        if (state[r + 1] == 1)
                        {
                            nextState[r + 1] = 2;
                            stateAges[r + 1] = 255;
                        }
                    }
                }
                if(state[r]==0 && random.Next(100)==0)
                {
                    nextState[r] = 1;
                    stateAges[r] = 1;
                }
            }

            nextState.CopyTo(state,0);

            for (int r = 0; r < 16; ++r)
            {
                int i = r+2;
                Color c = Color.Black;
                if (state[i] == 1)
                {

                    c = HSBColor.ShiftBrighness(color1,Math.Max(-255,-255+stateAges[i]));
                }
                if (state[i] == 2)
                {

                    c = HSBColor.ShiftBrighness(color2, Math.Max(-255, -255 + stateAges[i]));
                }
                controller.lights.setRailBoth(r, c);
            }
            
         }

    }
}
