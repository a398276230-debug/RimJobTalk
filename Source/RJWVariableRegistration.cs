using System;
using RimJobTalk.Data;
using rjw;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Main entry point for RJW integration with RimTalk's Scriban template system.
    /// 
    /// This module registers RJW variables with RimTalk, enabling templates to use:
    /// 
    /// Pawn variables ({{ pawn.xxx }}):
    ///   - rjw_orientation: Sexual orientation (Heterosexual, Homosexual, etc.)
    ///   - rjw_is_virgin: Whether the pawn is a virgin
    ///   - rjw_is_nympho: Whether the pawn has nymphomaniac trait
    ///   - rjw_is_rapist: Whether the pawn has rapist trait
    ///   - rjw_is_masochist: Whether the pawn has masochist trait
    ///   - rjw_sex_need: Current sex need status (Frustrated, Horny, Neutral, Satisfied)
    ///   - rjw_has_penis: Whether the pawn has a penis
    ///   - rjw_has_vagina: Whether the pawn has a vagina
    ///   - rjw_can_fuck: Whether the pawn can penetrate
    ///   - rjw_can_be_fucked: Whether the pawn can be penetrated
    ///   ... and more
    /// 
    /// Sex context variables ({{ sex_xxx }}):
    ///   - sex_type: Type of sex act (Vaginal, Anal, Oral, etc.)
    ///   - sex_type_description: Human-readable description
    ///   - sex_is_rape: Whether this is rape
    ///   - sex_is_whoring: Whether this is prostitution
    ///   - sex_is_necrophilia: Whether this is necrophilia (sex with corpse)
    ///   - sex_corpse_name: Name of the corpse (if necrophilia)
    ///   - sex_initiator_is_necrophiliac: Whether initiator has necrophiliac trait
    ///   - sex_initiator_name: Name of the initiator
    ///   - sex_recipient_name: Name of the recipient
    ///   ... and more
    /// </summary>
    [StaticConstructorOnStartup]
    public static class RJWVariableRegistration
    {
        // Current sex context - set by SexTalkUtility when sex starts
        private static RJWSexData _currentSexData;
        private static SexProps _currentSexProps;

        /// <summary>
        /// Gets the current sex data wrapper for template access
        /// </summary>
        public static RJWSexData CurrentSexData => _currentSexData;

        /// <summary>
        /// Gets the current raw SexProps
        /// </summary>
        public static SexProps CurrentSexProps => _currentSexProps;

        /// <summary>
        /// Sets the current sex context from SexProps.
        /// Call this before triggering RimTalk dialogue during sex.
        /// </summary>
        public static void SetSexContext(SexProps props)
        {
            _currentSexProps = props;
            _currentSexData = props != null ? new RJWSexData(props) : null;
            
            if (Prefs.DevMode && props != null)
            {
                Log.Message($"[RimJobTalk] Sex context set: Type={_currentSexData.Type}, IsRape={_currentSexData.IsRape}");
            }
        }

        /// <summary>
        /// Clears the current sex context.
        /// Call this when sex ends.
        /// </summary>
        public static void ClearSexContext()
        {
            _currentSexProps = null;
            _currentSexData = null;
        }

        /// <summary>
        /// Static constructor - initializes RJW integration with RimTalk
        /// </summary>
        static RJWVariableRegistration()
        {
            try
            {
                // Initialize the Scriban integration (registers all variables)
                RJWScribanIntegration.Initialize();
                
                Log.Message("[RimJobTalk] RJW variable registration complete.");
            }
            catch (Exception ex)
            {
                Log.Error($"[RimJobTalk] Failed to initialize RJW variable registration: {ex}");
            }
        }

        /// <summary>
        /// Gets RJWPawnData wrapper for a pawn.
        /// Useful for programmatic access to RJW data.
        /// </summary>
        public static RJWPawnData GetRJW(Pawn pawn)
        {
            return pawn != null ? new RJWPawnData(pawn) : null;
        }

        /// <summary>
        /// Gets the current sex event data wrapper.
        /// </summary>
        public static RJWSexData GetSex()
        {
            return _currentSexData;
        }
    }
}