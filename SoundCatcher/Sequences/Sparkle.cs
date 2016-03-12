using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;

namespace SoundCatcher.Sequences
{
    class Sparkle : SequenceBase
    {
        int fade = 5;
        int steps = 0;
        int lights = 0;
        bool redBlue = true;
        public override void init()
        {

            controller.nextSequenceState = DateTime.Now.AddSeconds(6);
            controller.doFlurry = false;
            
            fade = (random.Next(50) + 15) * -1;
            lights = random.Next(8)+1;
            steps = random.Next(12)+1;
            ticksPerCall = 1;
            controller.lights.fade = 0;
            redBlue = coinFlip();
        }
        int flash = 0;
        Color[] colors = new Color[16];
        int step = 0;
        Color flurry = Color.Black;
        public override void go()
        {
            for (int r = 0; r < 16; ++r) colors[r] = (redBlue)?Color.FromArgb(255,8,0,0):HSBColor.ShiftBrighness(colors[r], fade);
            ++step;
            if (step > steps)
            {
                step = 0;
                for (int i = 0; i < lights; ++i)
                {
                    colors[random.Next(8)] = colors[random.Next(8) + 8] = (redBlue)?Color.FromArgb(255,8,0,255):Color.White; // getColor(colors[_r.Next(8) + 8]);
                }
            }
            if (redBlue)
            {
                for (int r = 0; r < 8; ++r) controller.lights.setRailParLeft(r,colors[r]);
                for (int r = 8; r < 16; ++r) controller.lights.setRailParRight(r - 8, colors[r]);
            }
            else
            {
                for (int r = 0; r < 8; ++r) controller.lights.setRailParLeft(r, HSBColor.ShiftBrighness(colors[r], controller.fadeDepth));
                for (int r = 8; r < 16; ++r) controller.lights.setRailParRight(r - 8, HSBColor.ShiftBrighness(colors[r], controller.fadeDepth));
            }
            controller.flurry.setRGBLeft( (random.Next(steps+1) != 0) ? Color.Black : (redBlue) ? Color.FromArgb(255, 8, 0, 255) : Color.White);
            controller.flurry.setRGBRight( (random.Next(steps + 1) != 0) ? Color.Black : (redBlue) ? Color.FromArgb(255, 8, 0, 255) : Color.White);
            

        }

    }
}
