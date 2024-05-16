using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;
using StardewModdingAPI;
using StardewValley;
using StardewModdingAPI.Events;
using GenericModConfigMenu;

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
                this.Monitor.Log("Config Menu is somehow null. Fix it!");
                return;
            }

            // register mod
            configMenu.Register
            (
                mod: this.ModManifest,
                reset: () => config = new ModConfig(),
                save: () => this.Helper.WriteConfig(config)
            );

            configMenu.SetTitleScreenOnlyForNextOptions
            (
                mod: this.ModManifest,
                titleScreenOnly: false
            );

            configMenu.AddSectionTitle
            (
                mod: this.ModManifest, 
                text: () => "Iridium Tools "
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
        }
    }

    class PatchedFarmingTilesAffected
    {
        static public void Postfix(ref List<Vector2> __result, Vector2 tileLocation, int power, Farmer who)
        {
            if (power == 5)
            {
                __result.Clear();
                Vector2 direction;
                Vector2 orthogonal;

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

                int length = ModEntry.config.iLength;
                int radius = ModEntry.config.iRadius;

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
