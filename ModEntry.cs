using System;
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

/***
 ** Original credit to ToweringRedwood for creating this mod in the first place. Many thanks, you're a legend. I hope to do justice to your work.
***/

//formerly known as IridiumToolsPatch
namespace FarmingToolsPatch
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        public static ModConfig config;
        private const int enumOffset = 2;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry ( IModHelper helper )
        {
            config = Helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.GameLaunched += this.GameLaunched;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            Harmony harmony = new Harmony ( "Torsang.PatchedFarmingTools" );
            Type[] types = new Type[] { typeof ( Vector2 ), typeof ( int ), typeof ( Farmer ) };
            MethodInfo originalToolsMethod = typeof ( Tool ).GetMethod ( "tilesAffected", BindingFlags.Instance | BindingFlags.NonPublic, null, types, null );
            MethodInfo farmingToolsPatch = typeof ( PatchedFarmingTilesAffected ).GetMethod( "Postfix" );
            harmony.Patch( originalToolsMethod, null, new HarmonyMethod ( farmingToolsPatch ) );
        }

        private void GameLaunched ( object sender, GameLaunchedEventArgs e )
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>( "spacechase0.GenericModConfigMenu" );

            if ( configMenu != null )
            {
                GenModConfigMenu.configurate( this, configMenu );
            }
        }

        /*
         * TODO: Find out how to detect the reach enchant so I can work with it later
        */
        private void OnButtonPressed ( object sender, ButtonPressedEventArgs e )
        {
            // ignore if player hasn't loaded a save yet or hot keys are disabled
            if ( !Context.IsWorldReady || !config.hKeyBool )
                return;
            
            if ( config.incLengthBtn.JustPressed() )
                adjustLength ( true );
            if ( config.decLengthBtn.JustPressed() )
                adjustLength ( false );
            if ( config.incRadiusBtn.JustPressed() )
                adjustRadius ( true );
            if ( config.decRadiusBtn.JustPressed() )
                adjustRadius ( false );
            if ( config.cyclePwrLvl.JustPressed())
                cyclePowerLevel ();

            // print button presses to the console window
            // Added for debugging purposes
            /*if (e.Button is SButton.MouseLeft)
            {
                this.Monitor.Log($"Player has {Game1.player.enchantments.Count} enchantments.", LogLevel.Debug);
                if (Game1.player.enchantments.Count > 0)
                {
                    foreach (BaseEnchantment ench in Game1.player.enchantments)
                    {
                        this.Monitor.Log($"Player has the following enchantments: {ench}.", LogLevel.Debug);
                    }
                }
            }*/
        }

        private void adjustLength ( bool increase = false )
        {
            switch ( config.pwrIndex )
            {
                case (int) Pwr.Copper:
                    config.cLength = Math.Clamp ( config.cLength + ( (increase) ? (1) : (-1) ), 1, 128 );
                    break;

                case (int) Pwr.Steel:
                    config.sLength = Math.Clamp ( config.sLength + ( (increase) ? (1) : (-1) ), 1, 128 );
                    break;
                    
                case (int) Pwr.Gold:
                    config.gLength = Math.Clamp ( config.gLength + ( (increase) ? (1) : (-1) ), 1, 128 );
                    break;

                case (int) Pwr.Iridium:
                    config.iLength = Math.Clamp ( config.iLength + ( (increase) ? (1) : (-1) ), 1, 128 );
                    break;

                default:
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( (increase) ? "Longer..." : "Shorter...", Color.White, 800f ) );
        }

        private void adjustRadius (bool increase = false)
        {
            switch (config.pwrIndex)
            {
                case (int) Pwr.Copper:
                    config.cRadius = Math.Clamp ( config.cRadius + ( (increase) ? (1) : (-1) ), 0, 128 );
                    break;

                case (int) Pwr.Steel:
                    config.sRadius = Math.Clamp ( config.sRadius + ( (increase) ? (1) : (-1) ), 0, 128 );
                    break;

                case (int) Pwr.Gold:
                    config.gRadius = Math.Clamp ( config.gRadius + ( (increase) ? (1) : (-1) ), 0, 128 );
                    break;

                case (int) Pwr.Iridium:
                    config.iRadius = Math.Clamp ( config.iRadius + ( (increase) ? (1) : (-1) ), 0, 128 );
                    break;

                default:
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( (increase) ? "Wider..." : "Narrower...", Color.White, 800f ) );
        }

        private void cyclePowerLevel ()
        {
            var message = "Now Affecting...";
            switch ( config.pwrIndex )
            {
                case (int) Pwr.Copper:
                    config.pwrIndex = (int) Pwr.Steel;
                    message = "Now Affecting Steel...";
                    break;

                case (int) Pwr.Steel:
                    config.pwrIndex = (int) Pwr.Gold;
                    message = "Now Affecting Gold...";
                    break;

                case (int) Pwr.Gold:
                    config.pwrIndex = (int) Pwr.Iridium;
                    message = "Now Affecting Iridium...";
                    break;

                case (int) Pwr.Iridium:
                    config.pwrIndex = (int) Pwr.Copper;
                    message = "Now Affecting Copper...";
                    break;

                default:
                    config.pwrIndex = (int) Pwr.Copper;
                    message = "Now Affecting Copper...";
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( message, Color.White, 800f ) );
        }

        class PatchedFarmingTilesAffected
        {
            static public void Postfix ( ref List<Vector2> __result, Vector2 tileLocation, int power, Farmer who )
            {
                __result.Clear ();
                Vector2 direction;
                Vector2 orthogonal;
                int length = 1, radius = 0;

                switch ( who.FacingDirection )
                {
                    case 0:
                        direction = new Vector2(0, -1); orthogonal = new Vector2(1, 0);
                        break;
                    case 1:
                        direction = new Vector2(1, 0); orthogonal = new Vector2(0, 1);
                        break;
                    case 2:
                        direction = new Vector2(0, 1); orthogonal = new Vector2(-1, 0);
                        break;
                    case 3:
                        direction = new Vector2(-1, 0); orthogonal = new Vector2(0, -1);
                        break;
                    default:
                        direction = Vector2.Zero; orthogonal = Vector2.Zero;
                        break;
                }

                //Set copper values, modded or vanilla
                if ( power >= 2 )
                {
                    length = ( ModEntry.config.cBool ) ? ModEntry.config.cLength : 3;
                    radius = ( ModEntry.config.cBool ) ? ModEntry.config.cRadius : 0;
                }

                //Set steel values, modded or vanilla
                if ( power >= 3 )
                {
                    length = ( ModEntry.config.sBool ) ? ModEntry.config.sLength : 5;
                    radius = ( ModEntry.config.sBool ) ? ModEntry.config.sRadius : 0;
                }

                //Set gold values, modded or vanilla
                if ( power >= 4 )
                {
                    length = ( ModEntry.config.gBool ) ? ModEntry.config.gLength : 3;
                    radius = ( ModEntry.config.gBool ) ? ModEntry.config.gRadius : 1;
                }

                //Set iridium values, modded or vanilla
                if ( power >= 5 )
                {
                    length = ( ModEntry.config.iBool ) ? ModEntry.config.iLength : 6;
                    radius = ( ModEntry.config.iBool ) ? ModEntry.config.iRadius : 1;
                }

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
