using System;
using System.Collections.Generic;
using System.Text;

namespace SoundCatcher
{
    class BeatDetect
    {
        private const int WAVEDATASIZE = 256;
        public int suggestedMinHz =10;
        public int suggestedMaxHz = 240;
 

        public bool wereGood = false;
        public int prevConfidence;

        public double[] last_fftArray = new double[WAVEDATASIZE];
        private double[] wavedata = new double[WAVEDATASIZE];
        private double[] wavedata2 = new double[WAVEDATASIZE];

        private const int FRAME_BUFFER_SIZE = 4096;
        double[] frames = new double[FRAME_BUFFER_SIZE];
        public int currentFrame = 0;
        
        public  double beatLength = 20f;
        int triggerNudge =4;// 5;
        public int GoalTriggerBeat = 0;
        public int _TriggerBeat = 0;
 

        public int triggerBeat
        {
            set 
            {
                GoalTriggerBeat = value - triggerNudge;
                if (GoalTriggerBeat >= beatLength) GoalTriggerBeat = 0;
                if (GoalTriggerBeat < 0) GoalTriggerBeat += (int)beatLength;
                tweakTrigger();
                tweakTrigger();
            }
            get
            {
                int tmp = GoalTriggerBeat+ triggerNudge;
                if(tmp > beatLength) tmp-= (int)beatLength;
                return tmp;
            }
        }
        
        private void tweakTrigger()
        {
            if(GoalTriggerBeat>_TriggerBeat)
            {
                if(GoalTriggerBeat - _TriggerBeat > beatLength/2) --_TriggerBeat;
                else ++_TriggerBeat;
            }
            if (GoalTriggerBeat < _TriggerBeat)
            {
                if (_TriggerBeat - GoalTriggerBeat > beatLength / 2) ++_TriggerBeat;
                else --_TriggerBeat;
            }
            if (_TriggerBeat < 0) _TriggerBeat = (int)beatLength;
            if (_TriggerBeat > beatLength) _TriggerBeat = 0;

        }
        
        public bool _isBeat = false;
        public bool _isHalfBeat = false;

        public double Amplitude, MaxAmplitude, MaxMaxAmplitude;
        public double MinAmplitude=5.0f;
        public double Distance = 15;

        public void CalculateAverageAmplitude(ref double[] _wave)
        {
            Amplitude = 0;
            for (int r = 0; r < _wave.Length; ++r)
            {
                Amplitude += Math.Abs(_wave[r]);
            }
            Amplitude = Amplitude / _wave.Length /32768*255;
            if (Amplitude < MaxAmplitude) MaxAmplitude -= .2442;
            if (Amplitude > MaxAmplitude) MaxAmplitude += .41;
            MaxMaxAmplitude -= .1;
            if (MaxAmplitude > MaxMaxAmplitude) MaxMaxAmplitude = MaxAmplitude;
            MinAmplitude = MaxMaxAmplitude - Distance;
            if (MinAmplitude < 40) MinAmplitude = 40;
            if (Distance < 5) MinAmplitude = 0;
        }


        public bool isBeat
        {
            get
            {
                return _isBeat;
            }
        }
        public bool isHalfBeat
        {
            get
            {
                return _isHalfBeat;
            }
        }

     
        public int Step = 1;

        public double BeatCounter = 0.0f;
        
        public BeatDetect()
        {
        }


        public void Reset()
        {
            for (int i = 0; i < FRAME_BUFFER_SIZE; ++i) frames[i] = 0;
        }


        
        public double BeatLength { get { return beatLength; } }


        public float Intensity { 
        get {
            float intensity = (float)frames[currentFrame];
            if (float.IsInfinity(intensity) || float.IsNaN(intensity)) intensity = 0.0f;
            return intensity;
        } }

     
    
        public int  CycleEven=0,CycleOdd=0;
   

        public bool isPeak
        {
            get
            {
                if (Intensity > maxPeak * .9f) return true;
                return false;
            }
        }
        public void DetectBeats(double[] _fftArray)
        {
            doDetectBeats(_fftArray);
        }
        public int doubleTrigger = 0;
        double maxFlux = 0.0;
        public double[] bands = new double[30];
        double maxPeak;
        private void doDetectBeats(double[] _fftArray)
        {
            _isBeat = false;
            _isHalfBeat = false;


            //double max = 0;
            //for (int x = 0; x < _fftArray.Length; x++) //_fftArray.Length
            //{
            //    if (max < _fftArray[x])
            //    {
            //        max = _fftArray[x];
            //    }
            //}

            //************************ CALC FLUX ****************************************

            double flux = 0;
            for (int i = suggestedMinHz; i < suggestedMaxHz; i++)
            {
                double value = (_fftArray[i] - last_fftArray[i]);
                flux += value < 0 ? 0 : value;
            }
           flux /= (suggestedMaxHz - suggestedMinHz);

           if (flux > maxFlux && flux < 25) maxFlux = flux;
           flux = flux / maxFlux * 255;
           if (flux < 26) flux = 0;

           
            _fftArray.CopyTo(last_fftArray, 0);

            //***************************************************************************



            ++currentFrame;
            if (currentFrame >= FRAME_BUFFER_SIZE)
            {
                currentFrame = 0;
            }

            if(double.IsInfinity(flux)) flux=0;
            
            frames[currentFrame] = flux;

            maxPeak *= .999;
            if (flux > maxPeak) maxPeak = flux;
            
            --doubleTrigger;
            if (_TriggerBeat/2 == (int)BeatCounter)
            {
                _isHalfBeat = true;
            }

            if (_TriggerBeat == (int)BeatCounter && doubleTrigger < 0)
            {
                _isHalfBeat = true;
                _isBeat = true;
                doubleTrigger = (int)(beatLength * .7);
                tweakTrigger();
            }

            BeatCounter += 1.0f;
    
            if (BeatCounter >= beatLength)
            {
                if (_TriggerBeat >= (int)beatLength && doubleTrigger < 0) _isBeat = true;
                BeatCounter -= beatLength;
            }
 
            doScan();
            if (_isBeat) if (++Step > 4) Step = 1;
        }

        public double[] slopes = new double[1000];
        private void doScan()
        {
            for (double slope = 3.0f; slope < 79.0f; slope += 0.1f)
            {
                slopes[(int)(slope * 10)] = getFlux(slope, currentFrame);
            }
        }

        public double getFlux(double slope,int frame)
        {
            double flux = 0.0f;
                
            for (int i = 0; i <7; ++i)
            {
                double lookBack = (double)Math.Round(i * slope);
                flux += getFluxValue(frame - Convert.ToInt16(lookBack));
            }
            if ( Double.IsInfinity(flux)) flux = 0;
            return flux;
        }

        public double getFlux2(double slope, int frame)
        {
            double flux = 0.0f;

            for (int i = 0; i < 15; ++i)
            {
                double lookBack = (double)Math.Round(i * slope);
                if (lookBack > FRAME_BUFFER_SIZE) break;
                flux += getFluxValue(frame - Convert.ToInt16(lookBack));
            }
            if (Double.IsInfinity(flux)) flux = 0;
            return flux;
        }

        private double getFluxValue(int index)
        {
            if (index < 0) index += FRAME_BUFFER_SIZE;
            return frames[index];
        }

        public int triggerFound = 0;
        internal void findTrigger()
        {
            if(currentFrame - beatLength < 10) return;
            int beat = 0;
            double maxFlux = 0;
            for (int r = currentFrame - (int)beatLength; r <= currentFrame; ++r)
            {
                double flux = getFlux(beatLength, r);
                if (flux > maxFlux)
                {
                    maxFlux = flux;
                    triggerFound = currentFrame - r - (int)BeatCounter;
                }
            }
            if (triggerFound < 0) triggerFound += (int)beatLength;
            triggerFound = (int)beatLength - triggerFound;
        }


        public bool MusicStopped
        {
            get
            {
                return (MaxAmplitude < MinAmplitude);
            }
        }
    }
}
