using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class HappyColors : SequenceBase
    {
        public override void init()
        {

            controller.doFlurry = false;
            controller.MatchRails = true;
            
            ticksPerCall = 6;
            controller.lights.fade = 0;
            for (int par = 0; par < 4;++par )
            {
                Color c = getColor();
                controller.lights.setRailParLeft(par, c);
                controller.lights.setRailParLeft(par + 1, c);
            }
            for (int par = 0; par < 4; ++par)
            {
                Color c = getColor();
                controller.lights.setRailParRight(par, c);
                controller.lights.setRailParRight(par + 1, c);
            }
            controller.flurry.setRGBLeft(getColor());
            controller.flurry.setRGBRight(getColor());
            
        }

        int pause = 0;
        public override void go()
        {
            if (controller.isBeat && controller.beatDetect.Step == 1)
            {
                controller.flurry.move();
            }   
   
            if (--pause > 0) return;

            int par = random.Next(4)*2;
            Color c = getColor();
            controller.lights.setRailParLeft(par, c);
            controller.lights.setRailParLeft(par+1, c);

            par = random.Next(4) * 2;
            c = getColor();
            controller.lights.setRailParLeft(par, c);
            controller.lights.setRailParLeft(par + 1, c);

            par = random.Next(4) * 2;
            c = getColor();
            controller.lights.setRailParRight(par, c);
            controller.lights.setRailParRight(par + 1, c);

            par = random.Next(4) * 2;
            c = getColor();
            controller.lights.setRailParRight(par, c);
            controller.lights.setRailParRight(par + 1, c);

            controller.flurry.setRGBLeft(getColor());

            controller.flurry.setRGBRight(getColor());
            
            if (controller.isBeat) pause = 3;
 
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
