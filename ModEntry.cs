using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using StardewModdingAPI;
using StardewValley;
using StardewModdingAPI.Events;
using GenericModConfigMenu;
using Microsoft.Xna.Framework.Input;

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

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            config = Helper.ReadConfig<ModConfig>();
            helper.Events.GameLoop.GameLaunched += this.GameLaunched;
            //helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            Harmony harmony = new Harmony("Torsang.PatchedFarmingTools");
            Type[] types = new Type[] { typeof(Vector2), typeof(int), typeof(Farmer) };
            MethodInfo originalToolsMethod = typeof(Tool).GetMethod("tilesAffected", BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
            MethodInfo farmingToolsPatch = typeof(PatchedFarmingTilesAffected).GetMethod("Postfix");
            harmony.Patch(originalToolsMethod, null, new HarmonyMethod(farmingToolsPatch));
        }

        private void GameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
            {
                Monitor.Log("Config Menu is somehow null. Fix it!");
                return;
            }

            // register mod
            configMenu.Register
            (
                mod: this.ModManifest,
                reset: () => config = new ModConfig(),
                save: () => this.Helper.WriteConfig(config)
            );

            /*configMenu.SetTitleScreenOnlyForNextOptions
            (
                mod: this.ModManifest,
                titleScreenOnly: false
            );*/

            configMenu.AddSectionTitle
            (
                mod: this.ModManifest, 
                text: () => "Iridium Tools"
            );
            configMenu.AddBoolOption
            (
                mod: this.ModManifest,
                name: () => "Modify Iridum Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => config.iBool,
                setValue: value => config.iBool = value
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => config.iLength,
                setValue: value => config.iLength = value,
                min: 0,
                interval: 1
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => config.iRadius,
                setValue: value => config.iRadius = value,
                min: 0,
                interval: 1
            );

            configMenu.AddSectionTitle
            (
                mod: this.ModManifest,
                text: () => "Gold Tools"
            );
            configMenu.AddBoolOption
            (
                mod: this.ModManifest,
                name: () => "Modify Gold Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => config.gBool,
                setValue: value => config.gBool = value
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => config.gLength,
                setValue: value => config.gLength = value,
                min: 0,
                interval: 1
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => config.gRadius,
                setValue: value => config.gRadius = value,
                min: 0,
                interval: 1
            );

            configMenu.AddSectionTitle
            (
                mod: this.ModManifest,
                text: () => "Steel Tools"
            );
            configMenu.AddBoolOption
            (
                mod: this.ModManifest,
                name: () => "Modify Steel Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => config.sBool,
                setValue: value => config.sBool = value
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => config.sLength,
                setValue: value => config.sLength = value,
                min: 0,
                interval: 1
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => config.sRadius,
                setValue: value => config.sRadius = value,
                min: 0,
                interval: 1
            );

            configMenu.AddSectionTitle
            (
                mod: this.ModManifest,
                text: () => "Copper Tools"
            );
            configMenu.AddBoolOption
            (
                mod: this.ModManifest,
                name: () => "Modify Copper Tools",
                tooltip: () => "Check to use the below settings, uncheck to use vanilla values",
                getValue: () => config.cBool,
                setValue: value => config.cBool = value
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Length",
                tooltip: () => "Number of tiles away the tool will reach",
                getValue: () => config.cLength,
                setValue: value => config.cLength = value,
                min: 0,
                interval: 1
            );
            configMenu.AddNumberOption
            (
                mod: this.ModManifest,
                name: () => "Radius",
                tooltip: () => "Number of parallel lines on each side of the tool's reach",
                getValue: () => config.cRadius,
                setValue: value => config.cRadius = value,
                min: 0,
                interval: 1
            );
        }

        /* Added primarily for debugging purposes
         * TODO: Find out how to detect the reach enchant so I can work with it later
        */
        /*private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // print button presses to the console window
            if (e.Button is SButton.MouseLeft)
            {
                this.Monitor.Log($"Player has {Game1.player.enchantments.Count} enchantments.", LogLevel.Debug);
                if (Game1.player.enchantments.Count > 0)
                {
                    foreach (BaseEnchantment ench in Game1.player.enchantments)
                    {
                        this.Monitor.Log($"Player has the following enchantments: {ench}.", LogLevel.Debug);
                    }
                }
            }
        }*/

        class PatchedFarmingTilesAffected
        {
            static public void Postfix(ref List<Vector2> __result, Vector2 tileLocation, int power, Farmer who)
            {
                __result.Clear();
                Vector2 direction;
                Vector2 orthogonal;
                int length = 1, radius = 0;

                switch (who.FacingDirection)
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
                if (power >= 2)
                {
                    length = (ModEntry.config.cBool) ? ModEntry.config.cLength : 3;
                    radius = (ModEntry.config.cBool) ? ModEntry.config.cRadius : 0;
                }

                //Set steel values, modded or vanilla
                if (power >= 3)
                {
                    length = (ModEntry.config.sBool) ? ModEntry.config.sLength : 5;
                    radius = (ModEntry.config.sBool) ? ModEntry.config.sRadius : 0;
                }

                //Set gold values, modded or vanilla
                if (power >= 4)
                {
                    length = (ModEntry.config.gBool) ? ModEntry.config.gLength : 3;
                    radius = (ModEntry.config.gBool) ? ModEntry.config.gRadius : 1;
                }

                //Set iridium values, modded or vanilla
                if (power >= 5)
                {
                    length = (ModEntry.config.iBool) ? ModEntry.config.iLength : 6;
                    radius = (ModEntry.config.iBool) ? ModEntry.config.iRadius : 1;
                }

                for (int x = 0; x < length; x++)
                {
                    __result.Add(direction * x + tileLocation);
                    for (int y = 1; y <= radius; y++)
                    {
                        __result.Add((direction * x) + (orthogonal * y) + tileLocation);
                        __result.Add((direction * x) + (orthogonal * -y) + tileLocation);
                    }
                }
            }
        }
    }
}
