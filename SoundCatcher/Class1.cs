using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SoundCatcher
{
    class Volume
    {

        [DllImport("Volume.dll", EntryPoint = "getVolume",ExactSpelling=false,CallingConvention=CallingConvention.Cdecl)]
        public static extern int getVolume();

        [DllImport("Volume.dll", EntryPoint = "setVolume", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setVolume(int volume);
 
 
    }
}
