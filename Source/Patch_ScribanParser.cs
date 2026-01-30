using System;
using RimJobTalk.Data;
using RimTalk.API;
using rjw;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Core integration between RJW and RimTalk's Scriban template system.
    /// 
    /// This provides access to RJW data in templates via:
    /// 
    /// 1. Individual pawn variables (use {{ pawn.xxx }} in templates):
    ///    - rjw_orientation: Sexual orientation (Heterosexual, Homosexual, etc.)
    ///    - rjw_is_virgin, rjw_is_nympho, rjw_is_rapist, rjw_is_masochist
    ///    - rjw_sex_need, rjw_sex_drive, rjw_is_frustrated, rjw_is_horny, rjw_is_satisfied
    ///    - rjw_has_penis, rjw_has_vagina, rjw_has_breasts, rjw_has_anus
    ///    - rjw_can_fuck, rjw_can_be_fucked, rjw_can_masturbate
    ///    - rjw_is_whore, rjw_is_pregnant, rjw_is_prude
    /// 
    /// 2. Sex context variables (use {{ xxx }} in templates):
    ///    - sex_type, sex_type_description
    ///    - sex_is_rape, sex_is_whoring, sex_is_loving
    ///    - sex_initiator_name, sex_recipient_name
    ///    - sex_orgasms, sex_is_vaginal, sex_is_anal, sex_is_oral
    /// 
    /// Example template usage:
    ///   {{ if pawn.rjw_is_nympho }}{{ pawn.Name }} is feeling extra needy today.{{ end }}
    ///   The {{ sex_type_description }} continues as {{ sex_initiator_name }} takes control.
    /// </summary>
    public static class RJWScribanIntegration
    {
        private static bool _initialized = false;

        /// <summary>
        /// Initialize RJW integration with RimTalk's Scriban system.
        /// Called automatically via RJWVariableRegistration's static constructor.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;

            try
            {
                // Register individual pawn variables
                RegisterPawnVariables();
                
                // Register sex context variables
                RegisterSexContextVariables();

                _initialized = true;
                Log.Message("[RimJobTalk] RJW Scriban integration initialized - registered all template variables.");
            }
            catch (Exception ex)
            {
                Log.Error($"[RimJobTalk] Failed to initialize RJW Scriban integration: {ex}");
            }
        }

        /// <summary>
        /// Register pawn variables with RimTalk API.
        /// These allow {{ pawn.rjw_xxx }} syntax in templates.
        /// </summary>
        private static void RegisterPawnVariables()
        {
            // Sexual orientation
            RegisterPawnVar("rjw_orientation", p => p.GetCompRJW()?.orientation.ToString() ?? "Unknown");
            
            // Sexual traits/preferences
            RegisterPawnVar("rjw_is_virgin", p => xxx.is_Virgin(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_nympho", p => xxx.is_nympho(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_rapist", p => xxx.is_rapist(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_masochist", p => xxx.is_masochist(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_zoophile", p => xxx.is_zoophile(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_necrophiliac", p => xxx.is_necrophiliac(p).ToString().ToLower());
            
            // Sex need/drive status
            RegisterPawnVar("rjw_sex_need", p => xxx.need_sex(p).ToString());
            RegisterPawnVar("rjw_sex_drive", p => xxx.get_sex_drive(p).ToString("F2"));
            RegisterPawnVar("rjw_is_frustrated", p => xxx.is_frustrated(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_horny", p => xxx.is_horny(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_satisfied", p => xxx.is_satisfied(p).ToString().ToLower());
            
            // Genital status
            RegisterPawnVar("rjw_has_penis", p => Genital_Helper.has_male_bits(p).ToString().ToLower());
            RegisterPawnVar("rjw_has_vagina", p => Genital_Helper.has_vagina(p).ToString().ToLower());
            RegisterPawnVar("rjw_has_breasts", p => Genital_Helper.has_breasts(p).ToString().ToLower());
            RegisterPawnVar("rjw_has_anus", p => Genital_Helper.has_anus(p).ToString().ToLower());
            RegisterPawnVar("rjw_has_genitals", p => Genital_Helper.has_genitals(p).ToString().ToLower());
            
            // Sex capabilities
            RegisterPawnVar("rjw_can_fuck", p => xxx.can_fuck(p).ToString().ToLower());
            RegisterPawnVar("rjw_can_be_fucked", p => xxx.can_be_fucked(p).ToString().ToLower());
            RegisterPawnVar("rjw_can_masturbate", p => xxx.can_masturbate(p).ToString().ToLower());
            
            // Other status
            RegisterPawnVar("rjw_is_whore", p => xxx.is_whore(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_pregnant", p => p.IsPregnant().ToString().ToLower());
            RegisterPawnVar("rjw_is_prude", p => xxx.is_prude(p).ToString().ToLower());
            RegisterPawnVar("rjw_is_lecher", p => xxx.is_lecher(p).ToString().ToLower());
        }

        private static void RegisterPawnVar(string name, Func<Pawn, string> provider)
        {
            try
            {
                RimTalkPromptAPI.RegisterPawnVariable("RimJobTalk", name, provider);
            }
            catch (Exception ex)
            {
                Log.Warning($"[RimJobTalk] Failed to register pawn variable '{name}': {ex.Message}");
            }
        }

        /// <summary>
        /// Register sex context variables with RimTalk API.
        /// These allow {{ sex_xxx }} syntax in templates.
        /// </summary>
        private static void RegisterSexContextVariables()
        {
            // Sex type information
            RegisterContextVar("sex_type", () => RJWVariableRegistration.CurrentSexData?.Type ?? "None");
            RegisterContextVar("sex_type_description", () => RJWVariableRegistration.CurrentSexData?.TypeDescription ?? "no sexual activity");
            
            // Sex context flags
            RegisterContextVar("sex_is_rape", () => (RJWVariableRegistration.CurrentSexData?.IsRape ?? false).ToString().ToLower());
            RegisterContextVar("sex_is_whoring", () => (RJWVariableRegistration.CurrentSexData?.IsWhoring ?? false).ToString().ToLower());
            RegisterContextVar("sex_is_loving", () => (RJWVariableRegistration.CurrentSexData?.IsCoreLovin ?? false).ToString().ToLower());
            RegisterContextVar("sex_used_condom", () => (RJWVariableRegistration.CurrentSexData?.UsedCondom ?? false).ToString().ToLower());
            
            // Sex participants
            RegisterContextVar("sex_initiator_name", () => RJWVariableRegistration.CurrentSexData?.InitiatorName ?? "Unknown");
            RegisterContextVar("sex_recipient_name", () => RJWVariableRegistration.CurrentSexData?.RecipientName ?? "Unknown");
            
            // Sex statistics
            RegisterContextVar("sex_orgasms", () => (RJWVariableRegistration.CurrentSexData?.Orgasms ?? 0).ToString());
            
            // Sex type booleans for easy conditionals
            RegisterContextVar("sex_is_vaginal", () => (RJWVariableRegistration.CurrentSexData?.IsVaginal ?? false).ToString().ToLower());
            RegisterContextVar("sex_is_anal", () => (RJWVariableRegistration.CurrentSexData?.IsAnal ?? false).ToString().ToLower());
            RegisterContextVar("sex_is_oral", () => (RJWVariableRegistration.CurrentSexData?.IsOral ?? false).ToString().ToLower());
            RegisterContextVar("sex_is_masturbation", () => (RJWVariableRegistration.CurrentSexData?.IsMasturbation ?? false).ToString().ToLower());
        }

        private static void RegisterContextVar(string name, Func<string> provider)
        {
            try
            {
                RimTalkPromptAPI.RegisterContextVariable("RimJobTalk", name, ctx => provider());
            }
            catch (Exception ex)
            {
                Log.Warning($"[RimJobTalk] Failed to register context variable '{name}': {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Static utility class with RJW helper functions.
    /// These can be used programmatically to access RJW data.
    /// </summary>
    public static class RJWTemplateUtil
    {
        /// <summary>
        /// Get RJW data wrapper for a pawn
        /// </summary>
        public static RJWPawnData GetRJW(Pawn pawn) => pawn != null ? new RJWPawnData(pawn) : null;

        /// <summary>
        /// Get current sex event data
        /// </summary>
        public static RJWSexData GetSex() => RJWVariableRegistration.CurrentSexData;

        /// <summary>
        /// Check if pawn is a nympho
        /// </summary>
        public static bool IsNympho(Pawn pawn) => pawn != null && xxx.is_nympho(pawn);

        /// <summary>
        /// Check if pawn is a virgin
        /// </summary>
        public static bool IsVirgin(Pawn pawn) => pawn != null && xxx.is_Virgin(pawn);

        /// <summary>
        /// Get pawn's sexual orientation as string
        /// </summary>
        public static string GetOrientation(Pawn pawn) => pawn?.GetCompRJW()?.orientation.ToString() ?? "Unknown";

        /// <summary>
        /// Get pawn's sex need status as string
        /// </summary>
        public static string GetSexNeed(Pawn pawn) => pawn != null ? xxx.need_sex(pawn).ToString() : "Unknown";

        /// <summary>
        /// Check if current sex event is rape
        /// </summary>
        public static bool IsSexRape() => RJWVariableRegistration.CurrentSexData?.IsRape ?? false;

        /// <summary>
        /// Get current sex type as string
        /// </summary>
        public static string GetSexType() => RJWVariableRegistration.CurrentSexData?.Type ?? "None";
    }
}