using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher;
using System.Drawing;
using System.Drawing.Imaging;

namespace SoundCatcher.Sequences
{
    class BitMapScan : SequenceBase
    {
        Bitmap bitmap;
        Image gifImage;
        int frameCount;
        FrameDimension dimension = null;
        public override void init()
        {
            controller.doFlurry = false;
            ticksPerCall = 7;
            controller.lights.bFlipSiblingRail = true;

            gifImage = Image.FromFile("C:\\dev\\The Great Wall of Wah Wah.gif");
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            frameCount = gifImage.GetFrameCount(dimension);
        }
        int index = 0;
        public override void go()
        {
            gifImage.SelectActiveFrame(dimension, index);
            Color c = GetPixel(8,31);
            controller.flurry.setRGBLeft(c);
            controller.lights.setRailParRight(0,c);
            
            c = GetPixel(17,38);
            controller.lights.setRailParRight(1,c);

            c = GetPixel(25,46);
            controller.lights.setRailParRight(2,c);

            c = GetPixel(31,53);
            controller.lights.setRailParRight(3,c);

            c = GetPixel(38,59);
            controller.lights.setRailParRight(4,c);

            c = GetPixel(44,64);
            controller.lights.setRailParRight(5,c);

            c = GetPixel(49,70);
            controller.lights.setRailParRight(6,c);

            c = GetPixel(53,74);
            controller.lights.setRailParRight(7,c);

            //--------------------------------------//

            c = GetPixel(57 ,78 );
            controller.lights.setRailParLeft(0, c);

            c = GetPixel(61 , 81);
            controller.lights.setRailParLeft(1, c);

            c = GetPixel(64 , 85);
            controller.lights.setRailParLeft(2, c);

            c = GetPixel(67,88 );
            controller.lights.setRailParLeft(3, c);

            c = GetPixel(70 ,90 );
            controller.lights.setRailParLeft(4, c);

            c = GetPixel(72,93 );
            controller.lights.setRailParLeft(5, c);

            c = GetPixel(75,95 );
            controller.lights.setRailParLeft(6, c);

           
            c = GetPixel(77 ,97 );
            controller.lights.setRailParLeft(7, c);
            controller.flurry.setRGBRight(c);
        

            if(++index >=frameCount) index=0;
        }

        Color GetPixel(int x, int y)
        {
            Color c = ((Bitmap)gifImage).GetPixel(x, y);
            float bright = c.GetBrightness();
            if (bright < .08) return Color.Black;
            //Console.WriteLine(bright);
            return c;
        }
    }
}
