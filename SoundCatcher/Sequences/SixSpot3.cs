using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class SixSpot3 : SequenceBase
    {
        Color c1,c2;
        int ChaseMode = 0;
        int ColorMode = 0;
        Color[] clr = new Color[3];
       
        public override void init()
        {
            ticksPerCall = 1;
            controller.MatchRails = true;

           
            clr[0] = Color.Red;
            clr[1] = Color.Yellow;
            clr[2] = Color.Blue;
      
            controller.lights.setRailAll(Color.Black);
      

        }

        int step = 0;
        int count = 0;
        public override void go()
        {
            doFade();
            if (!controller.isBeat) return;

            Color tmp = clr[0];
            clr[0] = clr[2];
            clr[2] = tmp;
        
            if (random.Next(2) == 0)
            {
                tmp = clr[0];
                clr[0] = clr[1];
                clr[1] = tmp;
            }

            int brightPar = random.Next(3);
            
            SetSixRail(0, HSBColor.ShiftBrighness(clr[0],-200));
            SetSixRail(1, HSBColor.ShiftBrighness(clr[1],-200));
            SetSixRail(2, HSBColor.ShiftBrighness(clr[2],-200));
            SetSixRail(brightPar,clr[brightPar]);  
           
            if (random.Next(2) == 0)
            {
                SetSixRail(brightPar+3,clr[brightPar]);  
                SetSixRail(3, HSBColor.ShiftBrighness(clr[0],-200));
                SetSixRail(4, HSBColor.ShiftBrighness(clr[1],-200));
                SetSixRail(5, HSBColor.ShiftBrighness(clr[2],-200));
            }
            else
            {
                SetSixRail(3, HSBColor.ShiftBrighness(clr[2],-200));
                SetSixRail(4, HSBColor.ShiftBrighness(clr[1],-200));
                SetSixRail(5, HSBColor.ShiftBrighness(clr[0],-200));
                SetSixRail(5-brightPar,clr[brightPar]);  
         
            }

         }

        void doFade()
        {
            for (int r = 0; r < 6; ++r)
            {
                if(colorArray[r].GetBrightness()>.1f)//= destIntensity[r])
                    _SetSixRail(r,HSBColor.ShiftBrighness(colorArray[r],-20));
            }
        }


        Color[] colorArray = new Color[6];
        float[] destIntensity = new float[6];
        void SetSixRail(int par, Color color)
        {
            destIntensity[par] = HSBColor.ShiftBrighness(color,-30).GetBrightness();
            _SetSixRail(par, color);
        }
        void _SetSixRail(int par,Color color)
        {
           // Console.WriteLine("par:" + par + " color:" + color.ToString());
            colorArray[par] = color;
            if (par == 2)
            {
                controller.lights.setRailParLeft(0, color);
                controller.lights.setRailParLeft(1, color);
            }
            if (par == 1)
            {
                controller.lights.setRailParLeft(3, color);
                controller.lights.setRailParLeft(4, color);
            }
            if (par == 0)
            {
                controller.lights.setRailParLeft(6, color);
                controller.lights.setRailParLeft(7, color);
            }
            if (par == 3)
            {
                controller.lights.setRailParRight(0, color);
                controller.lights.setRailParRight(1, color);
            }
            if (par == 4)
            {
                controller.lights.setRailParRight(3, color);
                controller.lights.setRailParRight(4, color);
            }
            if (par == 5)
            {
                controller.lights.setRailParRight(6, color);
                controller.lights.setRailParRight(7, color);
            }

        }

    }
}
