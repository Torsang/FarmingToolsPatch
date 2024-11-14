using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericModConfigMenu;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System.Runtime.CompilerServices;

namespace FarmingToolsPatch
{
    internal static class GenModConfigMenu
    {
        public static void configurate ( IMod mod, IGenericModConfigMenuApi cfgMenu )
        {

            // register mod
            cfgMenu.Register
            (
                mod: mod.ModManifest,
                reset: () => ModEntry.config = new ModConfig(),
                save: () => mod.Helper.WriteConfig ( ModEntry.config )
            );

            /**********
            ** Top-level Options
            ** Note: Not enough here to merit its own separate page
            **********/
            #region Main Page
            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "main-mod-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "main-mod-toggle" ),
                tooltip: () => mod.Helper.Translation.Get ( "main-mod-toggle-tooltip" ),
                getValue: () => ModEntry.config.mainBool,
                setValue: value => ModEntry.config.mainBool = value
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "gmcm-empty-entry" )
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "hotkey-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "hotkey-toggle" ),
                tooltip: () => mod.Helper.Translation.Get ( "hktoggle-tooltip" ),
                getValue: () => ModEntry.config.hKeyBool,
                setValue: value => ModEntry.config.hKeyBool = value
            );

            cfgMenu.AddPageLink
            (
                mod: mod.ModManifest,
                pageId: mod.Helper.Translation.Get ( "hotkey-pageID" ),
                text: () => mod.Helper.Translation.Get ( "hotkey-page-name" ),
                tooltip: () => mod.Helper.Translation.Get ( "hotkey-page-tooltip" )
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "gmcm-empty-entry" )
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "aoe-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "max-immediate-toggle" ),
                tooltip: () => mod.Helper.Translation.Get ( "max-immediate-toggle-tooltip" ),
                getValue: () => ModEntry.config.mIBool,
                setValue: value => ModEntry.config.mIBool = value
            );

            cfgMenu.AddPageLink
            (
                mod: mod.ModManifest,
                pageId: mod.Helper.Translation.Get ( "aoe-pageID" ),
                text: () => mod.Helper.Translation.Get ( "aoe-page-name" ),
                tooltip: () => mod.Helper.Translation.Get ( "aoe-page-tooltip" )
            );
            #endregion Main Page

            /**********
            ** Hotkey Page
            **********/
            #region Hotkey Page
            cfgMenu.AddPage
            (
                mod: mod.ModManifest,
                pageId: mod.Helper.Translation.Get ( "hotkey-pageID" ),
                pageTitle: () => mod.Helper.Translation.Get ( "hotkey-page-name" )
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "length-inc-btn" ),
                tooltip: () => mod.Helper.Translation.Get ( "length-ib-tooltip" ),
                getValue: () => ModEntry.config.incLengthBtn,
                setValue: value => ModEntry.config.incLengthBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "radius-inc-btn" ),
                tooltip: () => mod.Helper.Translation.Get ( "radius-ib-tooltip" ),
                getValue: () => ModEntry.config.incRadiusBtn,
                setValue: value => ModEntry.config.incRadiusBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "length-dec-btn" ),
                tooltip: () => mod.Helper.Translation.Get ( "length-db-tooltip" ),
                getValue: () => ModEntry.config.decLengthBtn,
                setValue: value => ModEntry.config.decLengthBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "radius-dec-btn" ),
                tooltip: () => mod.Helper.Translation.Get ( "radius-db-tooltip" ),
                getValue: () => ModEntry.config.decRadiusBtn,
                setValue: value => ModEntry.config.decRadiusBtn = value
            );
            cfgMenu.AddKeybindList
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "cycle-chargelvl-btn" ),
                tooltip: () => mod.Helper.Translation.Get ( "cycle-cl-tooltip" ),
                getValue: () => ModEntry.config.cyclePwrLvl,
                setValue: value => ModEntry.config.cyclePwrLvl = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "reset-default-btn" ),
                tooltip: () => mod.Helper.Translation.Get ( "reset-tooltip" ),
                getValue: () => ModEntry.config.resetTime,
                setValue: value => ModEntry.config.resetTime = value,
                min: 1,
                max: 5,
                interval: 1
            );
            #endregion Hotkey Page

            /**********
            ** AOE Page
            **********/
            #region AOE Page
            cfgMenu.AddPage
            (
                mod: mod.ModManifest,
                pageId: mod.Helper.Translation.Get ( "aoe-pageID" ),
                pageTitle: () => mod.Helper.Translation.Get ( "aoe-page-name" )
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "reach-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "reach-bool" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-bool-tooltip" ),
                getValue: () => ModEntry.config.rBool,
                setValue: value => ModEntry.config.rBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-length" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-l-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Reaching, (int) Dim.Length],
                setValue: value => ModEntry.config.modT [(int) Pwr.Reaching, (int) Dim.Length] = value,
                min: 1,
                max: ModEntry.toolMax,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-radius" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-r-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Reaching, (int) Dim.Radius],
                setValue: value => ModEntry.config.modT [(int) Pwr.Reaching, (int) Dim.Radius] = value,
                min: 0,
                max: ModEntry.toolMax,
                interval: 1
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "iridium-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "iridium-bool" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-bool-tooltip" ),
                getValue: () => ModEntry.config.iBool,
                setValue: value => ModEntry.config.iBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-length" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-l-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Iridium, (int) Dim.Length],
                setValue: value => ModEntry.config.modT [(int) Pwr.Iridium, (int) Dim.Length] = value,
                min: 1,
                max: ModEntry.toolMax,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-radius" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-r-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Iridium, (int) Dim.Radius],
                setValue: value => ModEntry.config.modT [(int) Pwr.Iridium, (int) Dim.Radius] = value,
                min: 0,
                max: ModEntry.toolMax,
                interval: 1
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "gold-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "gold-bool" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-bool-tooltip" ),
                getValue: () => ModEntry.config.gBool,
                setValue: value => ModEntry.config.gBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-length" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-l-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Gold, (int) Dim.Length],
                setValue: value => ModEntry.config.modT [(int) Pwr.Gold, (int) Dim.Length] = value,
                min: 1,
                max: ModEntry.toolMax,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-radius" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-r-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Gold, (int) Dim.Radius],
                setValue: value => ModEntry.config.modT [(int) Pwr.Gold, (int) Dim.Radius] = value,
                min: 0,
                max: ModEntry.toolMax,
                interval: 1
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "steel-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "steel-bool" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-bool-tooltip" ),
                getValue: () => ModEntry.config.sBool,
                setValue: value => ModEntry.config.sBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-length" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-l-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Steel, (int) Dim.Length],
                setValue: value => ModEntry.config.modT [(int) Pwr.Steel, (int) Dim.Length] = value,
                min: 1,
                max: ModEntry.toolMax,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-radius" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-r-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Steel, (int) Dim.Radius],
                setValue: value => ModEntry.config.modT [(int) Pwr.Steel, (int) Dim.Radius] = value,
                min: 0,
                max: ModEntry.toolMax,
                interval: 1
            );

            cfgMenu.AddSectionTitle
            (
                mod: mod.ModManifest,
                text: () => mod.Helper.Translation.Get ( "copper-section" )
            );
            cfgMenu.AddBoolOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "copper-bool" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-bool-tooltip" ),
                getValue: () => ModEntry.config.cBool,
                setValue: value => ModEntry.config.cBool = value
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-length" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-l-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Copper, (int) Dim.Length],
                setValue: value => ModEntry.config.modT [(int) Pwr.Copper, (int) Dim.Length] = value,
                min: 1,
                max: ModEntry.toolMax,
                interval: 1
            );
            cfgMenu.AddNumberOption
            (
                mod: mod.ModManifest,
                name: () => mod.Helper.Translation.Get ( "tool-radius" ),
                tooltip: () => mod.Helper.Translation.Get ( "tool-r-tooltip" ),
                getValue: () => ModEntry.config.modT [(int) Pwr.Copper, (int) Dim.Radius],
                setValue: value => ModEntry.config.modT [(int) Pwr.Copper, (int) Dim.Radius] = value,
                min: 0,
                max: ModEntry.toolMax,
                interval: 1
            );
            #endregion AOE Page
        }
    }
}
