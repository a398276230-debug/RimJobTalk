using HarmonyLib;
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

        public RimJobTalkMod(ModContentPack content) : base(content)
        {
            // Don't patch here - it's too early!
            // Harmony patches are applied in HarmonyBootstrap using StaticConstructorOnStartup
            HarmonyInstance = new Harmony("ruaji.rimjobtalk");
            Log.Message("[RimJobTalk] Mod loaded. Waiting for game initialization to apply patches...");
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