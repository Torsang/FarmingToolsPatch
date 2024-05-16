using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingToolsPatch
{
    public class ModConfig
    {
        public int cLength, sLength, gLength, iLength;
        public int cRadius, sRadius, gRadius, iRadius;
        public bool cBool, sBool, gBool, iBool;

        public ModConfig()
        {
            //Iridium fields
            iLength = 5;
            iRadius = 2;
            iBool = true;

            //Gold fields
            gLength = 6;
            gRadius = 1;
            gBool = true;

            //Steel fields
            sLength = 3;
            sRadius = 1;
            sBool = true;

            //Copper fields
            cLength = 3;
            cRadius = 0;
            cBool = true;
        }
    }
}
