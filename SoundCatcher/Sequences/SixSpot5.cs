using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SoundCatcher.Objects;

namespace SoundCatcher.Sequences
{
    class SixSpot5 : SequenceBase
    {
        Color c1,c2;
        int ChaseMode = 0;
        int ColorMode = 0;
        Color[] clr = new Color[6];
        List<Chase> chase1 = new List<Chase>();
        List<Chase> chase2 = new List<Chase>();
        List<Chase> chase3 = new List<Chase>();
        List<Chase> chase4 = new List<Chase>();
        List<Chase> chase5 = new List<Chase>();
        List<Chase> chase6 = new List<Chase>();
        List<Chase> chase7 = new List<Chase>();
        List<object> chases = new List<object>();

        List<Chase> sequence = null;
        double fadeDepth = 0;
        public SixSpot5()
        {
            chase1.Add(new Chase(0));
            chase1.Add(new Chase(1));
            chase1.Add(new Chase(2));
            chase1.Add(new Chase(3));
            chase1.Add(new Chase(4));
            chase1.Add(new Chase(5));

            chase2.Add(new Chase());
            chase2.Add(new Chase(0, 5));
            chase2.Add(new Chase(1, 4));
            chase2.Add(new Chase(2, 3));
            chase2.Add(new Chase(1, 4));
            chase2.Add(new Chase(0, 5));

            chase3.Add(new Chase(0, 1, 2));
            chase3.Add(new Chase(3, 4, 5));

            chase4.Add(new Chase(0));
            chase4.Add(new Chase(3));
            chase4.Add(new Chase(1));
            chase4.Add(new Chase(4));
            chase4.Add(new Chase(2));
            chase4.Add(new Chase(5));

            chase5.Add(new Chase(2, 3));
            chase5.Add(new Chase(0, 5));
            chase5.Add(new Chase(2, 4));
            chase5.Add(new Chase(1, 3));
            chase5.Add(new Chase(0, 5));
            chase5.Add(new Chase(1, 4));

            chase6.Add(new Chase(0, 5));
            chase6.Add(new Chase(0, 1));
            chase6.Add(new Chase(4, 5));
            chase6.Add(new Chase(1, 2));
            chase6.Add(new Chase(3, 4));
            chase6.Add(new Chase(2, 3));
            chase6.Add(new Chase(0, 1));

            chase7.Add(new Chase());
            chase7.Add(new Chase(1, 4));
            chase7.Add(new Chase(2, 3));
            chase7.Add(new Chase(0, 5));
            chase7.Add(new Chase(2, 3));
            chase7.Add(new Chase(1, 4));


            chases.Add(chase1);
            chases.Add(chase2);
            chases.Add(chase3);
            chases.Add(chase4);
            chases.Add(chase5);
            chases.Add(chase6);
            chases.Add(chase7);

        }

        
        public override void init()
        {
            controller.MatchRails = true;

            sequence = (List<Chase>) chases[random.Next(chases.Count)];
            controller.doFlurry = false;
            ticksPerCall = 1;

            if (random.Next(3) == 0)
            {
                clr[0] = clr[1] = clr[2] = controller.colors.even;
                clr[3] = clr[4] = clr[5] = controller.colors.odd;
            }
            else
            {
                clr[0] = clr[3] = Color.Red;
                clr[1] = clr[4] = Color.Yellow;
                clr[2] = clr[5] = Color.Blue;
            }
      
            controller.lights.setRailAll(Color.Black);
            //controller.lights.bFlipSiblingRail = (random.Next(2) == 0);
            fadeDepth = 1.6+ (random.Next(2) / 10);

            sequenceStep = 0;
            black = -250;
            fadeDepth = 1.6 + (random.Next(2) / 10);
        }
        int black = 0;
        int step = 0;
        int count = 0;
        bool[] fadePar = new bool[3];
        int fade=0;
        int sequenceStep = 0;
        int flurryClr = 0;
        public override void go()
        {
            if (controller.isBeat && controller.beatDetect.Step == 1)
            {
                controller.flurry.move();
            }

            if (controller.isBeat) {
               if (++sequenceStep >= sequence.Count) sequenceStep = 0;
                if(++flurryClr>5) flurryClr=0;
                controller.flurry.setRGBLeft(clr[flurryClr]);
                controller.flurry.setRGBRight(clr[flurryClr]);
            }


            for (int r = 0; r < 6; ++r) SetSixRail(r, clr[r]);
            if (sequence[sequenceStep].par1 >= 0)
                SetSixRail(sequence[sequenceStep].par1, HSBColor.ShiftBrighness(clr[sequence[sequenceStep].par1], (int)(controller.fadeDepth * fadeDepth)));
                //SetSixRail(sequence[sequenceStep].par1, Color.Black);//HSBColor.ShiftBrighness(clr[sequence[sequenceStep].par1], black ));
          
            if (sequence[sequenceStep].par2 >= 0)
                SetSixRail(sequence[sequenceStep].par2, HSBColor.ShiftBrighness(clr[sequence[sequenceStep].par2], (int)(controller.fadeDepth * fadeDepth)));
                //SetSixRail(sequence[sequenceStep].par2, Color.Black);//HSBColor.ShiftBrighness(clr[sequence[sequenceStep].par2], black));
            
            if (sequence[sequenceStep].par3 >= 0)
                SetSixRail(sequence[sequenceStep].par3, HSBColor.ShiftBrighness(clr[sequence[sequenceStep].par3], (int)(controller.fadeDepth * fadeDepth)));
                //SetSixRail(sequence[sequenceStep].par3, Color.Black);//HSBColor.ShiftBrighness(clr[sequence[sequenceStep].par3], black));
            
         }



        Color[] colorArray = new Color[6];
        void SetSixRail(int par,Color color)
        {
           // Console.WriteLine("par:" + par + " color:" + color.ToString());
            colorArray[par] = color;
       
            if (par == 0)
            {
               // controller.flurry.setRGBLeft(color);
                controller.lights.setRailBoth(0, color);
                controller.lights.setRailBoth(1, color);
            }
            if (par == 1)
            {
                controller.lights.setRailBoth(3, color);
                controller.lights.setRailBoth(4, color);
            }
            if (par == 2)
            {
                controller.lights.setRailBoth(6, color);
                controller.lights.setRailBoth(7, color);
            }
            if (par == 3)
            {
                controller.lights.setRailBoth(8, color);
                controller.lights.setRailBoth(9, color);
            }
            if (par == 4)
            {
                controller.lights.setRailBoth( 11, color);
                controller.lights.setRailBoth(12, color);
            }
            if (par == 5)
            {
                controller.lights.setRailBoth(14, color);
                controller.lights.setRailBoth(15, color);
            }

        }

    }
}
