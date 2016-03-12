using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class OverLapping3 : SequenceBase
    {
        Color color1, color2;
        bool bFade = true;
        public override void init()
        {

            controller.doFlurry = false;
            
            ticksPerCall = 1;
            controller.lights.fade = 0;
            controller.flurry.setAllRGB(Color.Black);
        
        }

        int step = -1;  
        bool direction=true;
        Color color;
        int filter = 0;
        public override void go()
        {
            if (controller.isBeat )//&& controller.beatDetect.Step %2==0)
            {
                while (step < 16) render();
                step = -1;
                //controller.flurry.move();
                direction = (random.Next(2)==0);
                Color c = color;
                while(c==color) color = getColor();

                filter = random.Next(2) + 2;
            }
            render();
        }

        void render()
        {
            Color c = color;
            if (++step < 16)
            {
                if (direction)
                {
                    controller.lights.setRailBoth(step, color);
                    if (filter > 1 && (step) % filter == 0) controller.lights.setRailBoth(step, Color.Black);

                }
                else
                {
                    controller.lights.setRailBoth(15 - step, color);
                }
            }

        }
        Color getColor()
        {
            int r = random.Next(6);
            if (r == 1) return Color.Magenta;
            if (r == 2) return Color.Blue;
            if (r == 3) return Color.Cyan;
            if (r == 4) return Color.Lime;
            if (r == 5) return Color.Yellow;
            return Color.Red;

        }


 
    }
}
