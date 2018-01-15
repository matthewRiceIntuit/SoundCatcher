using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SoundCatcher
{
    class FlurryWash
    {
        public int flurryLeft = 1001;
        public int flurryRight = 14;
        public int flurryLeft2 = 27;
        public int flurryRight2 = 40;

        public int drumlite = 108;

        public bool bFlipSiblingFlurry = true;
        public bool bIndependentMiddle = true;
        public Graphics g = null;
        public byte level = 255;

        public Color[] colors = new Color[2];
        public Color[] colorsGoal = new Color[2];
        public int[] diffR = new int[6];
        public int[] diffG = new int[6];
        public int[] diffB = new int[6];
        public Color[] lightCurrent = new Color[4];
        public Color[] lightGoal = new Color[4];
        public int[] lightStepR = new int[4];
        public int[] lightStepG = new int[4];
        public int[] lightStepB = new int[4];
        public int[] lightStep = new int[4];


        public int bStrobe=-1;

        private Random _r = new Random();

        public FlurryWash()
        {
        }
        public void setLevel(int l)
        {
            level = (byte) l;
        }

        public void set_drumlite1(Color color)
        {
            DmxController.setDmxValue(drumlite, color.R);
            DmxController.setDmxValue(drumlite + 1, color.G);
            DmxController.setDmxValue(drumlite + 2, color.B);

            DmxController.setDmxValue(drumlite + 3, color.R);
            DmxController.setDmxValue(drumlite + 4, color.G);
            DmxController.setDmxValue(drumlite + 5, color.B);

            DmxController.setDmxValue(drumlite + 6, color.R);
            DmxController.setDmxValue(drumlite + 7, color.G);
            DmxController.setDmxValue(drumlite + 8, color.B);
        }

        public void set_drumlite2(Color color)
        {

            DmxController.setDmxValue(drumlite + 9, color.R);
            DmxController.setDmxValue(drumlite + 10, color.G);
            DmxController.setDmxValue(drumlite + 11, color.B);

            DmxController.setDmxValue(drumlite + 12, color.R);
            DmxController.setDmxValue(drumlite + 13, color.G);
            DmxController.setDmxValue(drumlite + 14, color.B);
        }

        
        
        public void setRGBLeft(Color color)
        {
            DmxController.setDmxValue(flurryLeft + 6, color.R);
            DmxController.setDmxValue(flurryLeft + 7, color.G);
            DmxController.setDmxValue(flurryLeft + 8, color.B);
            DmxController.setDmxValue(flurryLeft + 5, 255);//(byte)(134 - (level / 2)));

            set_drumlite1(color);


            }

        public void setRGBLeft2(Color color)
        {
            DmxController.setDmxValue(flurryLeft2 + 6, color.R);
            DmxController.setDmxValue(flurryLeft2 + 7, color.G);
            DmxController.setDmxValue(flurryLeft2 + 8, color.B);
            DmxController.setDmxValue(flurryLeft2 + 5, 255);//(byte)(134 - (level / 2)));

            set_drumlite2(color);
        }

        public void setRGBRight(Color color)
        {
            DmxController.setDmxValue(flurryRight + 6, color.R);
            DmxController.setDmxValue(flurryRight + 7, color.G);
            DmxController.setDmxValue(flurryRight + 8, color.B);
            DmxController.setDmxValue(flurryRight + 5, 255);//(byte)(134 - (level / 2)));

            set_drumlite1(color);
        }

        public void setRGBRight2(Color color)
        {
            DmxController.setDmxValue(flurryRight2 + 6, color.R);
            DmxController.setDmxValue(flurryRight2 + 7, color.G);
            DmxController.setDmxValue(flurryRight2 + 8, color.B);
            DmxController.setDmxValue(flurryRight2 + 5, 255);//(byte)(134 - (level / 2)));
            set_drumlite2(color);
        }

        public void setAllRGB(Color color)
        {
            setLight2(0,color);
            setLight2(1,color);
            setLight2(2, color);
            setLight2(3, color);
            set_drumlite1(color);
            set_drumlite2(color);

        }

  
        public void setEvenRGB(Color color)
        {

            setLight2(0, color);
            setLight2(2,color);
            set_drumlite1(color);
           
        }

        public void setOddRGB(Color color)
        {
            setLight2(1, color);
            setLight2(3, color);
            set_drumlite2(color);

        }

        public void setLight(int par, Color color)
        {
            if (par == 0) setRGBLeft(color);
            if (par == 1) setRGBLeft2(color);
            if (par == 2) setRGBRight2(color);
            if (par == 3) setRGBRight(color);
        }
    

        public void movePosition(int flurry,int pan,int tilt,int speed)
        {

            tilt = adjustTilt(tilt,pan);
            if (bDelayMoves)
            {
                if (flurry == flurryLeft)
                {
                    currentTick = 0;
                    currentPan = pan;
                    currentTilt = tilt;
                }
                else
                {
                    return;
                }
            }
         
            _movePosition(flurry,pan,tilt, speed);


        }

        public void _movePosition(int flurry,int pan,int tilt,int speed)
        {
            DmxController.setDmxValue(flurry, (byte)pan);
            DmxController.setDmxValue(flurry + 2, (byte)tilt);
            DmxController.setDmxValue(flurry + 4, (byte)speed);
        }



        public int adjustTilt(int tilt,int pan)
        {
            if (tilt > 100) tilt = 100;
            else if (tilt < 10) tilt = 0;
            else if (tilt < 60) tilt = 60;
            if (pan < 42 || (pan > 124 && pan < 210)) tilt = 255 - tilt;
            return tilt;
        }


        public void strobe()
        {

            if (bStrobe == 0)
            {

                //DmxController.dark();
                // value between 225 and 239
                DmxController.setDmxValue(flurryLeft + 5, (byte)230);
                DmxController.setDmxValue(flurryRight + 5, (byte)230);
                DmxController.setDmxValue(flurryLeft2 + 5, (byte)230);
                DmxController.setDmxValue(flurryRight2 + 5, (byte)230);
            }
            if (bStrobe == 1)
            {
                //DmxController.dark();
                DmxController.setDmxValue(flurryLeft + 9, (byte)240);
                DmxController.setDmxValue(flurryRight + 9, (byte)240);
                DmxController.setDmxValue(flurryLeft + 10, (byte)15);
                DmxController.setDmxValue(flurryRight + 10, (byte)15);
                DmxController.setDmxValue(flurryLeft2 + 9, (byte)240);
                DmxController.setDmxValue(flurryRight2 + 9, (byte)240);
                DmxController.setDmxValue(flurryLeft2 + 10, (byte)15);
                DmxController.setDmxValue(flurryRight2 + 10, (byte)15);
            }
            else
            {
                DmxController.setDmxValue(flurryLeft + 9, (byte)0);
                DmxController.setDmxValue(flurryRight + 9, (byte)0);
                DmxController.setDmxValue(flurryLeft + 10, (byte)0);
                DmxController.setDmxValue(flurryRight + 10, (byte)0);
                DmxController.setDmxValue(flurryLeft2 + 9, (byte)0);
                DmxController.setDmxValue(flurryRight2 + 9, (byte)0);
                DmxController.setDmxValue(flurryLeft2 + 10, (byte)0);
                DmxController.setDmxValue(flurryRight2 + 10, (byte)0);
            }
 
        }


        bool odd = false;
        public void move()
        {
            //if( _r.Next(4)==0) return;

            doMove(_r.Next(3) == 0 ? 0 : 255);
        }
        public void doMove(int speed)
        {
            
            int pan = _r.Next(255);
            int tilt = _r.Next(120);

            if (bIndependentMiddle)
            {
                odd = !odd;
                if (odd)
                {
                    movePosition(flurryLeft, pan, tilt, speed);
                    pan = 255 - pan;
                    tilt = 255 - tilt;
                    movePosition(flurryRight, pan, tilt, speed);
                }
                else
                {
                    movePosition(flurryLeft2, pan, tilt, speed);
                    pan = 255 - pan;
                    tilt = 255 - tilt;
                    movePosition(flurryRight2, pan, tilt, speed);
                }

            }
            else
            {

                movePosition(flurryLeft, pan, tilt, speed);
                movePosition(flurryRight2, pan, tilt, speed);

                if (bFlipSiblingFlurry)
                {
                    pan = 255 - pan;
                    tilt = 255 - tilt;
                }
                movePosition(flurryRight, pan, tilt, speed);
                movePosition(flurryLeft2, pan, tilt, speed);
            }
        }

        public void moveSlow()
        {
            doMove(255);
        }
 
        public void moveFast()
        {
            doMove(0);
        }
 
        public void write()
        {
            DmxController.write();
        }

        public bool bDelayMoves = false;
        public int delayTicks = 6;
        int currentPan;
        int currentTilt;
        int currentSpeed =0;
        int currentTick;



        public bool bSequence = false;
        public int sequenceLength = 2;
        public int sequenceStep = 0;
        public int sequenceDelay = 2;
        public int sequenceTicks = 0;
        public int sequenceSpeed = 10;


        public Color sequenceColor = Color.Black;
        public Color sequenceColor2 = Color.Black;

        public void doSequence()
        {
            bSequence = true;
            sequenceDelay =  _r.Next(4) + 2;
            sequenceLength = _r.Next(4) + 1;
     
        }
        public void tick()
        {
            if (bDelayMoves)
            {


                ++currentTick;

                if (currentTick == delayTicks) _movePosition(flurryLeft2, currentPan, currentTilt, currentSpeed);
                if (currentTick == delayTicks * 2) _movePosition(flurryRight2, currentPan, currentTilt, currentSpeed);
                if (currentTick == delayTicks * 3) _movePosition(flurryRight, currentPan, currentTilt, currentSpeed);
            }

            if (bSequence)
            {
                if (++sequenceTicks > sequenceSpeed)
                {
                    sequenceTicks = 0;

                    if (++sequenceStep > 4 + sequenceLength) sequenceStep = 0;


                }
                for (int r = 0; r < 4; ++r)
                {
                    if (r <= sequenceStep && r > sequenceStep - sequenceLength)
                        setLight2(r, sequenceColor2);
                    else
                        setLight2(r, sequenceColor);
                }

            }
        }

        void set_drumelite(int r, Color color)
        {
            if (r == 0)
            {
                DmxController.setDmxValue(drumlite + 0, color.R);
                DmxController.setDmxValue(drumlite + 1, color.G);
                DmxController.setDmxValue(drumlite + 2, color.B);
            }
            if (r == 1)
            {
                DmxController.setDmxValue(drumlite + 3, color.R);
                DmxController.setDmxValue(drumlite + 4, color.G);
                DmxController.setDmxValue(drumlite + 5, color.B);
            }
            if (r==2)
            {
                DmxController.setDmxValue(drumlite + 6, color.R);
                DmxController.setDmxValue(drumlite + 7, color.G);
                DmxController.setDmxValue(drumlite + 8, color.B);
            }
            else
            {
                DmxController.setDmxValue(drumlite + 9, color.R);
                DmxController.setDmxValue(drumlite + 10, color.G);
                DmxController.setDmxValue(drumlite +11, color.B);

                DmxController.setDmxValue(drumlite + 12, color.R);
                DmxController.setDmxValue(drumlite + 13, color.G);
                DmxController.setDmxValue(drumlite + 14, color.B);
            }
        }


        void setLight2(int r, Color c)
        {

            if (lightCurrent[r] == c )
            {
                setLight(r, c);
                return;
            }

            if (lightGoal[r] != c)
            {
                lightGoal[r] = c;
                lightStepR[r] = (lightCurrent[r].R - c.R) / 5;
                lightStepG[r] = (lightCurrent[r].G - c.G) / 5;
                lightStepB[r] = (lightCurrent[r].B - c.B) / 5;
                lightStep[r] = 0;
            }

            if (++lightStep[r] > 5)
            {
                lightCurrent[r] = c;
                setLight(r, c);
                return;
            }
          
            Color temp = Color.FromArgb(lightCurrent[r].R - (lightStepR[r]*lightStep[r]),
                                         lightCurrent[r].G - (lightStepG[r] * lightStep[r]),
                                         lightCurrent[r].B - (lightStepB[r] * lightStep[r]));
            setLight(r,temp);
        }
    }
}
