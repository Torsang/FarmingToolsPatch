using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingToolsPatch
{
    public class ModConfig
    {
        public int bLength, cLength, sLength, gLength, iLength;
        public int bRadius, cRadius, sRadius, gRadius, iRadius;

        public ModConfig()
        {
            iLength = 5;
            iRadius = 2;
        }
    }
}
