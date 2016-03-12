using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher.Objects;

namespace SoundCatcher
{
    class FlurryScenes
    {
        Random random = new Random();
         
        List<FlurryPos> positions = new List<FlurryPos>();
        public FlurryWash flurry = null;
        public FlurryScenes()
        {
            //triangle
            Tag("matt", 85, 60, 80 + 30, 60);
            Tag("mark", 85 + 30, 60, 85, 60);
            Tag("drummer", 85 + 42, 60, 85 + 42, 60);
            Tag("beth", 100, 70, 100, 70);
            Tag("DownDown", 50, 110, 50, 110);
            return;
            // top triangle
            Tag("TopStaight", 75, 0, 75 + 30, 0);
            Tag("TopStaight", 75 + 30, 0, 75, 0);
            Tag("TopStaight", 85 + 42, 0, 85 + 42, 0);
            return;
            
          
            return;
            Tag("TopStaight",85,0,85,0);
            Tag("DownRight", 127,63,127,127);
            Tag("DownLeft", 127, 127, 127, 63);
            Tag("DownCenter", 127, 63, 127, 63);
            Tag("Center", 85 + 20, 63, 85 + 20, 63);
            Tag("TopCenter", 85 + 20, 0, 85 + 20, 0);
            Tag("DownDown", 0,127, 0, 127);
         
        }

        int step = 0;

        public void Go(string name)
        {
            step += (random.Next(2) == 0) ? 1 : -1;
            if (step < 0) step = positions.Count - 1;
            if (step >= positions.Count) step = 0;

            FlurryPos pos = positions[step];
            setFlurry(pos);
        }

        public void Straight()
        {
            setFlurry( new FlurryPos("TopStaight", 85, 0, 85, 0));
         
        }

        public void setFlurry(FlurryPos pos)
        {
            int s = random.Next(3);
            
            int pan = pos.pan + (s * 85);
            int rightPan = (pos.rightPan + (s * 85));
       
            if (pan>255) pan -= 170;
            if (rightPan > 255) rightPan -= 170;
            rightPan = 255 - rightPan;
         
            int tilt = pos.tilt;
            int rightTilt = 255 - pos.rightTilt;
             
            if(s%2!=0)
            {
                tilt = 128  + ((tilt-127)*-1);
                rightTilt = (128 + ((rightTilt - 127) * -1));
            }

            flurry.movePosition(flurry.flurryLeft,pan, tilt, 0);
            flurry.movePosition(flurry.flurryLeft2, pan, tilt, 0);
            flurry.movePosition(flurry.flurryRight, rightPan, rightTilt, 0);
            flurry.movePosition(flurry.flurryRight2, rightPan, rightTilt, 0);
        }

        private void Tag(string name,int pan ,int tilt,int rightPan,int rightTilt)
        {
            positions.Add(new FlurryPos(name,pan,tilt,rightPan,rightTilt));
        }
    }
}
