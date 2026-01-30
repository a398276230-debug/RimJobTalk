using HarmonyLib;
using rjw;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Harmony patch to hook into RJW's sex initiation.
    /// Triggers RimTalk conversation when sex starts.
    /// </summary>
    [HarmonyPatch(typeof(JobDriver_SexBaseInitiator), "Start")]
    public static class Patch_SexStart
    {
        /// <summary>
        /// Postfix patch that triggers after JobDriver_SexBaseInitiator.Start() completes.
        /// </summary>
        public static void Postfix(JobDriver_SexBaseInitiator __instance)
        {
            // Safety check - ensure we have valid sex properties
            if (__instance?.Sexprops == null)
            {
                return;
            }

            Pawn initiator = __instance.pawn;
            Pawn partner = __instance.Partner;
            SexProps sexProps = __instance.Sexprops;

            // Trigger the conversation
            SexTalkUtility.OnSexStart(initiator, partner, sexProps);
        }
    }
}