using System;
using rjw;
using RimTalk.Data;
using RimTalk.Source.Data;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Core utility class for triggering RimTalk conversations during RJW sex acts.
    /// </summary>
    public static class SexTalkUtility
    {
        /// <summary>
        /// Called when a sex act begins. Determines if and how to trigger a RimTalk conversation.
        /// Defers the actual talk generation to avoid blocking the current frame.
        /// </summary>
        /// <param name="initiator">The pawn initiating the sex act</param>
        /// <param name="partner">The partner pawn (may be null for masturbation, or animal for bestiality)</param>
        /// <param name="sexProps">The sex properties containing type and context information</param>
        public static void OnSexStart(Pawn initiator, Pawn partner, SexProps sexProps)
        {
            // Always log when this is called (not just in DevMode) to debug issues
            Log.Message($"[RimJobTalk] OnSexStart called - Initiator: {initiator?.Name?.ToStringShort ?? "null"}, Partner: {partner?.Name?.ToStringShort ?? "null"}, SexType: {sexProps?.sexType.ToString() ?? "null"}");
            
            if (initiator == null || sexProps == null)
            {
                Log.Message("[RimJobTalk] OnSexStart - Aborted: initiator or sexProps is null");
                return;
            }

            // Set the RJW sex context for template variable access
            // This enables {{ sex_type }}, {{ sex_is_rape }}, etc. in templates
            RJWVariableRegistration.SetSexContext(sexProps);

            // Check if at least one party is human
            bool initiatorIsHuman = xxx.is_human(initiator);
            bool partnerIsHuman = partner != null && xxx.is_human(partner);

            // Skip if neither party is human (animal x animal)
            if (!initiatorIsHuman && !partnerIsHuman)
            {
                if (Prefs.DevMode)
                {
                    Log.Message("[RimJobTalk] Skipping - neither party is human");
                }
                return;
            }

            // Skip masturbation - no dialogue needed
            if (sexProps.sexType == xxx.rjwSextype.Masturbation && partner == null)
            {
                if (Prefs.DevMode)
                {
                    Log.Message("[RimJobTalk] Skipping - masturbation");
                }
                return;
            }

            // Determine speaker and target based on who is human
            Pawn speaker;
            Pawn target;
            bool isBestiality = !initiatorIsHuman || !partnerIsHuman;

            if (initiatorIsHuman && partnerIsHuman)
            {
                // Both human - normal dialogue mode
                speaker = initiator;
                target = partner;
            }
            else
            {
                // Bestiality - only human speaks (monologue mode)
                speaker = initiatorIsHuman ? initiator : partner;
                target = null; // Monologue - no recipient
            }

            // Generate the prompt based on context (this is lightweight)
            string prompt = PromptGenerator.Generate(
                speaker,
                target,
                sexProps,
                isBestiality,
                isBestiality ? (initiatorIsHuman ? partner : initiator) : null // animal pawn for bestiality
            );

            if (string.IsNullOrEmpty(prompt))
            {
                Log.Message("[RimJobTalk] OnSexStart - Aborted: prompt is empty");
                return;
            }
            
            Log.Message($"[RimJobTalk] OnSexStart - Prompt generated, length: {prompt.Length}");

            // Submit the talk request directly (synchronously)
            // RimTalk's TalkService.GenerateTalk() handles the async AI call internally via Task.Run()
            TrySubmitTalkRequest(speaker, target, prompt, sexProps.sexType);
        }

        /// <summary>
        /// Submits the talk request to RimTalk by adding directly to pawn's request queue.
        /// This bypasses the CanGenerateTalk() check in TalkService.GenerateTalk().
        /// </summary>
        private static void TrySubmitTalkRequest(Pawn speaker, Pawn target, string prompt, xxx.rjwSextype sexType)
        {
            Log.Message($"[RimJobTalk] TrySubmitTalkRequest called - Speaker: {speaker?.Name?.ToStringShort}, Target: {target?.Name?.ToStringShort}");
            
            try
            {
                // Get the pawn's state from RimTalk's cache
                Log.Message($"[RimJobTalk] Attempting to get pawnState from Cache for {speaker?.Name?.ToStringShort}");
                PawnState speakerState = Cache.Get(speaker);
                PawnState targetState = target != null ? Cache.Get(target) : null;
                
                if (speakerState == null)
                {
                    Log.Warning($"[RimJobTalk] Speaker {speaker?.Name?.ToStringShort} not in RimTalk cache! This pawn won't trigger dialogue.");
                    return;
                }
                
                Log.Message($"[RimJobTalk] PawnState found. TalkRequests count: {speakerState.TalkRequests.Count}, TalkResponses count: {speakerState.TalkResponses.Count}");
                Log.Message($"[RimJobTalk] IsGeneratingTalk: {speakerState.IsGeneratingTalk}, CanDisplayTalk: {speakerState.CanDisplayTalk()}, CanGenerateTalk: {speakerState.CanGenerateTalk()}");

                // Clear any pending talk responses to make way for our dialogue
                speakerState.IgnoreAllTalkResponses();
                targetState?.IgnoreAllTalkResponses();
                
                // Clear any pending requests from the speaker
                while (speakerState.TalkRequests.Count > 0)
                {
                    speakerState.TalkRequests.RemoveFirst();
                }
                
                Log.Message($"[RimJobTalk] Cleared pending responses and requests");

                // Use User type to bypass CanGenerateTalk() check
                // Our Patch_ContextBuilder will intercept and remove the "do not generate" restriction
                // Add to queue - RimTalk will process it when AI is available
                speakerState.AddTalkRequest(prompt, target, TalkType.User);
                
                Log.Message($"[RimJobTalk] Request added to queue. TalkRequests count: {speakerState.TalkRequests.Count}");
                
                // Also add to UserRequestPool for priority processing
                UserRequestPool.Add(speaker, priority: true);
                
                Log.Message($"[RimJobTalk] Added to UserRequestPool with priority");
                Log.Message($"[RimJobTalk] Speaker: {speaker?.Name?.ToStringShort}, Target: {target?.Name?.ToStringShort}, SexType: {sexType}");
                Log.Message($"[RimJobTalk] PROMPT CONTENT:\n{prompt}");
            }
            catch (Exception ex)
            {
                Log.Error($"[RimJobTalk] Failed to submit talk request: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}