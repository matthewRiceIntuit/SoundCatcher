using System;
using System.Collections.Generic;
using System.Text;

namespace SoundCatcher.Objects
{
    class FlurryPos
    {
        public FlurryPos(string _name, int _pan, int _tilt,int _rightPan,int _rightTilt)
        {
            this.name = _name;
            this.pan = _pan;
            this.tilt = _tilt;
            this.rightPan = _rightPan;
            this.rightTilt = _rightTilt;
        }
       
        public string name;
        public int pan;
        public int tilt;
        public int rightPan;
        public int rightTilt;
    }
}
