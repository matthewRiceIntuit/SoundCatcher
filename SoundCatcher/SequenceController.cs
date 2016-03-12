using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SoundCatcher.Sequences;
using System.Reflection;
using System.Windows.Forms;



namespace SoundCatcher
{
    class SequenceController
    {
        public  int step = 0;
        public Chauvet lights = new Chauvet();
        public FlurryWash flurry = new FlurryWash();
        public float[] spectrum = null;
       
        private int tickCount=0;

        public Graphics graphics{set{ lights.g = value;}}

      
        public ColorDuo colors;
        public BeatDetect beatDetect;
        public float Intensity = 0;
        public bool isBeat;
        public bool isHalfBeat;
        public bool isPeak;
        public List<SequenceBase> SequenceList;
        public List<SequenceBase> SequenceListSlow;

        public SequenceBase Sequence;
        public List<ColorDuo> ColorDuoList;

        public Flurry2 flurrySequence = new Flurry2();
        public BlackOut blackout = new BlackOut();
        public White white = new White();
        public Color flurryColor,flurryColor2 = Color.Black;
        public bool doFlurry = true;
        public FlurryScenes flurryScenes = new FlurryScenes();
        public bool MatchRails = false;
        public SequenceController()
        {
            Console.WriteLine("init");
            DmxController.openDMX(90);
            Console.WriteLine("init1");
            flurry.level = 250;
            flurrySequence.Controller = this;
            blackout.Controller = this;
            white.Controller = this;
            flurryScenes.flurry = flurry;

            //SequenceList.Add(new Tiers());
            //SequenceList.Add(new Tiers2());
     
            SequenceList = new List<SequenceBase>();
            SequenceListSlow = new List<SequenceBase>();
    
            colors = new ColorDuo(Color.Red, Color.Lime);
      
            SequenceList.Add(new MovingHeadOnly());
            SequenceList.Add(new MovingHeadOnly());
            
            SequenceList.Add(new ColorWaves2());
            SequenceList.Add(new ShootingStar());
            SequenceList.Add(new EqDrops());
            SequenceList.Add(new LevelTrail());
            SequenceList.Add(new LevelTrail2());
            SequenceList.Add(new CellAtomota());
           
            SequenceList.Add(new Skitter());
            SequenceList.Add(new Sparkle());
            SequenceList.Add(new LevelMeterColor());

            Console.WriteLine("init4");
            //SequenceList.Add(new Slugs());
            SequenceList.Add(new HappyColors());
            SequenceList.Add(new SinCos());
            //SequenceList.Add(new Waves());
            //SequenceList.Add(new BitMapScan());
            SequenceList.Add(new Blobs());
            SequenceList.Add(new FlyingBars());
           // SequenceList.Add(new CenterPulse());
            SequenceList.Add(new Flash1());
            SequenceList.Add(new RainboxFlash());
            SequenceList.Add(new OverLapping());
            SequenceList.Add(new OverLapping2());
            SequenceList.Add(new OverLapping3());
            SequenceList.Add(new OddEven());
            SequenceList.Add(new SixSpot4());
            SequenceList.Add(new SixSpot5());
        
            //****************************************************

            SequenceListSlow.Add(new SinCos());
            SequenceListSlow.Add(new Waves());
            SequenceListSlow.Add(new OddEven());
            SequenceListSlow.Add(new SixSpot4());
           
            
            Console.WriteLine("init5");
       

            foreach (SequenceBase seq in SequenceList)
            {
                seq.Controller = this;
            }
            foreach (SequenceBase seq in SequenceListSlow)
            {
                seq.Controller = this;
            }
         
            ColorDuoList = new List<ColorDuo>();
            ColorDuoList.Add(new ColorDuo(Color.Red, Color.Blue));
            ColorDuoList.Add(new ColorDuo(Color.Blue,Color.Red));
            ColorDuoList.Add(new ColorDuo(Color.Red, Color.Yellow));
            ColorDuoList.Add(new ColorDuo(Color.Magenta, Color.Lime));
            ColorDuoList.Add(new ColorDuo( Color.Blue,Color.Magenta));
            ColorDuoList.Add(new ColorDuo(Color.Blue, Color.Lime));
            //ColorDuoList.Add(new ColorDuo(Color.Lime, Color.Yellow));
            ColorDuoList.Add(new ColorDuo(Color.Aqua, Color.Yellow));
            ColorDuoList.Add(new ColorDuo(Color.DarkOrange, Color.RoyalBlue));
            ColorDuoList.Add(new ColorDuo(Color.Blue,Color.Magenta));
            ColorDuoList.Add(new ColorDuo(Color.DarkMagenta, Color.Aqua));


            Console.WriteLine("init6");




            try
            {

                //UpdateSequence();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Console.WriteLine("init7");
            dark();
            Console.WriteLine("init8");
  
        }

        public void dark()
        {
            DmxController.dark();
        }

        int sequenceStep = 5;
        public void UpdateSequence()
        {
            Random _r = new Random();
            flurry.bStrobe = -1;
            lights.fade = 0f;
            doFlurry = true;
            MatchRails = false;
            flurry.bSequence = false;

            nextSequenceState = DateTime.Now.AddSeconds(40);
            try
            {
                if (beatDetect.beatLength < 31)
                {
                    Sequence = SequenceList[_r.Next(SequenceList.Count)];
                }
                else
                {
                    Sequence = SequenceListSlow[_r.Next(SequenceListSlow.Count)];
                }
            }catch{}

            if (sequnceOverride == 1)
            {
                Sequence = white;
                flurry.setAllRGB(Color.Black);
            }
            if (sequnceOverride > 1)
            {
                Sequence = SequenceList[sequnceOverride-2];
                flurry.setAllRGB(Color.Black);
            }

            lights.bFlipSiblingRail = false;
            lights.setRailAll(Color.Black);
            

            Console.WriteLine(Sequence.ToString());

            colors = ColorDuoList[_r.Next(ColorDuoList.Count)];

            MatchRails = false;
            flurrySequence.init();
            Sequence.init();
            if(doFlurry) flurrySequence.init();
          
        
            flurryColor = Color.White;
            flurryColor2 = Color.White;

            
        }



        public DateTime nextSequenceState;
        public void  DoSequence()
        {
            if (beatDetect.MusicStopped &&  sequnceOverride==0)
            {
                nextSequenceState = DateTime.Now.AddSeconds(3);
                Sequence = blackout;
                blackout.go();
                lights.write(); 
                return;
            }

            ++tickCount;
            if (beatDetect.isBeat) isBeat = true;
            if (beatDetect.isHalfBeat) isHalfBeat = true;
            if (beatDetect.isPeak) isPeak = true;
            Intensity = Math.Max( beatDetect.Intensity,Intensity);
            calcBeatFade();

            if (tickCount >= Sequence.ticksPerCall)
            {
                //if (isBeat && beatDetect.Step%2==0) Intensity = 255f;
                tickCount = 0;
                ++step;
                if (step > 8) step = 1;


             
                Sequence.go();
                if(doFlurry) flurrySequence.go();
             
                Intensity = 0f;
                isBeat = false;
                isPeak = false;
                isHalfBeat = false;
            }
            
            flurry.tick();
            
            if (MatchRails)
            {
                flurry.setRGBLeft(lights.getPAR(6));
                flurry.setRGBLeft2(lights.getPAR(1));
                flurry.setRGBRight2(lights.getPAR(9));
                flurry.setRGBRight(lights.getPAR(14));
            }

            lights.write();
  
            if (DateTime.Now >= nextSequenceState && Sequence.Done && beatDetect.isBeat)
            {
                UpdateSequence();
            }
        }

       public double beatFadeVal = 0;
       public double beatFade = 0;
       public int fadeDepth;
       private void calcBeatFade()
       {
            beatFadeVal = beatDetect.BeatCounter - beatDetect.triggerBeat;
            if (beatFadeVal < 0) beatFadeVal += beatDetect.beatLength;
            beatFade = beatFadeVal;
            if (beatFade >beatDetect.beatLength / 2) beatFade =beatDetect.beatLength - beatFade;
            fadeDepth = (int)(1 /beatDetect.beatLength * -500 *beatFade);
           
       }

        public ColorDuo GetNextColor()
        {
            Random _r = new Random();
            colors = ColorDuoList[_r.Next(ColorDuoList.Count)];
            return colors;
        }

        int sequnceOverride = 0;
        internal void setSequenceOVerride(int p)
        {
            sequnceOverride = p;
            nextSequenceState = DateTime.Now;
        }

    }

    class ColorDuo
    {
        public Color odd;
        public Color even;
        public ColorDuo(Color even, Color odd)
        {
            this.odd = odd;
            this.even = even;
        }
    }
 
}
