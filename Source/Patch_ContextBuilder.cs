using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using RimTalk.Data;
using RimTalk.Service;
using RimTalk.Source.Data;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Harmony patch for ContextBuilder.BuildDialogueType.
    /// When detecting a RimJobTalk request (User type with our signature),
    /// skip the original method and use our own prompt format without "do not generate" restriction.
    /// </summary>
    [HarmonyPatch(typeof(ContextBuilder), nameof(ContextBuilder.BuildDialogueType))]
    public static class Patch_ContextBuilder_BuildDialogueType
    {
        public static bool Prefix(StringBuilder sb, TalkRequest talkRequest, List<Pawn> pawns, string shortName, Pawn mainPawn)
        {
            // Only intercept User type
            if (!talkRequest.TalkType.IsFromUser())
                return true;

            // Check if this is a RimJobTalk request
            string prompt = talkRequest.Prompt ?? "";
            if (!IsRimJobTalkPrompt(prompt))
                return true;

            // This is our request - build our own dialogue type without "do not generate"
            // Just use multi-turn format
            sb.Append($"{shortName} starts conversation, taking turns");
            sb.Append($"\n{prompt}");

            // Skip original method
            return false;
        }

        private static bool IsRimJobTalkPrompt(string prompt)
        {
            return prompt.Contains("getting intimate") ||
                   prompt.Contains("passionate dialogue") ||
                   prompt.Contains("Include whispers, moans");
        }
    }
}