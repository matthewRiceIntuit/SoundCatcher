using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class SixSpot2 : SequenceBase
    {
        Color c1,c2;
        int ChaseMode = 0;
        int ColorMode = 0;
        Color odd, even, black;

        public override void init()
        {
            ticksPerCall = 1;
            step = -1;
            controller.MatchRails = true;

            
            controller.lights.bFlipSiblingRail = coinFlip();

            if (coinFlip())
            {
                odd = controller.colors.odd;
                even = controller.colors.even;
            }
            else
            {
                even = controller.colors.odd;
                odd = controller.colors.even;
            }

            black = (coinFlip()) ? Color.Black : HSBColor.ShiftBrighness(odd, -200);
            black =  HSBColor.ShiftBrighness(odd, -250);

            controller.lights.setRailAll(Color.Black);
            for (int r = 0; r < 6; ++r) SetSixRail(r, black);
 

            ChaseMode = 1; // _r.Next(2);
            
            done();
        }

        int step = 0;
        int count = 0;
        public override void go()
        {
            doFade();
            if (!controller.isBeat) return;
            Done = false;
            ++step;
            if (ChaseMode == 0)
            {
                if (step < 6)
                {
                    SetSixRail(step, getColor(step));
                }
                else
                {
                    done();
                }
            }
            if (ChaseMode == 1)
            {
                if (step ==0)
                {
                    SetSixRail(0, getColor(0));
                    SetSixRail(5, getColor(5));
                }
                if (step == 1)
                {
                    SetSixRail(1, getColor(1));
                    SetSixRail(4, getColor(4));
                }
                if (step == 2)
                {
                    SetSixRail(2, getColor(2));
                    SetSixRail(3, getColor(3));
                }
                if (step == 3)
                {
                    SetSixRail(2, black);
                    SetSixRail(3, black);
                }
                if (step == 4)
                {
                    SetSixRail(1, black);
                    SetSixRail(4, black);
                }
                if (step == 5)
                {
                    SetSixRail(0, black);
                    SetSixRail(5, black);
                    done();
                }
            }
         }

        void done()
        {
            step = -1;
            Done = true;
 
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

        Color getColor(int par)
        {
            Color c = _getColor(par);
            return c;
        }

        Color _getColor(int par)
        {
            if (ColorMode == 0) return odd;
            if (ColorMode == 1)
            {
                Color c = even;
                if (par == 1 || par == 4) c = odd;
                return c;
            }
            if (ColorMode == 2)
            {
                Color c = odd;
                if (par == 1 || par == 4) c = even;
                return c;
            }
            
  
            return even;
        }
    }
}
