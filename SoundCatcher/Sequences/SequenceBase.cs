using System;
using System.Collections.Generic;
using System.Text;
using SoundCatcher.Objects;

namespace SoundCatcher.Sequences
{
    class SequenceBase
    {
        public int ticksPerCall = 1;
        public bool Done = true;

        protected SequenceController controller = null;
        public SequenceController Controller { set { controller = value; } }
        public Random random = new Random();
        public bool needsBeat = true;

        public List<ConfigParam> ConfigParams = new List<ConfigParam>();


        public virtual void init()
        {
            
        }

 
        public virtual void go()
        {

        }

        public bool coinFlip()
        {
            return (random.Next(2) == 0);
        }

        public bool param(string name)
        {
            foreach (ConfigParam p in ConfigParams)
            {
                if (p.name == name)
                {
                    if (p.frequency == 0) return false;
                    if (p.frequency == 1) return true;
                    return (random.Next(p.frequency) == 0);
                }

            }
            throw new Exception("SequenceBase - Param name not found: " + name );
            return false;
        }

        public ConfigParam getParam(string name)
        {
            foreach (ConfigParam p in ConfigParams)
            {
                if (p.name == name) return p;
            }
            throw new Exception("SequenceBase - Param name not found: " + name );
            return null;
        }
        public void setParam(string name, int frequency)
        {
            ConfigParam p = getParam(name);
            p.frequency = frequency;
            
        }
     
    }
}
