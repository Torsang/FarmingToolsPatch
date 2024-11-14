using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace FarmingToolsPatch
{
    public class Pwr
    {
        public const int Copper = 0;
        public const int Steel = 1;
        public const int Gold = 2;
        public const int Iridium = 3;
        public const int Reaching = 4;
    }

    public class Dim
    {
        public const int Length = 0;
        public const int Radius = 1;
    }

    public class ModConfig
    {
        public KeybindList incLengthBtn, incRadiusBtn, decLengthBtn, decRadiusBtn, cyclePwrLvl;
        public SButtonState resetHold;
        public int pwrIndex, resetTime;
        public int cLength, sLength, gLength, iLength, rLength;
        public int cRadius, sRadius, gRadius, iRadius, rRadius;
        public bool cBool, sBool, gBool, iBool, rBool, mIBool, hKeyBool, resetBool, mainBool;
        public int[,] modT = new int[5,2];
        public int[,] baseT = new int[5,2];

        public ModConfig()
        {
            // Main mod field
            mainBool = true;
            
            // Hotkey fields
            incLengthBtn = new KeybindList(SButton.OemOpenBrackets);
            incRadiusBtn = new KeybindList(SButton.OemCloseBrackets);
            decLengthBtn = new KeybindList(SButton.OemSemicolon);
            decRadiusBtn = new KeybindList(SButton.OemQuotes);
            cyclePwrLvl = new KeybindList(SButton.OemPipe);
            hKeyBool = true;
            resetBool = false;
            pwrIndex = Pwr.Copper;
            resetTime = 2;

            // Max Immediate field
            mIBool = false;

            // Reaching fields, modded and vanilla
            //rLength = 7;
            //rRadius = 3;
            rBool = true;
            modT[Pwr.Reaching, Dim.Length] = 7;
            modT[Pwr.Reaching, Dim.Radius] = 3;
            baseT[Pwr.Reaching, Dim.Length] = 5;
            baseT[Pwr.Reaching, Dim.Radius] = 2;

            // Iridium fields, modded and vanilla
            //iLength = 5;
            //iRadius = 2;
            iBool = true;
            modT[Pwr.Iridium, Dim.Length] = 5;
            modT[Pwr.Iridium, Dim.Radius] = 2;
            baseT[Pwr.Iridium, Dim.Length] = 6;
            baseT[Pwr.Iridium, Dim.Radius] = 1;

            // Gold fields, modded and vanilla
            //gLength = 6;
            //gRadius = 1;
            gBool = true;
            modT[Pwr.Gold, Dim.Length] = 6;
            modT[Pwr.Gold, Dim.Radius] = 1;
            baseT[Pwr.Gold, Dim.Length] = 3;
            baseT[Pwr.Gold, Dim.Radius] = 1;

            // Steel fields, modded and vanilla
            //sLength = 3;
            //sRadius = 1;
            sBool = true;
            modT[Pwr.Steel, Dim.Length] = 3;
            modT[Pwr.Steel, Dim.Radius] = 1;
            baseT[Pwr.Steel, Dim.Length] = 5;
            baseT[Pwr.Steel, Dim.Radius] = 0;

            // Copper fields, modded and vanilla
            //cLength = 3;
            //cRadius = 0;
            cBool = true;
            modT[Pwr.Copper, Dim.Length] = 3;
            modT[Pwr.Copper, Dim.Radius] = 0;
            baseT[Pwr.Copper, Dim.Length] = 3;
            baseT[Pwr.Copper, Dim.Radius] = 0;
        }
    }
}
