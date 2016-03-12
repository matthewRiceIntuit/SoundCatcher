using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class OverLapping2 : SequenceBase
    {
        public override void init()
        {
            ticksPerCall = 3;
            controller.lights.fade = 0;//2f;
            controller.lights.bFlipSiblingRail = random.Next(2) == 0;
            
        }
        int flash = 0;
        int step = -1;
        int direction = 7;
        Color color1 = Color.Blue;
        Color color2 = Color.Red;
        bool waitForBeat = true;
        public override void go()
        {

            if (waitForBeat && !controller.isBeat) return;
            waitForBeat = false;
 
            ++step;
            if (step < 8)
            {
                for (int r = 0; r <= step; ++r)
                {
                    Color c = HSBColor.ShiftBrighness(color1, (r-step) * 35);
                    controller.lights.setRailPar(Math.Abs(direction-r), c);
                }
                controller.flurryColor = color1;
                controller.flurryColor2 = color2;
                if (step == 7) waitForBeat = true;
                return;
            }

            if (step < 16)
            {
                for (int r = 0; r <= step-8; ++r)
                {
                    Color c = HSBColor.ShiftBrighness(color2, (r - (step-8)) * 35);
                    controller.lights.setRailPar(Math.Abs(direction - r), c);
                }
 
                return;
            }
            step = -1;
            waitForBeat = true;
            if (direction > 0)
            {
                direction = 0;
                color1 = Color.Blue;
                color2 = Color.Red;
                controller.flurryColor = color1;
                controller.flurryColor2 = color2;
            }
            else
            {
                color1 = Color.Lime;
                color2 = Color.Red;
                controller.flurryColor2 = color1;
                controller.flurryColor = color2;

                direction = 7;
            }



            //ColorDuo duo = controller.GetNextColor();
            //color1 = duo.even;
            //color2 = duo.odd;


   
        }
  
    }
}
