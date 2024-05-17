using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericModConfigMenu;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace FarmingToolsPatch
{
    internal static class GenModConfigMenu
    {
        public static void configurate(IMod mod, IGenericModConfigMenuApi cfgMenu)
        {

            // register mod
            cfgMenu.Register
            (
                mod: mod.ModManifest,
                reset: () => ModEntry.config = new ModConfig(),
                save: () => mod.Helper.WriteConfig(ModEntry.config)
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => "Hotkey Options"
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => "Toggle Hot Keys",
                tooltip: () => "Check to enable hot keys, uncheck to disable",
                getValue: () => ModEntry.config.hKeyBool,
                setValue: value => ModEntry.config.hKeyBool = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => "Increase Length",
                tooltip: () => "Add tiles to the reach of your tool's area of effect",
                getValue: () => ModEntry.config.incLengthBtn,
                setValue: value => ModEntry.config.incLengthBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => "Increase Radius",
                tooltip: () => "Add tiles laterally to your tool's area of effect",
                getValue: () => ModEntry.config.incRadiusBtn,
                setValue: value => ModEntry.config.incRadiusBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => "Decrease Length",
                tooltip: () => "Remove tiles from the reach of your tool's area of effect",
                getValue: () => ModEntry.config.decLengthBtn,
                setValue: value => ModEntry.config.decLengthBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => "Decrease Radius",
                tooltip: () => "Remove tiles laterally from your tool's area of effect",
                getValue: () => ModEntry.config.decRadiusBtn,
                setValue: value => ModEntry.config.decRadiusBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => "Cycle Charge Level",
                tooltip: () => "Cycle (one way) through the power levels to adjust their area of effect",
                getValue: () => ModEntry.config.cyclePwrLvl,
                setValue: value => ModEntry.config.cyclePwrLvl = value
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => "Iridium Tools"
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => "Modify Iridum Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => ModEntry.config.iBool,
                setValue: value => ModEntry.config.iBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => ModEntry.config.iLength,
                setValue: value => ModEntry.config.iLength = value,
                min: 1,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => ModEntry.config.iRadius,
                setValue: value => ModEntry.config.iRadius = value,
                min: 0,
                interval: 1
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => "Gold Tools"
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => "Modify Gold Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => ModEntry.config.gBool,
                setValue: value => ModEntry.config.gBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => ModEntry.config.gLength,
                setValue: value => ModEntry.config.gLength = value,
                min: 1,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => ModEntry.config.gRadius,
                setValue: value => ModEntry.config.gRadius = value,
                min: 0,
                interval: 1
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => "Steel Tools"
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => "Modify Steel Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => ModEntry.config.sBool,
                setValue: value => ModEntry.config.sBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => ModEntry.config.sLength,
                setValue: value => ModEntry.config.sLength = value,
                min: 1,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => ModEntry.config.sRadius,
                setValue: value => ModEntry.config.sRadius = value,
                min: 0,
                interval: 1
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => "Copper Tools"
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => "Modify Copper Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => ModEntry.config.cBool,
                setValue: value => ModEntry.config.cBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => ModEntry.config.cLength,
                setValue: value => ModEntry.config.cLength = value,
                min: 1,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => ModEntry.config.cRadius,
                setValue: value => ModEntry.config.cRadius = value,
                min: 0,
                interval: 1
            );
        }
    }
}
