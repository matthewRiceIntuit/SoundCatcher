using System;
using System.Collections.Generic;
using System.Text;

namespace SoundCatcher.Objects
{
    class Chase
    {
        public Chase(int par1,int par2,int par3)
        {
            this.par1 = par1;
            this.par2 = par2;
            this.par3 = par3;
        }
        public Chase(int par1, int par2)
        {
            this.par1 = par1;
            this.par2 = par2;
            this.par3 = -1;
        }
        public Chase(int par1)
        {
            this.par1 = par1;
            this.par2 = -1;
            this.par3 = -1;
        }

        public Chase()
        {
            this.par1 = -1;
            this.par2 = -1;
            this.par3 = -1;
        }
        public int par1, par2, par3;
    }
}
