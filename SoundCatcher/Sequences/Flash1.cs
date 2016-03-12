using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Flash1 : SequenceBase
    {
        
        public Flash1()
        {
            if(colors==null) colors = new ColorDuo(Color.Red, Color.Red);
        }

        Color oddColor,evenColor = Color.White;
        public override void init()
        {
            oddColor = (random.Next(2) == 0) ? controller.colors.even : controller.colors.odd;
            ticksPerCall = 1;

        }
        int step = 0;
        ColorDuo colors;
        bool odd = false;
        public override void go()
        {
         
            //Console.WriteLine("step:" + controller.step + " isBeat:" + controller.isBeat.ToString());
            if (controller.isBeat)// && controller.beatDetect.Step % 2==0 )
            {

                if (odd)
                {
                    // step = 0;
                    controller.flurryColor2 = controller.colors.odd; 
                    colors.odd = HSBColor.ShiftBrighness(controller.colors.odd, -240);
                    colors.even = controller.colors.even;
                }
                else
                {
                    controller.flurryColor2 = controller.colors.even;
                    colors.odd = HSBColor.ShiftBrighness(controller.colors.even, -240);
                    colors.even = controller.colors.odd;
                }
                
               
                odd = !odd;
            }
            //if (++step > 50) return;
            colors.even = HSBColor.ShiftBrighness(colors.even, -10);
            if (odd)
            {
                controller.lights.setRailEven(colors.even);
                controller.lights.setRailOdd(colors.odd);
            }
            else
            {
                controller.lights.setRailEven(colors.odd);
                controller.lights.setRailOdd(colors.even);
            }
            controller.flurryColor = colors.even;
        }
    }
}
