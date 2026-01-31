using System.Linq;
using rjw;
using RimWorld;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Generates contextual prompts for RimTalk based on RJW sex act properties.
    /// Uses configurable templates from mod settings.
    /// </summary>
    public static class PromptGenerator
    {
        /// <summary>
        /// Gets the current mod settings
        /// </summary>
        private static RimJobTalkSettings Settings => RimJobTalkMod.Settings;

        /// <summary>
        /// Checks if a pawn has a tail body part.
        /// </summary>
        public static bool HasTail(Pawn pawn)
        {
            if (pawn?.health?.hediffSet == null)
                return false;

            // Check for any body part with "tail" in its label or def name
            return pawn.health.hediffSet.GetNotMissingParts()
                .Any(part =>
                    part.def.defName.ToLower().Contains("tail") ||
                    part.Label.ToLower().Contains("tail"));
        }

        /// <summary>
        /// Gets a description of the pawn's tail if they have one.
        /// </summary>
        public static string GetTailDescription(Pawn pawn)
        {
            if (pawn?.health?.hediffSet == null)
                return null;

            var tailPart = pawn.health.hediffSet.GetNotMissingParts()
                .FirstOrDefault(part =>
                    part.def.defName.ToLower().Contains("tail") ||
                    part.Label.ToLower().Contains("tail"));

            if (tailPart == null)
                return null;

            // Try to get a descriptive name based on race
            string raceName = pawn.def?.label?.ToLower() ?? "";
            
            if (raceName.Contains("kurin") || raceName.Contains("miho"))
                return "fluffy fox tail";
            if (raceName.Contains("ratkin") || raceName.Contains("rat"))
                return "long rat tail";
            if (raceName.Contains("kiiro") || raceName.Contains("feline"))
                return "graceful cat tail";
            if (raceName.Contains("wolfein") || raceName.Contains("canine") || raceName.Contains("wolf"))
                return "wagging tail";
            if (raceName.Contains("dragon") || raceName.Contains("reptile") || raceName.Contains("lizard"))
                return "powerful scaled tail";
            if (raceName.Contains("succubus") || raceName.Contains("demon"))
                return "seductive demon tail";
            if (raceName.Contains("rabbit") || raceName.Contains("bunny"))
                return "cute bunny tail";
                
            // Default description
            return "tail";
        }

        /// <summary>
        /// Checks if the interaction involves a tailjob based on the interaction def name.
        /// </summary>
        public static bool IsTailjob(SexProps sexProps)
        {
            if (sexProps?.dictionaryKey == null)
                return false;
                
            string defName = sexProps.dictionaryKey.defName?.ToLower() ?? "";
            return defName.Contains("tail");
        }

        /// <summary>
        /// Generates a prompt for the AI to create dialogue based on the sex context.
        /// </summary>
        /// <param name="speaker">The pawn who will speak</param>
        /// <param name="target">The target pawn for dialogue (null for monologue)</param>
        /// <param name="sexProps">The sex properties from RJW</param>
        /// <param name="isBestiality">Whether this is a human x animal scenario</param>
        /// <param name="animalPawn">The animal pawn in bestiality scenarios</param>
        /// <param name="isNecrophilia">Whether this is a necrophilia scenario (target is dead)</param>
        /// <param name="corpsePawn">The corpse pawn in necrophilia scenarios</param>
        /// <returns>A prompt string for RimTalk</returns>
        public static string Generate(Pawn speaker, Pawn target, SexProps sexProps, bool isBestiality, Pawn animalPawn = null, bool isNecrophilia = false, Pawn corpsePawn = null)
        {
            if (speaker == null || sexProps == null)
            {
                return null;
            }

            string speakerName = speaker.Name?.ToStringShort ?? "Someone";
            string sexTypeDesc = GetSexTypeDescription(sexProps.sexType, sexProps);
            
            // Build context modifiers including tail info
            string contextModifier = GetContextModifier(sexProps, speaker, target);

            // Check for necrophilia scenario first
            if (isNecrophilia && corpsePawn != null)
            {
                // Necrophilia scenario - human monologue with corpse
                string corpseDesc = GetCorpseDescription(corpsePawn, speaker);
                return GenerateNecrophiliaPrompt(speakerName, corpseDesc, sexTypeDesc, contextModifier, sexProps, speaker);
            }
            else if (isBestiality && animalPawn != null)
            {
                // Bestiality scenario - human monologue
                string animalDesc = GetAnimalDescription(animalPawn);
                return GenerateBestialityPrompt(speakerName, animalDesc, sexTypeDesc, contextModifier, sexProps);
            }
            else if (target != null)
            {
                // Normal human x human scenario
                string targetName = target.Name?.ToStringShort ?? "Someone";
                return GenerateNormalPrompt(speakerName, targetName, sexTypeDesc, contextModifier, sexProps, speaker, target);
            }
            else
            {
                // Solo/monologue scenario
                return GenerateSoloPrompt(speakerName, sexTypeDesc, contextModifier, sexProps);
            }
        }

        private static string GenerateNormalPrompt(string speakerName, string targetName, string sexType, string context, SexProps props, Pawn speaker, Pawn target)
        {
            // Build tail flavor text
            string tailFlavor = "";
            if (HasTail(speaker))
            {
                string tailDesc = GetTailDescription(speaker);
                tailFlavor += FormatTemplate(Settings.TailFlavorSpeaker, 
                    ("{name}", speakerName),
                    ("{tailDesc}", tailDesc));
            }
            if (HasTail(target))
            {
                string tailDesc = GetTailDescription(target);
                tailFlavor += FormatTemplate(Settings.TailFlavorTarget,
                    ("{name}", targetName),
                    ("{tailDesc}", tailDesc));
            }
            
            // Select template based on scenario
            string template;
            if (props.isRape)
            {
                template = Settings.PromptRape;
            }
            else if (props.isWhoring)
            {
                template = Settings.PromptWhoring;
            }
            else
            {
                template = Settings.PromptNormal;
            }
            
            // Apply template with context appended for normal prompts
            string prompt = FormatTemplate(template,
                ("{speaker}", speakerName),
                ("{target}", targetName),
                ("{sexType}", sexType),
                ("{tailFlavor}", tailFlavor));
            
            // Add context modifier for normal prompts
            if (!props.isRape && !props.isWhoring && !string.IsNullOrEmpty(context))
            {
                prompt = prompt.Replace("{context}", context);
            }
            
            return prompt;
        }

        private static string GenerateBestialityPrompt(string speaker, string animalDesc, string sexType, string context, SexProps props)
        {
            return FormatTemplate(Settings.PromptBestiality,
                ("{speaker}", speaker),
                ("{animal}", animalDesc),
                ("{sexType}", sexType),
                ("{context}", context));
        }

        private static string GenerateSoloPrompt(string speaker, string sexType, string context, SexProps props)
        {
            return FormatTemplate(Settings.PromptSolo,
                ("{speaker}", speaker),
                ("{sexType}", sexType),
                ("{context}", context));
        }

        /// <summary>
        /// Generates a necrophilia prompt for when a pawn is having sex with a corpse.
        /// </summary>
        private static string GenerateNecrophiliaPrompt(string speaker, string corpseDesc, string sexType, string context, SexProps props, Pawn speakerPawn)
        {
            // Add necrophiliac trait context if applicable
            string necroContext = context;
            if (xxx.is_necrophiliac(speakerPawn))
            {
                necroContext += "The speaker has the Necrophiliac trait and finds this arousing. ";
            }
            else
            {
                necroContext += "The speaker does not normally do this and may feel conflicted. ";
            }
            
            return FormatTemplate(Settings.PromptNecrophilia,
                ("{speaker}", speaker),
                ("{corpse}", corpseDesc),
                ("{sexType}", sexType),
                ("{context}", necroContext));
        }

        /// <summary>
        /// Gets a description of a corpse for necrophilia scenarios.
        /// </summary>
        private static string GetCorpseDescription(Pawn corpse, Pawn speaker)
        {
            if (corpse == null)
            {
                return "a corpse";
            }

            string name = corpse.Name?.ToStringShort ?? corpse.LabelShort ?? "someone";
            string genderDesc = corpse.gender switch
            {
                Gender.Male => "male",
                Gender.Female => "female",
                _ => ""
            };
            
            // Check relationship
            string relationDesc = "";
            if (speaker?.relations != null && corpse.relations != null)
            {
                var relation = speaker.relations.GetDirectRelation(PawnRelationDefOf.Lover, corpse)
                    ?? speaker.relations.GetDirectRelation(PawnRelationDefOf.Spouse, corpse)
                    ?? speaker.relations.GetDirectRelation(PawnRelationDefOf.Fiance, corpse);
                    
                if (relation != null)
                {
                    relationDesc = " (their former lover)";
                }
            }

            if (!string.IsNullOrEmpty(genderDesc))
            {
                return $"{name}, a {genderDesc} corpse{relationDesc}";
            }

            return $"{name}'s corpse{relationDesc}";
        }

        private static string GetSexTypeDescription(xxx.rjwSextype sexType, SexProps sexProps)
        {
            // Check for tailjob first (comes from interaction def, not sexType enum)
            if (IsTailjob(sexProps))
            {
                return "having a tailjob - using their tail for pleasure";
            }
            
            return sexType switch
            {
                xxx.rjwSextype.Vaginal => "having vaginal sex",
                xxx.rjwSextype.Anal => "having anal sex",
                xxx.rjwSextype.Oral => "having oral sex",
                xxx.rjwSextype.Fellatio => "performing fellatio",
                xxx.rjwSextype.Cunnilingus => "performing cunnilingus",
                xxx.rjwSextype.Sixtynine => "in a sixty-nine position",
                xxx.rjwSextype.DoublePenetration => "having double penetration",
                xxx.rjwSextype.Boobjob => "having a boobjob",
                xxx.rjwSextype.Handjob => "giving a handjob",
                xxx.rjwSextype.Footjob => "giving a footjob",
                xxx.rjwSextype.Fingering => "fingering",
                xxx.rjwSextype.Scissoring => "scissoring",
                xxx.rjwSextype.MutualMasturbation => "mutually masturbating",
                xxx.rjwSextype.Fisting => "fisting",
                xxx.rjwSextype.Rimming => "rimming",
                xxx.rjwSextype.Masturbation => "masturbating",
                xxx.rjwSextype.None => "being intimate",
                _ => "being intimate"
            };
        }

        private static string GetContextModifier(SexProps props, Pawn speaker, Pawn target)
        {
            string modifier = "";

            if (props.isCoreLovin)
            {
                modifier += Settings.ContextLoving;
            }
            
            // Add tailjob context if applicable
            if (IsTailjob(props))
            {
                Pawn tailOwner = HasTail(speaker) ? speaker : (target != null && HasTail(target) ? target : null);
                if (tailOwner != null)
                {
                    string tailDesc = GetTailDescription(tailOwner);
                    modifier += FormatTemplate(Settings.ContextTailjob,
                        ("{owner}", tailOwner.Name?.ToStringShort ?? "their"),
                        ("{tailDesc}", tailDesc));
                }
            }

            return modifier;
        }

        private static string GetAnimalDescription(Pawn animal)
        {
            if (animal == null)
            {
                return "an animal";
            }

            string kindLabel = animal.kindDef?.label ?? animal.def?.label ?? "animal";
            
            // Add gender if available
            string genderDesc = animal.gender switch
            {
                Gender.Male => "male",
                Gender.Female => "female",
                _ => ""
            };

            if (!string.IsNullOrEmpty(genderDesc))
            {
                return $"a {genderDesc} {kindLabel}";
            }

            return $"a {kindLabel}";
        }

        /// <summary>
        /// Helper method to format a template string with variable replacements.
        /// </summary>
        private static string FormatTemplate(string template, params (string key, string value)[] replacements)
        {
            if (string.IsNullOrEmpty(template))
                return "";
                
            string result = template;
            foreach (var (key, value) in replacements)
            {
                result = result.Replace(key, value ?? "");
            }
            return result;
        }
    }
}