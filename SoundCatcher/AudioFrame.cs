/* Copyright (C) 2008 Jeff Morton (jeffrey.raymond.morton@gmail.com)

   This program is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA */

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SoundCatcher
{
    class AudioFrame
    {
        public BeatDetect beatDetect = new BeatDetect();

        public SequenceController sequence = new SequenceController();
        public NotifyIcon notifyIcon = null;
        private double[] _wave;
        private double[] _fftArray;
        private ArrayList _fftSpect = new ArrayList();
        private int _maxHeightSpect = 0;


        public AudioFrame()
        {
            sequence.beatDetect = beatDetect;
            notifyIcon = new NotifyIcon();
            notifyIcon.Visible = true;
            for(int r=0;r<beats.Length;++r) beats[r]=0;
        }

        /// <summary>
        /// Process 16 bit sample
        /// </summary>
        /// <param name="wave"></param>
        
        
        public void Process(ref byte[] wave)
        {
            _wave = new double[wave.Length / 4];
 
            // Split out channels from sample
            int h = 0;
            for (int i = 0; i < wave.Length; i += 4)
            {
                _wave[h] = (double)BitConverter.ToInt16(wave, i);
                h++;
            }

            // Generate frequency domain data in decibels
            _fftArray = FourierTransform.FFT(ref _wave);
            _fftSpect.Add(_fftArray);
            if (_fftSpect.Count > _maxHeightSpect)
                _fftSpect.RemoveAt(0);

            beatDetect.CalculateAverageAmplitude(ref _wave);
        }

        internal void DetectBeats()
        {
           beatDetect.DetectBeats(_fftArray);
        }

        public void RenderFrequencyDomain(ref PictureBox pictureBox, int samples)
        {
            // Set up for drawing
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);
            SolidBrush brush = new System.Drawing.SolidBrush(Color.FromArgb(128, 255, 255, 255));
            Pen pen = new System.Drawing.Pen(Color.WhiteSmoke);
            Font font = new Font("Arial", 10);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;

            double min = double.MaxValue;
            double minHz = 0;
            double max = double.MinValue;
            double maxHz = 0;
            double range = 0;
            double scale = 0;
            double scaleHz = (double)(samples / 2) / (double)_fftArray.Length;

            // get  min/max
            for (int x = 0; x < _fftArray.Length; x++)
            {
                double amplitude = _fftArray[x];
                if (min > amplitude)
                {
                    min = amplitude;
                    minHz = (double)x * scaleHz;
                }
                if (max < amplitude)
                {
                    max = amplitude;
                    maxHz = (double)x * scaleHz;
                }
            }

            // get  range
            if (min < 0 || max < 0)
                if (min < 0 && max < 0)
                    range = max - min;
                else
                    range = Math.Abs(min) + max;
            else
                range = max - min;
            scale = range / height;

            // draw  channel
            for (int xAxis = 0; xAxis < width; xAxis++)
            {
                double amplitude = (double)_fftArray[(int)(((double)(_fftArray.Length) / (double)(width)) * xAxis)];
                if (amplitude == double.NegativeInfinity || amplitude == double.PositiveInfinity || amplitude == double.MinValue || amplitude == double.MaxValue)
                    amplitude = 0;
                int yAxis;
                if (amplitude < 0)
                    yAxis = (int)(height - ((amplitude - min) / scale));
                else
                    yAxis = (int)(0 + ((max - amplitude) / scale));
                if (yAxis < 0)
                    yAxis = 0;
                if (yAxis > height)
                    yAxis = height;
                pen.Color = pen.Color = Color.FromArgb(0, GetColor(min, max, range, amplitude), 0);
                offScreenDC.DrawLine(pen, xAxis, height, xAxis, yAxis);
            }
            //offScreenDC.DrawString("Max: " + maxHz.ToString(".#") + " Hz (±" + scaleHz.ToString(".#") + ") = " + max.ToString(".###") + " dB", font, brush, 0 + 1, 0 + 18);

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
        }

        public void RenderSpectrogram(ref PictureBox pictureBox)
        {
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;

            double min = double.MaxValue;
            double max = double.MinValue;
            double range = 0;

            if (height > _maxHeightSpect)
                _maxHeightSpect = height;

            // get min/max
            for (int w = 0; w < _fftSpect.Count; w++)
                for (int x = 0; x < ((double[])_fftSpect[w]).Length; x++)
                {
                    double amplitude = ((double[])_fftSpect[w])[x];
                    if (min > amplitude)
                    {
                        min = amplitude;
                    }
                    if (max < amplitude)
                    {
                        max = amplitude;
                    }
                }

            // get range
            if (min < 0 || max < 0)
                if (min < 0 && max < 0)
                    range = max - min;
                else
                    range = Math.Abs(min) + max;
            else
                range = max - min;

            // lock image
            PixelFormat format = canvas.PixelFormat;
            BitmapData data = canvas.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, format);
            int stride = data.Stride;
            int offset = stride - width * 4;

            try
            {
                unsafe
                {
                    byte* pixel = (byte*)data.Scan0.ToPointer();

                    // for each cloumn
                    for (int y = 0; y <= height; y++)
                    {
                        if (y < _fftSpect.Count)
                        {
                            // for each row
                            for (int x = 0; x < width; x++, pixel += 4)
                            {
                                double amplitude = ((double[])_fftSpect[_fftSpect.Count - y - 1])[(int)(((double)(_fftArray.Length) / (double)(width)) * x)];
                                double color = GetColor(min, max, range, amplitude);
                                //double color = GetColor(0.0, 40.0, 100.0, amplitude);
                                if (x % (width / 8) == 0) color = 255;
                                pixel[0] = (byte)0;
                                pixel[1] = (byte)color;
                                pixel[2] = (byte)0;
                                pixel[3] = (byte)255;
                            }
                            pixel += offset;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // unlock image
            canvas.UnlockBits(data);

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
        }
 

        /// <summary>
        /// Get color in the range of 0-255 for amplitude sample
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="range"></param>
        /// <param name="amplitude"></param>
        /// <returns></returns>
        private static int GetColor(double min, double max, double range, double amplitude)
        {
            double color;
            if (min != double.NegativeInfinity && min != double.MaxValue & max != double.PositiveInfinity && max != double.MinValue && range != 0)
            {
                if (min < 0 || max < 0)
                    if (min < 0 && max < 0)
                        color = (255 / range) * (Math.Abs(min) - Math.Abs(amplitude));
                    else
                        if (amplitude < 0)
                            color = (255 / range) * (Math.Abs(min) - Math.Abs(amplitude));
                        else
                            color = (255 / range) * (amplitude + Math.Abs(min));
                else
                    color = (255 / range) * (amplitude - min);
            }
            else
                color = 0;
            return (int)color;
        }

        internal void RenderSpectrum(ref PictureBox pictureBox)
        {
            // Set up for drawing
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);
            Pen pen = new System.Drawing.Pen(Color.Green);
 

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;
            
            //g.FillRectangle(mBrushBlack, 0, 0, width, height);
            double max = 0;
            int maxHz=0;
            int Hz = 0;
            double amplitude;
            for (int x = 20; x < _fftArray.Length; x++) //_fftArray.Length
            {
                amplitude = _fftArray[x];
                if (max < amplitude)
                {
                    max = amplitude;
                    maxHz = x;
                }
            }

            /*
                The upper band of frequencies at 44khz is pretty boring (ie 11-22khz), so we are only
                going to display the first 256 frequencies, or (0-11khz) 
            */
            for (Hz = 0; Hz < _fftArray.Length; Hz++)
            {
                amplitude = _fftArray[Hz] / max * height;

                if (amplitude >= height)
                {
                    amplitude = height - 1;
                }

                if (amplitude < 0)
                {
                    amplitude = 0;
                }
                pen.Color = pen.Color = Color.FromArgb(0, 255 * Convert.ToInt16(amplitude) / height/2, 0);
                if (Hz % 20 == 0) pen.Color = Color.Red;
                offScreenDC.DrawLine(pen, Hz, height - (float)amplitude, Hz, (float)amplitude);
            }

            SolidBrush brush = new System.Drawing.SolidBrush(Color.FromArgb(128, 255, 255, 255));
            Font font = new Font("Arial", 10);

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
        }


        int frameX = 0;
        Pen beatRed = new Pen(Color.FromArgb(128, 255, 0, 0));
        Pen beatYellow = new Pen(Color.Yellow);
        Brush black = new SolidBrush(Color.Black);



   
        float beatX = 0, beatY = 0;
        public void RenderPeakChart(ref PictureBox pictureBox)
        {
            System.Drawing.Graphics g = pictureBox.CreateGraphics();

            // Determine channnel boundries
            int width = pictureBox.Width;
            int height = pictureBox.Height;

            
            Color heat = Color.FromArgb(Math.Min((int)beatDetect.Intensity,255) , 0, 0);
            g.FillRectangle(new SolidBrush(heat), beatX * 3, (beatY * 2) , 3, 2);
   
            ++beatY;
            if (beatY > beatDetect.BeatLength)
            {
                g.FillRectangle(new SolidBrush(Color.Black), beatX * 3, beatY * 2, 3, height);
                g.FillRectangle(new SolidBrush(Color.Yellow), beatX * 3, ((beatY) * 2), 3, 2);
                beatY -= (float)beatDetect.BeatLength;
                if (++beatX > width/3) beatX = 0;
            }
        }

        int waveX = 0;
        public void RenderLevelMeter(ref PictureBox pictureBox)
        {

            System.Drawing.Graphics g = pictureBox.CreateGraphics();

            // Determine channnel boundries
            int width = pictureBox.Width;
            int height = pictureBox.Height;



            double amp = beatDetect.Amplitude / 255 * height;
            double max = beatDetect.MaxAmplitude / 255 * height;
            double min = beatDetect.MinAmplitude / 255 * height;
            double maxmax = beatDetect.MaxMaxAmplitude / 255 * height;
           
            g.FillRectangle(black, 0, 0, width, height - (int)amp);
            g.FillRectangle(new SolidBrush(Color.Green), 0, height - (int)amp, width, height);
            g.FillRectangle(new SolidBrush(Color.Red), 0, height - (int)max, width, 4);
            g.FillRectangle(new SolidBrush(Color.Lime), 0, height - (int)maxmax, width, 4);
            g.FillRectangle(new SolidBrush(Color.White), 0, height - (int)min, width, 2);



        }


        internal void RenderIntensityPeaks(ref PictureBox pictureBox)
        {
            System.Drawing.Graphics g = pictureBox.CreateGraphics();
            Pen pen = new System.Drawing.Pen(Color.Green);

            // Determine channnel boundries
            int width = pictureBox.Width;
            int height = pictureBox.Height;

            ++frameX;
            if (frameX > width)
            {
                frameX = 0;

                g.FillRectangle(new SolidBrush(Color.Black), 0, 0, width, height);
            }


            int intensity = (int)(beatDetect.Intensity / 255 * height / 3);
            if (intensity < 0) intensity = 0;
            int mid = height / 2;

            if (beatDetect.isBeat)
            {

                g.DrawLine(beatRed, frameX, 0, frameX, height);
                g.DrawLine(beatRed, frameX + 1, 0, frameX + 1, height);

            }
            g.DrawLine(pen, frameX, mid - intensity, frameX, mid + intensity);
        }

        public void RenderWaveForm(ref PictureBox pictureBox)
        {
            
            System.Drawing.Graphics g = pictureBox.CreateGraphics();

            // Determine channnel boundries
            int width = pictureBox.Width;
            int height = pictureBox.Height;
            int mid = height / 2;


            
            int d = (int)(beatDetect.Amplitude /255*mid);

            if (beatDetect.isBeat)
            {
                g.DrawLine(beatRed, waveX, 0, waveX, height);
            }
            if (trigger)
            {
                g.DrawLine(beatYellow, waveX, 0, waveX, height/2);
            }
            if ((int)beatDetect.BeatCounter == maxTrigger)
            {
                g.DrawLine(new System.Drawing.Pen(Color.Aqua), waveX, 0, waveX, height / 8);
            }
            if (maxTrigger == (int)beatDetect.BeatCounter && !beatDetect.isBeat)
            {
                g.DrawLine(new System.Drawing.Pen(Color.Magenta), waveX, 0, waveX, height / 2);
            }

            g.DrawLine(new Pen(Color.Green), waveX, mid - (int)d, waveX, mid + (int)d);
            
            if (++waveX > width)
            {
                g.FillRectangle(black,0,0,width,height);
                waveX = 0;
            }

        }

        public double[] beats = new double[9000];
        public double[] finalBeats = new double[5000];
        double median;
        double maxDope = 1;
        double maxTriggerDope = 1;
        int maxDopeR;
    
        double[] triggers = new double[100];

        public void RenderBeats(ref PictureBox pictureBox)
        {

            // Set up for drawing
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);
            Pen pen = new System.Drawing.Pen(Color.FromArgb(100,255,0,0));
            Pen pen1 = new System.Drawing.Pen(Color.FromArgb(200, 255, 0, 0));

            Pen yellow = new System.Drawing.Pen(Color.Yellow);
            Pen green = new System.Drawing.Pen(Color.Green);
            Pen lime = new System.Drawing.Pen(Color.Lime);

            Pen blue = new System.Drawing.Pen(Color.Blue);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;

            for (int r = 10; r < 800; ++r) beats[r] *= .9999;
            maxDope *= .9;
            maxTriggerDope *= .999;

            double maxmax = 1;
            for (int r = 100; r <500; ++r)
            {
                if (beats[r] > maxmax) maxmax = beats[r];
            }
            for (int r = 10; r < 800; ++r)
            {
                offScreenDC.DrawLine(yellow, r, height - (int)(beats[r] / maxmax * height), r, height);
            }
            for (int r = 0; r<800;++r)
            {
                //if (r % (int)(beat/2) == 0) offScreenDC.DrawLine(blue, r, 0, r, height);
                if (r % (int)(beat) == 0) offScreenDC.DrawLine(green, r, 0, r, height);
                if (r % (int)(beat2) == 0) offScreenDC.DrawLine(blue, r, 0, r, 10);
                if (r == (int)beat || r == (int)(beat * 2) || r == (int)(beat * 4) || r == (int)(beat * 8) ) offScreenDC.DrawLine(lime, r, 0, r, height);
            
                double amp = beatDetect.slopes[r] / 10;

                //for (int i = 0; i < 100; ++i) triggers[i] *= .999;
                if (amp < maxDope) continue;
                
                beats[r] += 20;
                if (beats[r] >= maxDope)
                {
                    
                    maxDopeR = r;
                    maxDope = beats[r];
                    if (Math.Abs(r - (int)(beatDetect.beatLength * 10)) < 3)
                    {
                        doTrigger(7);
                    }
           
                }
               
            }

           
            

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();

        }


        public void RenderFinalBeats(ref PictureBox pictureBox)
        {

            // Set up for drawing
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);
            Pen pen2 = new System.Drawing.Pen(Color.Yellow);
            Pen pen3 = new System.Drawing.Pen(Color.Green);
            Pen pen4 = new System.Drawing.Pen(Color.Lime);


            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;

            double maxmax=1.0;
            for (int r = 0; r < width; ++r)
            {
                if (finalBeats[r] > maxmax) maxmax = finalBeats[r];
            }
    
            for (int r = 0; r < 600; ++r)
            {
                offScreenDC.DrawLine(pen2, r, height - (int)(finalBeats[r] / maxmax * height), r, height);
            }
            int r2= (int)(beatDetect.beatLength*10);
            offScreenDC.DrawLine(pen4, r2, height - (int)(finalBeats[r2] / maxmax * height), r2, height);


            SolidBrush brush = new System.Drawing.SolidBrush(Color.FromArgb(255, 255, 255, 255));
            Font font = new Font("Arial", 10);

            offScreenDC.DrawString("Length: " + beatDetect.BeatLength.ToString("0.00") + "    flux:"+finalBeats[(int)(beatDetect.BeatLength*10)].ToString("0.0"), font, brush, 10, 5);
            offScreenDC.DrawString("beat: " + (beat / 10).ToString("0.00"), font, brush, 10, 20);
            offScreenDC.DrawString("trigger: " + beatDetect.triggerBeat, font, brush, 10, 35);
            offScreenDC.DrawString("BPM: " + (1/beatDetect.BeatLength * 3700).ToString("0.00"), font, brush, 10, 50);


            for (int r = 0; r < 100; ++r)
            {
                offScreenDC.DrawLine(pen2, width - 100 + r, height-(int)(triggers[r] / maxTriggerVal * height), width - 100 + r, height);
                offScreenDC.DrawLine(pen4, width - 100 + r, height - (int)(triggers[r]), width - 100 + r, height);
            }
            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();

        }

        public double musicStopped() 
        {
            for (int r = 10; r < 1000; ++r) beats[r] *= .99;
            for (int r = 0; r < 500; ++r) finalBeats[r] *= .9;
           for (int i = 0; i < 100; ++i) triggers[i] *=.99;

            
            return 0.0;
        }
        int maxTrigger = 0;
        int maxTriggerVal = 1;
        public void doTrigger(int inc)
        {
            for (int i = 0; i < 99; ++i)  triggers[i] *= .999;
    
            trigger = true;
            if (fuzzy(beatDetect.BeatCounter, beatDetect.triggerBeat,3))
            {
                for (int i = 0; i < 99; ++i) triggers[i] = 0;
                triggers[(int)beatDetect.BeatCounter] = 5;
                //beatDetect.triggerBeat = (int)beatDetect.BeatCounter;
            }
    
            triggers[(int)beatDetect.BeatCounter] += inc;
    
                 
            int maxTrigger2 = 0;
            maxTriggerVal = 1;
            maxTrigger = -1;
            for (int i = 0; i < 99; ++i)
            {
                if (triggers[i] > 0) triggers[i]*=.95;

                if (triggers[i]  > maxTriggerVal)
                {
                    maxTrigger2 = maxTrigger;
                    maxTriggerVal = (int)triggers[i];
                    maxTrigger = i;
                }
            }
            //if (triggers[maxTrigger] - triggers[maxTrigger2] < 3) return;
    
            if(maxTrigger == (int)beatDetect.BeatCounter)
                beatDetect.triggerBeat = (int)beatDetect.BeatCounter;

            return;
            beatDetect.findTrigger();
            if (fuzzy(beatDetect.triggerFound, maxTrigger, 3))
            {
                //beatDetect.triggerBeat = beatDetect.triggerFound;
                Console.WriteLine("Trigger: " + beatDetect.triggerFound );
            }
            
        }


        public bool fuzzy(double a, double b,int ratio)
        {
            return (Math.Abs(a - b) < ratio);
        }

        public double getFlux(double bpm)
        {
            return beats[(int)(bpm)] + beats[(int)(bpm) - 1] + beats[(int)(bpm) - 2] + beats[(int)(bpm * 2)] + beats[(int)(bpm * 4)] + beats[(int)(bpm*4) - 1] + beats[(int)(bpm*4) - 2] + beats[(int)(bpm * 4) + 1] + beats[(int)(bpm * 4) - 1] + beats[(int)(bpm * 8)];
        }

        public double getFlux3(double bpm)
        {
            return beats[(int)(bpm)] + beats[(int)(bpm) - 1] + beats[(int)(bpm) - 2] + beats[(int)(bpm * 3)] + beats[(int)(bpm * 6)] + beats[(int)(bpm * 9)];
        }

        public void go()
        {
            maxDope = 1;
            for (int r = 100; r < 1000; ++r) beats[r] *= .5;
        }

        double beat = 100;
        double beat2 = 100;
        bool trigger;
        double highFlux = 1;
        public void findBeat()
        {
            trigger = false;
            if (beatDetect.MusicStopped)
            {
                musicStopped();
                return;
            }

           
            if (beatDetect.isPeak)
            {
                 doTrigger(3);
            }

            double decrement = .995;
            if (finalBeats[(int)(beatDetect.BeatLength * 10)] > 200) decrement = .9;
            for (int r = 40; r < 800; ++r) finalBeats[r] *= decrement;
               
         
            double maxFlux = 0;
            double bpm = 2;
    
            for (int r = 50; r < 300; ++r)
            {
                double flux = getFlux(r);
                if(flux>maxFlux)
                {
                    maxFlux = flux;
                    beat2 = bpm;
                    bpm = r;
                }
            }

            beat = bpm;

            double maxFlux2 = 0;
            for (double r = 0; r < 1.1; r += .1)
            {
                double flux =0;
                flux = beatDetect.getFlux2(bpm + r, beatDetect.currentFrame);
                if (flux > maxFlux2)
                {
                    maxFlux2 = flux;
                    beat = bpm+r;
                }
            }
 
            //beat += .5;

          
           double incumbent = beatDetect.beatLength*10;
    
            finalBeats[(int)beat]++;

            double test = beat;
            int timeSignature =  (getFlux3(beat) > getFlux(beat))?3:2;
            //timeSignature = 2;
            if (timeSignature == 3) Console.WriteLine("TRIPLE");

            while (test < 180) test *= timeSignature;
      
            if (fuzzy(test, incumbent,5) )
            {
                finalBeats[(int)((incumbent + test) / 2)] = finalBeats[(int)incumbent] + 3;
                beatDetect.beatLength  = ((incumbent + test) / 20);
                return;
            }

            finalBeats[(int)test]++;
     
  
  
            double maxFinalBeatFlux = 1;
            double maxFinalBeat=1;
            for (int r = 30; r < 500; ++r)
            {
                if (finalBeats[r] > maxFinalBeatFlux)
                {
                    maxFinalBeatFlux = finalBeats[r];
                    maxFinalBeat = r;
                }
            }
            if (maxFinalBeat <= 1) maxFinalBeat = test;
            double inc = maxFinalBeat;
            while (maxFinalBeat < 180) maxFinalBeat *=timeSignature;
       
          
            if ((int)incumbent!=(int)maxFinalBeat)
            {
                beatDetect.beatLength = maxFinalBeat/10;
                for (int i = 0; i < 99; ++i) if (triggers[i] > 0) triggers[i] = 0;
                //double ratio = (incumbent > maxFinalBeat) ? maxFinalBeat / incumbent : incumbent / maxFinalBeat;

                //if(ratio < .95) Console.WriteLine("ratio: "+ ratio.ToString("0.0"));
            }
        }


    }

}