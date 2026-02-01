using System.Collections.Generic;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Mod settings for RimJobTalk.
    /// Stores customizable prompt templates that players can edit.
    /// </summary>
    public class RimJobTalkSettings : ModSettings
    {
        // ===== General Settings =====
        
        /// <summary>
        /// Cooldown time in seconds between sex talk triggers.
        /// 0 = disabled (always trigger dialogue)
        /// </summary>
        public int SexTalkCooldownSeconds = DefaultSexTalkCooldownSeconds;
        public const int DefaultSexTalkCooldownSeconds = 30;
        
        // ===== Prompt Templates =====
        
        /// <summary>
        /// Template for normal romantic/loving sex between two humans.
        /// Variables: {speaker}, {target}, {sexType}, {tailFlavor}
        /// </summary>
        public string PromptNormal = DefaultPromptNormal;
        
        /// <summary>
        /// Template for rape scenarios.
        /// Variables: {speaker}, {target}, {sexType}, {tailFlavor}
        /// </summary>
        public string PromptRape = DefaultPromptRape;
        
        /// <summary>
        /// Template for prostitution/whoring scenarios.
        /// Variables: {speaker}, {target}, {sexType}, {tailFlavor}
        /// </summary>
        public string PromptWhoring = DefaultPromptWhoring;
        
        /// <summary>
        /// Template for bestiality scenarios (human monologue).
        /// Variables: {speaker}, {animal}, {sexType}, {context}
        /// </summary>
        public string PromptBestiality = DefaultPromptBestiality;
        
        /// <summary>
        /// Template for masturbation/solo scenarios.
        /// Variables: {speaker}, {sexType}, {context}
        /// </summary>
        public string PromptSolo = DefaultPromptSolo;

        /// <summary>
        /// Template for necrophilia scenarios (human with corpse).
        /// Variables: {speaker}, {corpse}, {sexType}, {context}
        /// </summary>
        public string PromptNecrophilia = DefaultPromptNecrophilia;

        // ===== Default Templates =====
        
        public const string DefaultPromptNormal = 
            "{speaker} and {target} are getting intimate. They are {sexType}.{tailFlavor} " +
            "Generate a short, passionate dialogue between them. Include whispers, moans, or expressions of love and pleasure. Make it intimate and romantic.";
        
        public const string DefaultPromptRape = 
            "{speaker} is forcing themselves on {target}. They are {sexType}.{tailFlavor} " +
            "Generate a short, tense dialogue. The aggressor may speak dominantly while the victim may protest, plead, or be silent with shock. Keep it dramatic but not gratuitously violent.";
        
        public const string DefaultPromptWhoring = 
            "{speaker} is having paid sex with {target}. They are {sexType}.{tailFlavor} " +
            "Generate a short dialogue. It can be transactional, professional, or unexpectedly intimate. Include some whispers or expressions of physical sensation.";
        
        public const string DefaultPromptBestiality = 
            "{speaker} is being intimate with {animal}. They are {sexType}. {context}" +
            "Generate {speaker}'s inner thoughts or whispered words during this act. Express their feelings - whether pleasure, excitement, guilt, taboo thrill, or primal satisfaction. This is a monologue, the animal cannot respond with words.";
        
        public const string DefaultPromptSolo =
            "{speaker} is pleasuring themselves ({sexType}). {context}" +
            "Generate their inner thoughts or soft moans. Express their fantasies, desires, or the sensations they're experiencing.";

        public const string DefaultPromptNecrophilia =
            "{speaker} is violating the corpse of {corpse}. They are {sexType}. {context}" +
            "Generate {speaker}'s inner thoughts or whispered words during this dark act. " +
            "Express their feelings - whether morbid fascination, guilt, forbidden pleasure, or detached coldness. " +
            "This is a monologue, the corpse cannot respond.";

        // ===== Context Modifiers =====
        
        /// <summary>
        /// Additional context added when it's loving/romantic sex.
        /// </summary>
        public string ContextLoving = DefaultContextLoving;
        
        /// <summary>
        /// Context template for tailjob scenarios.
        /// Variables: {owner}, {tailDesc}
        /// </summary>
        public string ContextTailjob = DefaultContextTailjob;

        public const string DefaultContextLoving = "This is a loving, romantic encounter between partners. ";
        public const string DefaultContextTailjob = "They are using {owner}'s {tailDesc} for intimate pleasure. ";

        // ===== Tail Flavor Templates =====
        
        /// <summary>
        /// Template for speaker's tail description.
        /// Variables: {name}, {tailDesc}
        /// </summary>
        public string TailFlavorSpeaker = DefaultTailFlavorSpeaker;
        
        /// <summary>
        /// Template for target's tail description.
        /// Variables: {name}, {tailDesc}
        /// </summary>
        public string TailFlavorTarget = DefaultTailFlavorTarget;

        public const string DefaultTailFlavorSpeaker = " {name} has a {tailDesc} that sways with their movements.";
        public const string DefaultTailFlavorTarget = " {name} has a {tailDesc} that curls with pleasure.";

        /// <summary>
        /// Expose settings to save/load
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            
            // General settings
            Scribe_Values.Look(ref SexTalkCooldownSeconds, "SexTalkCooldownSeconds", DefaultSexTalkCooldownSeconds);
            
            // Prompt templates
            Scribe_Values.Look(ref PromptNormal, "PromptNormal", DefaultPromptNormal);
            Scribe_Values.Look(ref PromptRape, "PromptRape", DefaultPromptRape);
            Scribe_Values.Look(ref PromptWhoring, "PromptWhoring", DefaultPromptWhoring);
            Scribe_Values.Look(ref PromptBestiality, "PromptBestiality", DefaultPromptBestiality);
            Scribe_Values.Look(ref PromptSolo, "PromptSolo", DefaultPromptSolo);
            Scribe_Values.Look(ref PromptNecrophilia, "PromptNecrophilia", DefaultPromptNecrophilia);
            
            // Context modifiers
            Scribe_Values.Look(ref ContextLoving, "ContextLoving", DefaultContextLoving);
            Scribe_Values.Look(ref ContextTailjob, "ContextTailjob", DefaultContextTailjob);
            
            // Tail flavor
            Scribe_Values.Look(ref TailFlavorSpeaker, "TailFlavorSpeaker", DefaultTailFlavorSpeaker);
            Scribe_Values.Look(ref TailFlavorTarget, "TailFlavorTarget", DefaultTailFlavorTarget);
        }

        /// <summary>
        /// Reset all settings to defaults
        /// </summary>
        public void ResetToDefaults()
        {
            // General settings
            SexTalkCooldownSeconds = DefaultSexTalkCooldownSeconds;
            
            // Prompt templates
            PromptNormal = DefaultPromptNormal;
            PromptRape = DefaultPromptRape;
            PromptWhoring = DefaultPromptWhoring;
            PromptBestiality = DefaultPromptBestiality;
            PromptSolo = DefaultPromptSolo;
            PromptNecrophilia = DefaultPromptNecrophilia;
            ContextLoving = DefaultContextLoving;
            ContextTailjob = DefaultContextTailjob;
            TailFlavorSpeaker = DefaultTailFlavorSpeaker;
            TailFlavorTarget = DefaultTailFlavorTarget;
        }

        /// <summary>
        /// Get available variables documentation for UI display
        /// </summary>
        public static Dictionary<string, string> GetVariableHelp()
        {
            return new Dictionary<string, string>
            {
                { "{speaker}", "Name of the speaking pawn" },
                { "{target}", "Name of the target pawn" },
                { "{sexType}", "Description of the sex type (e.g., 'having vaginal sex')" },
                { "{tailFlavor}", "Tail description text (if applicable)" },
                { "{animal}", "Description of the animal (e.g., 'a male horse')" },
                { "{corpse}", "Name of the corpse" },
                { "{context}", "Additional context modifiers" },
                { "{owner}", "Name of the tail owner" },
                { "{tailDesc}", "Description of the tail (e.g., 'fluffy fox tail')" },
                { "{name}", "Pawn's name" }
            };
        }

        /// <summary>
        /// Get Scriban variable documentation for UI display
        /// </summary>
        public static Dictionary<string, string> GetScribanVariableHelp()
        {
            return new Dictionary<string, string>
            {
                // Sex event variables
                { "{{ sex_type }}", "Sex type (Vaginal, Anal, etc.)" },
                { "{{ sex_type_description }}", "Human-readable sex description" },
                { "{{ sex_is_rape }}", "Is this rape ('true'/'false')" },
                { "{{ sex_is_whoring }}", "Is this prostitution ('true'/'false')" },
                { "{{ sex_is_loving }}", "Is this loving sex ('true'/'false')" },
                { "{{ sex_initiator_name }}", "Initiator's name" },
                { "{{ sex_recipient_name }}", "Recipient's name" },
                { "{{ sex_is_vaginal }}", "Is vaginal sex ('true'/'false')" },
                { "{{ sex_is_anal }}", "Is anal sex ('true'/'false')" },
                { "{{ sex_is_oral }}", "Is oral sex ('true'/'false')" },
                { "{{ sex_is_necrophilia }}", "Is necrophilia ('true'/'false')" },
                
                // Pawn variables
                { "{{ pawn.rjw_orientation }}", "Sexual orientation" },
                { "{{ pawn.rjw_is_virgin }}", "Is virgin ('true'/'false')" },
                { "{{ pawn.rjw_is_nympho }}", "Is nymphomaniac ('true'/'false')" },
                { "{{ pawn.rjw_has_penis }}", "Has penis ('true'/'false')" },
                { "{{ pawn.rjw_has_vagina }}", "Has vagina ('true'/'false')" },
                { "{{ pawn.rjw_is_horny }}", "Is horny ('true'/'false')" }
            };
        }
    }
}