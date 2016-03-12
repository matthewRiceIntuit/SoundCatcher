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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SoundCatcher
{
    public partial class FormMain : Form
    {
        private WaveInRecorder _recorder;
        private byte[] _recorderBuffer;
        private WaveFormat _waveFormat;
        private AudioFrame _audioFrame;
        private bool _isShown = true;
      
        public FormMain()
        {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {

            //Initialization


         
            if (WaveNative.waveInGetNumDevs() == 0)
            {
                MessageBox.Show("No audio input devices detected\r\n");
            }
            else
            {
                _audioFrame = new AudioFrame();
              
                Start();
            }

            //trackBar1.Value = Volume.getVolume();
            trackBar2.Value = (int)_audioFrame.beatDetect.Distance;// MinAmplitude;

            comboBox1.Items.Add("<auto>");
            comboBox1.Items.Add("White");

            for (int r = 0; r < _audioFrame.sequence.SequenceList.Count; ++r)
            {
                comboBox1.Items.Add(_audioFrame.sequence.SequenceList[r].ToString());
            }
            comboBox1.SelectedIndex = 0;
        }
        private void FormMain_Resize(object sender, EventArgs e)
        {

        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _audioFrame.sequence.dark();
            Stop();
        }
 
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOptionsDialog form = new FormOptionsDialog();
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettingsDialog form = new FormSettingsDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                Stop();
                _audioFrame = new AudioFrame();
                Start();
            }
        }
        private void Start()
        {
            Stop();
            try
            {
                _waveFormat = new WaveFormat(Properties.Settings.Default.SettingSamplesPerSecond, Properties.Settings.Default.SettingBitsPerSample, Properties.Settings.Default.SettingChannels);
                _recorder = new WaveInRecorder(Properties.Settings.Default.SettingAudioInputDevice, _waveFormat, Properties.Settings.Default.SettingBytesPerFrame * Properties.Settings.Default.SettingChannels, 3, new BufferDoneEventHandler(DataArrived));
   
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.ToString());
            }
        }
        private void Stop()
        {
            if (_recorder != null)
                try
                {
                    _recorder.Dispose();
                }
                finally
                {
                    _recorder = null;
                }
        }
 
        private void DataArrived(IntPtr data, int size)
        {

            if (_recorderBuffer == null || _recorderBuffer.Length != size)
                _recorderBuffer = new byte[size];
            if (_recorderBuffer != null)
            {
                System.Runtime.InteropServices.Marshal.Copy(data, _recorderBuffer, 0, size);
 
                _audioFrame.Process(ref _recorderBuffer);
                _audioFrame.DetectBeats();

                _audioFrame.findBeat();
               
                _audioFrame.RenderBeats(ref pictureBoxSpectrogramLeft);
                _audioFrame.RenderFinalBeats(ref pictureBoxFrequencyDomainRight);
                _audioFrame.RenderIntensityPeaks(ref pictureBoxTimeDomainLeft);

                //_audioFrame.RenderSpectrogram(ref pictureBoxSpectrogramLeft);
                //_audioFrame.RenderFrequencyDomain(ref pictureBoxTimeDomainLeft, Properties.Settings.Default.SettingSamplesPerSecond);
                _audioFrame.RenderWaveForm(ref pictureBoxFrequencyWaveForm);
                _audioFrame.RenderPeakChart(ref pictureBoxPeakChart);
                _audioFrame.RenderLevelMeter(ref pictureBoxLevelMeter);
                _audioFrame.sequence.DoSequence();

            }
        }



        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Volume.setVolume(trackBar1.Value);
   
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _audioFrame.beatDetect.Distance = trackBar2.Value;
            Console.WriteLine(trackBar2.Value);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _audioFrame.sequence.setSequenceOVerride(comboBox1.SelectedIndex);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryLeft,trackBar3.Value, trackBar4.Value, 0);
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryLeft2, trackBar3.Value, trackBar4.Value, 0);
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryRight, trackBar3.Value, trackBar4.Value, 0);
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryRight2, trackBar3.Value, trackBar4.Value, 0);
            label1.Text = "Pan: " + trackBar3.Value.ToString() + " tilt: " + trackBar4.Value.ToString();
        }



        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryLeft, trackBar3.Value, trackBar4.Value, 0);
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryLeft2, trackBar3.Value, trackBar4.Value, 0);
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryRight, trackBar3.Value, trackBar4.Value, 0);
            _audioFrame.sequence.flurry.movePosition(_audioFrame.sequence.flurry.flurryRight2, trackBar3.Value, trackBar4.Value, 0);
            label1.Text = "Pan: " + trackBar3.Value.ToString() + " tilt: " + trackBar4.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _audioFrame.sequence.flurryScenes.Go(null);
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _audioFrame.sequence.dark();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
 
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtTags.Text += "Tag(\"" + txtTagName.Text + "\"," + trackBar3.Value + "," + trackBar4.Value + ");\r\n";
        }


    }
}