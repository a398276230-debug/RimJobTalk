using HarmonyLib;
using RimJobTalk.UI;
using UnityEngine;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// RimJobTalk mod entry point.
    /// Patches RJW to trigger RimTalk conversations during sex.
    /// </summary>
    public class RimJobTalkMod : Mod
    {
        public static Harmony HarmonyInstance { get; private set; }
        public static RimJobTalkSettings Settings { get; private set; }

        public RimJobTalkMod(ModContentPack content) : base(content)
        {
            // Load settings
            Settings = GetSettings<RimJobTalkSettings>();
            
            // Initialize Harmony
            HarmonyInstance = new Harmony("ruaji.rimjobtalk");
            Log.Message("[RimJobTalk] Mod loaded. Waiting for game initialization to apply patches...");
        }

        /// <summary>
        /// Mod settings button label in the mod list
        /// </summary>
        public override string SettingsCategory() => "RimJobTalk";

        /// <summary>
        /// Draw the settings window
        /// </summary>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            RimJobTalkSettingsUI.DoSettingsWindowContents(inRect, Settings);
        }
    }

    /// <summary>
    /// Bootstrap class that applies Harmony patches after all defs are loaded.
    /// StaticConstructorOnStartup ensures this runs after DefDatabase is populated.
    /// </summary>
    [StaticConstructorOnStartup]
    public static class HarmonyBootstrap
    {
        static HarmonyBootstrap()
        {
            // Now it's safe to patch - all defs are loaded
            RimJobTalkMod.HarmonyInstance?.PatchAll();
            Log.Message("[RimJobTalk] Harmony patches applied successfully.");
        }
    }
}