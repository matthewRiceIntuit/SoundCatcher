using System;
using System.Collections.Generic;
using System.Text;

namespace SoundCatcher.Objects
{
    class ConfigParam
    {
        public ConfigParam(string _name, int _frequency, string _description)
        {
            this.name = _name;
            this.frequency = _frequency;
            this.description = description;
        }
       
        public string name;
        public int frequency;
        public int description;
      }
}
