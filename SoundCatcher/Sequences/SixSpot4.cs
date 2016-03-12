using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class SixSpot4 : SequenceBase
    {
        Color c1,c2;
        int ChaseMode = 0;
        int ColorMode = 0;
        Color[] clr = new Color[3];

        double fadeDepth = 0;
        public override void init()
        {
            controller.doFlurry = false;
            ticksPerCall = 1;

            if (random.Next(3) == 0)
            {
                clr[0] = clr[2] = controller.colors.even;
                clr[1] = controller.colors.odd;
            }
            else
            {
                clr[0] = Color.Red;
                clr[1] = Color.Yellow;
                clr[2] = Color.Blue;
            }
      
            controller.lights.setRailAll(Color.Black);
            controller.lights.bFlipSiblingRail = (random.Next(2) == 0);
            fadeDepth = 1.6+ (random.Next(2) / 10);
            controller.MatchRails = true;
      

        }

        int step = 0;
        int count = 0;
        bool[] fadePar = new bool[3];
        int fade=0;
        public override void go()
        {
            Done = false;          
            
            for (int r = 0; r < 3; ++r)
            {
                SetSixRail(r,clr[r]);
                SetSixRail(r+3, clr[r]);
                if (fadePar[r] == false) continue;
                SetSixRail(r, HSBColor.ShiftBrighness(clr[r],(int)(controller.fadeDepth*fadeDepth)));
                SetSixRail(r + 3, HSBColor.ShiftBrighness(clr[r], (int)(controller.fadeDepth * fadeDepth)));
            }
            if (!controller.isBeat) return;
            if (controller.beatDetect.Step == 1) controller.flurryScenes.Go("");
            
            if (++step < 2) return;
            step = 0;
            
            fade = 0;
            fadePar[0] = false; fadePar[1] = false; fadePar[2] = false;
        
            if (random.Next(3) == 0)
            {
                int par1 = random.Next(3);
                int par2 = random.Next(3);
                Color tmp = clr[par1];
                clr[par1] = clr[par2];
                clr[par2] = tmp;
                fadePar[par1] = fadePar[par2] = true;
            }
  
            Done = true;
         }

 

        Color[] colorArray = new Color[6];
        void SetSixRail(int par,Color color)
        {
           // Console.WriteLine("par:" + par + " color:" + color.ToString());
            colorArray[par] = color;
       
            if (par == 2)
            {
                controller.flurry.setOddRGB(color);
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
                if(controller.lights.bFlipSiblingRail == false) controller.flurry.setEvenRGB(color);
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
                if (controller.lights.bFlipSiblingRail != false) controller.flurry.setEvenRGB(color);
                controller.lights.setRailParRight(6, color);
                controller.lights.setRailParRight(7, color);
            }

        }

    }
}
