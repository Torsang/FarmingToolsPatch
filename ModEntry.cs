using HarmonyLib;
using Microsoft.Xna.Framework;
using System.Reflection;
using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using GenericModConfigMenu;
using StardewValley.Tools;
using StardewValley.Enchantments;

/***
 ** Original credit to ToweringRedwood for creating this mod in the first place. Many thanks, you're a legend. I hope to do justice to your work.
 ** TODO: Confirm compatibility with DaLion's Chargeable Resource Tools https://www.nexusmods.com/stardewvalley/mods/23048
 ** Author: Torsang
 ** Latest Build: 2.0.0-beta6
 ** Build Date: 2024-11-17
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
            MethodInfo farmingToolsPatch = typeof ( ModEntry ).GetMethod ( "Postfix" );

            harmony.Patch( originalToolsMethod, null, new HarmonyMethod ( farmingToolsPatch ) );
        }

        /*********
        ** Private Methods
        *********/
        #region SMAPI method-appending methods
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
                // Ignore if mod and/or hot keys are disabled
                if ( config.hKeyBool && config.mainBool )
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
            // Ignore if player hasn't loaded a save yet
            if ( Context.IsWorldReady )
            {
                // Ignore if the mod and/or hot keys are disabled
                if ( config.hKeyBool && config.mainBool )
                {
                    if ( config.incLengthBtn.JustPressed () )
                    { adjustLength ( true ); }
                    if ( config.decLengthBtn.JustPressed () )
                    { adjustLength ( false ); }
                    if ( config.incRadiusBtn.JustPressed () )
                    { adjustRadius ( true ); }
                    if ( config.decRadiusBtn.JustPressed () )
                    { adjustRadius ( false ); }
                }
            }
        }
        #endregion SMAPI method-appending methods

        /*
          * Do the actual configuration changes when called from OnButtonsChanged above
        */
        #region Hotkey methods
        private void adjustLength ( bool increase = false )
        {
            switch ( config.pwrIndex )
            {
                case Pwr.Copper:
                    config.modT[Pwr.Copper, Dim.Length] = Math.Clamp ( config.modT[Pwr.Copper, Dim.Length] + ( increase ? 1 : -1 ), 1, toolMax );
                    break;

                case Pwr.Steel:
                    config.modT[Pwr.Steel, Dim.Length] = Math.Clamp ( config.modT[Pwr.Steel, Dim.Length] + ( increase ? 1 : -1 ), 1, toolMax);
                    break;
                    
                case Pwr.Gold:
                    config.modT[Pwr.Gold, Dim.Length] = Math.Clamp ( config.modT[Pwr.Gold, Dim.Length] + ( increase ? 1 : -1 ), 1, toolMax );
                    break;

                case Pwr.Iridium:
                    config.modT[Pwr.Iridium, Dim.Length] = Math.Clamp ( config.modT[Pwr.Iridium, Dim.Length] + ( increase ? 1 : -1 ), 1, toolMax );
                    break;

                case Pwr.Reaching:
                    config.modT[Pwr.Reaching, Dim.Length] = Math.Clamp ( config.modT[Pwr.Reaching, Dim.Length] + ( increase ? 1 : -1 ), 1, toolMax );
                    break;

                default:
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( increase ? this.Helper.Translation.Get ( "longer" ) : this.Helper.Translation.Get ( "shorter" ), 800f, false ) );
        }

        private void adjustRadius (bool increase = false)
        {
            switch ( config.pwrIndex )
            {
                case Pwr.Copper:
                    config.modT[Pwr.Copper, Dim.Radius] = Math.Clamp ( config.modT[Pwr.Copper, Dim.Radius] + ( increase ? 1 : -1 ), 0, toolMax);
                    break;

                case Pwr.Steel:
                    config.modT[Pwr.Steel, Dim.Radius] = Math.Clamp ( config.modT[Pwr.Steel, Dim.Radius] + ( increase ? 1 : -1 ), 0, toolMax );
                    break;

                case Pwr.Gold:
                    config.modT[Pwr.Gold, Dim.Radius] = Math.Clamp ( config.modT[Pwr.Gold, Dim.Radius] + ( increase ? 1 : -1 ), 0, toolMax );
                    break;

                case Pwr.Iridium:
                    config.modT[Pwr.Iridium, Dim.Radius] = Math.Clamp ( config.modT[Pwr.Iridium, Dim.Radius] + ( increase ? 1 : -1 ), 0, toolMax );
                    break;

                case Pwr.Reaching:
                    config.modT[Pwr.Reaching, Dim.Radius] = Math.Clamp ( config.modT[Pwr.Reaching, Dim.Radius] + ( increase ? 1 : -1 ), 0, toolMax );
                    break;
                
                default:
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( increase ? this.Helper.Translation.Get ( "wider" ) : this.Helper.Translation.Get ( "narrower" ), 800f, false ) );
        }

        private void cyclePowerLevel ()
        {
            var message = this.Helper.Translation.Get ( "affect-default" );
            switch ( config.pwrIndex )
            {
                case Pwr.Copper:
                    config.pwrIndex = Pwr.Steel;
                    message = this.Helper.Translation.Get ( "affect-steel" );
                    break;

                case Pwr.Steel:
                    config.pwrIndex = Pwr.Gold;
                    message = this.Helper.Translation.Get ( "affect-gold" );
                    break;

                case Pwr.Gold:
                    config.pwrIndex = Pwr.Iridium;
                    message = this.Helper.Translation.Get ( "affect-iridium" );
                    break;

                case Pwr.Iridium:
                    config.pwrIndex = Pwr.Reaching;
                    message = this.Helper.Translation.Get ( "affect-reaching" );
                    break;

                case Pwr.Reaching:
                    config.pwrIndex = Pwr.Copper;
                    message = this.Helper.Translation.Get ( "affect-copper" );
                    break;

                default:
                    config.pwrIndex = Pwr.Copper;
                    message = this.Helper.Translation.Get ( "affect-copper" );
                    break;
            }

            Game1.addHUDMessage ( new HUDMessage ( message, 800f, false ) );
        }
        #endregion Hotkey methods

        /*
         * Refactored from Inner Class method to a regular member method
         * This method below is the real core of the mod
         * It was the primary bit that came from ToweringRedwood, expanded by me
        */
        static public void Postfix ( ref List<Vector2> __result, Vector2 tileLocation, int power, Farmer who )
        {
            if ( who.CurrentTool is ( WateringCan or Hoe ) )
            {
                __result.Clear ();
                Vector2 direction;
                Vector2 orthogonal;
                int length = 1, radius = 0, powerLvl = 1;

                // Which direction to orient the AOE, and establish 1D arrays to that effect
                switch ( who.FacingDirection )
                {
                    // Up?
                    case 0:
                        direction = new Vector2 ( 0, -1 ); orthogonal = new Vector2 ( 1, 0 );
                        break;
                    
                    // Right?
                    case 1:
                        direction = new Vector2 ( 1, 0 ); orthogonal = new Vector2 ( 0, 1 );
                        break;
                    
                    // Down?
                    case 2:
                        direction = new Vector2 ( 0, 1 ); orthogonal = new Vector2 ( -1, 0 );
                        break;
                    
                    // Left?
                    case 3:
                        direction = new Vector2 ( -1, 0 ); orthogonal = new Vector2 ( 0, -1 );
                        break;
                    
                    // Fail-state?
                    default:
                        direction = Vector2.Zero; orthogonal = Vector2.Zero;
                        break;
                }

                // Override the power param with the current tool's max power index if set to immediately return the max AOE
                if ( config.mIBool && config.mainBool )
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
                    // Set reaching(enchant) values, modded or base-game
                    case 6:
                        length = ( config.rBool && config.mainBool ) ? config.modT[Pwr.Reaching, Dim.Length] : config.baseT[Pwr.Reaching, Dim.Length];
                        radius = ( config.rBool && config.mainBool ) ? config.modT[Pwr.Reaching, Dim.Radius] : config.baseT[Pwr.Reaching, Dim.Radius];
                        break;

                    // Set iridium values, modded or base-game
                    case 5:
                        length = ( config.iBool && config.mainBool ) ? config.modT[Pwr.Iridium, Dim.Length] : config.baseT[Pwr.Iridium, Dim.Length];
                        radius = ( config.iBool && config.mainBool ) ? config.modT[Pwr.Iridium, Dim.Radius] : config.baseT[Pwr.Iridium, Dim.Radius];
                        break;

                    // Set gold values, modded or base-game
                    case 4:
                        length = ( config.gBool && config.mainBool ) ? config.modT[Pwr.Gold, Dim.Length] : config.baseT[Pwr.Gold, Dim.Length];
                        radius = ( config.gBool && config.mainBool ) ? config.modT[Pwr.Gold, Dim.Radius] : config.baseT[Pwr.Gold, Dim.Radius];
                        break;

                    // Set steel values, modded or base-game
                    case 3:
                        length = ( config.sBool && config.mainBool ) ? config.modT[Pwr.Steel, Dim.Length] : config.baseT[Pwr.Steel, Dim.Length];
                        radius = ( config.sBool && config.mainBool ) ? config.modT[Pwr.Steel, Dim.Radius] : config.baseT[Pwr.Steel, Dim.Radius];
                        break;

                    // Set copper values, modded or base-game
                    case 2:
                        length = ( config.cBool && config.mainBool ) ? config.modT[Pwr.Copper, Dim.Length] : config.baseT[Pwr.Copper, Dim.Length];
                        radius = ( config.cBool && config.mainBool ) ? config.modT[Pwr.Copper, Dim.Radius] : config.baseT[Pwr.Copper, Dim.Radius];
                        break;
                }

                // After determining the starting point, orientation, and dimensions, use those to calculate the final AOE result
                // TODO: Streamline to avoid for-loop?
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
