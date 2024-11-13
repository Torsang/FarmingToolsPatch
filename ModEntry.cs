﻿using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Utilities;
using StardewValley.Tools;
using StardewValley.Enchantments;

/***
 ** Original credit to ToweringRedwood for creating this mod in the first place. Many thanks, you're a legend. I hope to do justice to your work.
 ** TODO: Find out how to detect the reach enchant so I can work with it later
 ** Author: Torsang
 ** Latest Build: 2.0.0-beta3
 ** Build Date: 2024-11-12
***/

// Formerly known as IridiumToolsPatch
namespace FarmingToolsPatch
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public static ModConfig config;
        public const int toolMax = 32;
        private const int tickModifier = 60;
        private int resetCounter = 0;

        /*********
        ** Public method(s)
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry ( IModHelper helper )
        {
            config = Helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += this.GameLaunched;
            helper.Events.GameLoop.UpdateTicked += this.UpdateTicking;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
            //helper.Events.GameLoop.OneSecondUpdateTicking += this.OneSecondUpdateTick;

            Harmony harmony = new Harmony ( ModManifest.UniqueID );

            Type[] types = new Type[] { typeof ( Vector2 ), typeof ( int ), typeof ( Farmer ) };
            MethodInfo originalToolsMethod = typeof ( Tool ).GetMethod ( "tilesAffected", BindingFlags.Instance | BindingFlags.NonPublic, null, types, null );
            MethodInfo farmingToolsPatch = typeof ( PatchedFarmingTilesAffected ).GetMethod ( "Postfix" );

            harmony.Patch( originalToolsMethod, null, new HarmonyMethod ( farmingToolsPatch ) );
        }

        /*********
        ** Private Methods
        *********/
        private void GameLaunched ( object sender, GameLaunchedEventArgs e )
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>( "spacechase0.GenericModConfigMenu" );

            if ( configMenu != null )
            { GenModConfigMenu.configurate( this, configMenu ); }
        }

        /*
          * Necessary to handle press vs hold functionality? IDK maybe I'm just dumb
          * The OnButtonsChanged method only seems to fire during presses/releases, it doesn't track button holds
          * Hopefully this is lightweight enough not to cause problems, fingers crossed
        */
        private void UpdateTicking ( object sender, EventArgs e )
        {
            // Ignore if save has not finished loading
            if ( Context.IsWorldReady )
            {
                //Ignore if hot keys are disabled
                if ( config.hKeyBool )
                {
                    config.resetHold = config.cyclePwrLvl.GetState ();

                    if ( config.resetHold == SButtonState.Held )
                    {
                        this.resetCounter++;

                        if ( this.resetCounter >= config.resetTime * tickModifier )
                        {
                            // Suppressing a single key, even if in a key-combo, will break the SButtonState.Held status
                            Helper.Input.Suppress ( config.cyclePwrLvl.Keybinds [0].Buttons [0] );

                            // Using the boolean to send the message will only send the message once
                            config.resetBool = true;
                            if ( config.resetBool )
                                Game1.addHUDMessage ( new HUDMessage ( this.Helper.Translation.Get ( "reset-notify" ), 800f, false ) );
                            config = Helper.ReadConfig<ModConfig> ();
                        }
                    }
                    else if ( config.resetHold == SButtonState.Released && this.resetCounter <= ( config.resetTime * tickModifier / 4 ) )
                    { cyclePowerLevel (); }
                    else if ( this.resetCounter != 0 )
                    { this.resetCounter = 0; }
                }
            }
        }

        private void OnButtonsChanged ( object sender, ButtonsChangedEventArgs e )
        {
            // Ignore if player hasn't loaded a save yet or hot keys are disabled
            if ( !Context.IsWorldReady || !config.hKeyBool )
            { return; }

            if ( config.incLengthBtn.JustPressed () )
            { adjustLength ( true ); }
            if ( config.decLengthBtn.JustPressed () )
            { adjustLength ( false ); }
            if ( config.incRadiusBtn.JustPressed () )
            { adjustRadius ( true ); }
            if ( config.decRadiusBtn.JustPressed () )
            { adjustRadius ( false ); }
        }

        private void adjustLength ( bool increase = false )
        {
            switch ( config.pwrIndex )
            {
                case (int) Pwr.Copper:
                    config.cLength = Math.Clamp ( config.cLength + ( (increase) ? (1) : (-1) ), 1, toolMax );
                    break;

                case (int) Pwr.Steel:
                    config.sLength = Math.Clamp ( config.sLength + ( (increase) ? (1) : (-1) ), 1, toolMax);
                    break;
                    
                case (int) Pwr.Gold:
                    config.gLength = Math.Clamp ( config.gLength + ( (increase) ? (1) : (-1) ), 1, toolMax);
                    break;

                case (int) Pwr.Iridium:
                    config.iLength = Math.Clamp ( config.iLength + ( (increase) ? (1) : (-1) ), 1, toolMax );
                    break;

                default:
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( (increase) ? this.Helper.Translation.Get ( "longer" ) : this.Helper.Translation.Get ( "shorter" ), 800f, false ) );
        }

        private void adjustRadius (bool increase = false)
        {
            switch (config.pwrIndex)
            {
                case (int) Pwr.Copper:
                    config.cRadius = Math.Clamp ( config.cRadius + ( (increase) ? (1) : (-1) ), 0, toolMax);
                    break;

                case (int) Pwr.Steel:
                    config.sRadius = Math.Clamp ( config.sRadius + ( (increase) ? (1) : (-1) ), 0, toolMax );
                    break;

                case (int) Pwr.Gold:
                    config.gRadius = Math.Clamp ( config.gRadius + ( (increase) ? (1) : (-1) ), 0, toolMax );
                    break;

                case (int) Pwr.Iridium:
                    config.iRadius = Math.Clamp ( config.iRadius + ( (increase) ? (1) : (-1) ), 0, toolMax );
                    break;

                default:
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( (increase) ? this.Helper.Translation.Get ( "wider" ) : this.Helper.Translation.Get ( "narrower" ), 800f, false ) );
        }

        private void cyclePowerLevel ()
        {
            var message = this.Helper.Translation.Get ( "affect-default" );
            switch ( config.pwrIndex )
            {
                case (int) Pwr.Copper:
                    config.pwrIndex = (int) Pwr.Steel;
                    message = this.Helper.Translation.Get ( "affect-steel" );
                    break;

                case (int) Pwr.Steel:
                    config.pwrIndex = (int) Pwr.Gold;
                    message = this.Helper.Translation.Get ( "affect-gold" );
                    break;

                case (int) Pwr.Gold:
                    config.pwrIndex = (int) Pwr.Iridium;
                    message = this.Helper.Translation.Get ( "affect-iridium" );
                    break;

                case (int) Pwr.Iridium:
                    config.pwrIndex = (int) Pwr.Copper;
                    message = this.Helper.Translation.Get ( "affect-copper" );
                    break;

                default:
                    config.pwrIndex = (int) Pwr.Copper;
                    message = this.Helper.Translation.Get ( "affect-copper" );
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( message, 800f, false ) );
        }

        class PatchedFarmingTilesAffected
        {
            static public void Postfix (ref List<Vector2> __result, Vector2 tileLocation, int power, Farmer who )
            {
                if ( who.CurrentTool is ( WateringCan or Hoe ) )
                {
                    __result.Clear ();
                    Vector2 direction;
                    Vector2 orthogonal;
                    int length = 1, radius = 0, powerLvl = 1;

                    // Which direction to orient the AOE, and establish matrix multipliers to that effect
                    switch ( who.FacingDirection )
                    {
                        case 0:
                            direction = new Vector2 ( 0, -1 ); orthogonal = new Vector2 ( 1, 0 );
                            break;
                        case 1:
                            direction = new Vector2 ( 1, 0 ); orthogonal = new Vector2 ( 0, 1 );
                            break;
                        case 2:
                            direction = new Vector2 ( 0, 1 ); orthogonal = new Vector2 ( -1, 0 );
                            break;
                        case 3:
                            direction = new Vector2 ( -1, 0 ); orthogonal = new Vector2 ( 0, -1 );
                            break;
                        default:
                            direction = Vector2.Zero; orthogonal = Vector2.Zero;
                            break;
                    }

                    // Override the power param with the current tool's max power index if set to immediately return the max AOE
                    if ( config.mIBool )
                    {
                        powerLvl = who.CurrentTool.UpgradeLevel + 1;
                        if ( who.CurrentTool.hasEnchantmentOfType<ReachingToolEnchantment>() )
                        { powerLvl++; }
                    }
                    else
                    { powerLvl = power; }

                    // Determine the AOE dimensions
                    switch ( powerLvl )
                    {
                        // Set iridium values, modded or vanilla
                        // Check for reaching enchantment and if present, use those values instead
                        case 6:
                            length = ( ModEntry.config.rBool ) ? ModEntry.config.rLength : 5;
                            radius = ( ModEntry.config.rBool ) ? ModEntry.config.rRadius : 2;
                            break;

                        case 5:
                            length = ( ModEntry.config.iBool ) ? ModEntry.config.iLength : 6;
                            radius = ( ModEntry.config.iBool ) ? ModEntry.config.iRadius : 1;
                            break;

                        // Set gold values, modded or vanilla
                        case 4:
                            length = ( ModEntry.config.gBool ) ? ModEntry.config.gLength : 3;
                            radius = ( ModEntry.config.gBool ) ? ModEntry.config.gRadius : 1;
                            break;

                        // Set steel values, modded or vanilla
                        case 3:
                            length = ( ModEntry.config.sBool ) ? ModEntry.config.sLength : 5;
                            radius = ( ModEntry.config.sBool ) ? ModEntry.config.sRadius : 0;
                            break;

                        // Set copper values, modded or vanilla
                        case 2:
                            length = ( ModEntry.config.cBool ) ? ModEntry.config.cLength : 3;
                            radius = ( ModEntry.config.cBool ) ? ModEntry.config.cRadius : 0;
                            break;
                    }

                    // After determining the starting point, orientation, and dimensions, use those to determine the final AOE result
                    for ( int x = 0; x < length; x++ )
                    {
                        __result.Add ( direction * x + tileLocation );
                        for ( int y = 1; y <= radius; y++ )
                        {
                            __result.Add ( ( direction * x ) + ( orthogonal * y ) + tileLocation );
                            __result.Add ( ( direction * x ) + ( orthogonal * -y ) + tileLocation );
                        }
                    }
                }
            }
        }
    }
}
