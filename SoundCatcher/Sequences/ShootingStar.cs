using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class ShootingStar : SequenceBase
    {

        Color headColor, tailColor;
        int headSize = 1;
        int tailSize = 14;
        int direction = 1;
        double accel = .1;//.15;
        double pos = 0;
        bool white;
        ColorDuo[] colors = new ColorDuo[3];
        int colorChoice = 0;
        public override void init()
        {
            colors[0] = new ColorDuo(Color.Blue, Color.Red);
            colors[1] = new ColorDuo(Color.Red, Color.Green);
            colors[2] = new ColorDuo(Color.Green,Color.Blue);


            headColor = Color.Red; //controller.colors.even;
            tailColor = Color.Blue;// controller.colors.odd;
            white = coinFlip();
            //controller.doFlurry = false;
            controller.MatchRails = true;
            ticksPerCall = 1;
            controller.lights.fade = 1.5f;
        }

        Color c;
        bool reverse = false;
        bool pause = false;
        public override void go()
        {
            controller.lights.setRailAll(Color.Black);
            if (pause && !controller.isBeat) return;
            pause = false;

            accel += (.051 * direction);
            if (accel < .01) accel = .01;
            pos += (accel * direction);
            
            if (direction > 0 && pos - (headSize + tailSize) > 18)
            {
                //accel -= .4;
                direction = -1;
                pos = 16;
            }
            if (direction < 0 && pos + headSize + tailSize < 9)
            {
                accel = .3;
                direction = 1;
                reverse = !reverse;
                if (++colorChoice > 2) colorChoice = 0;

                headColor = colors[colorChoice].even;
                tailColor = colors[colorChoice].odd;
                pos = -1;
                pause = true;
               
            }

            int tmpHeadSize = headSize + (int)(pos / 3);
            for (int r = 0; r < tmpHeadSize; ++r)
            {
                int par = (int)pos - (r*direction);
                setPar(par,white?Color.White: headColor);
            }

            float fade = -255 / tailSize * 1.0f;
            for (int r = 0; r < tailSize; ++r)
            {
                int par = (int)pos - ((tmpHeadSize + r)*direction);
              
                setPar(par, HSBColor.ShiftBrighness(tailColor,(r+1)*fade));
            }

            //controller.flurryColor = tailColor;
            //controller.flurryColor2 = tailColor;
           
        }

        private void setPar(int par,Color c)
        {
       
            controller.lights.setRailBoth((reverse)?15-par:par, c);
        }

    }
}
