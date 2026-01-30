using System.Linq;
using rjw;
using Verse;

namespace RimJobTalk
{
    /// <summary>
    /// Generates contextual prompts for RimTalk based on RJW sex act properties.
    /// </summary>
    public static class PromptGenerator
    {
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
        /// <returns>A prompt string for RimTalk</returns>
        public static string Generate(Pawn speaker, Pawn target, SexProps sexProps, bool isBestiality, Pawn animalPawn = null)
        {
            if (speaker == null || sexProps == null)
            {
                return null;
            }

            string speakerName = speaker.Name?.ToStringShort ?? "Someone";
            string sexTypeDesc = GetSexTypeDescription(sexProps.sexType, sexProps);
            
            // Build context modifiers including tail info
            string contextModifier = GetContextModifier(sexProps, speaker, target);

            if (isBestiality && animalPawn != null)
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
            string basePrompt = $"{speakerName} and {targetName} are getting intimate. They are {sexType}.";
            
            // Add tail-related flavor if applicable
            string tailFlavor = "";
            if (HasTail(speaker))
            {
                string tailDesc = GetTailDescription(speaker);
                tailFlavor += $" {speakerName} has a {tailDesc} that sways with their movements.";
            }
            if (HasTail(target))
            {
                string tailDesc = GetTailDescription(target);
                tailFlavor += $" {targetName} has a {tailDesc} that curls with pleasure.";
            }
            
            if (props.isRape)
            {
                basePrompt = $"{speakerName} is forcing themselves on {targetName}. They are {sexType}.{tailFlavor}";
                return $"{basePrompt} Generate a short, tense dialogue. The aggressor may speak dominantly while the victim may protest, plead, or be silent with shock. Keep it dramatic but not gratuitously violent.";
            }
            else if (props.isWhoring)
            {
                basePrompt = $"{speakerName} is having paid sex with {targetName}. They are {sexType}.{tailFlavor}";
                return $"{basePrompt} Generate a short dialogue. It can be transactional, professional, or unexpectedly intimate. Include some whispers or expressions of physical sensation.";
            }
            else
            {
                return $"{basePrompt}{tailFlavor} {context}Generate a short, passionate dialogue between them. Include whispers, moans, or expressions of love and pleasure. Make it intimate and romantic.";
            }
        }

        private static string GenerateBestialityPrompt(string speaker, string animalDesc, string sexType, string context, SexProps props)
        {
            string basePrompt = $"{speaker} is being intimate with {animalDesc}. They are {sexType}.";
            
            return $"{basePrompt} {context}Generate {speaker}'s inner thoughts or whispered words during this act. Express their feelings - whether pleasure, excitement, guilt, taboo thrill, or primal satisfaction. This is a monologue, the animal cannot respond with words.";
        }

        private static string GenerateSoloPrompt(string speaker, string sexType, string context, SexProps props)
        {
            return $"{speaker} is pleasuring themselves ({sexType}). {context}Generate their inner thoughts or soft moans. Express their fantasies, desires, or the sensations they're experiencing.";
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
                modifier += "This is a loving, romantic encounter between partners. ";
            }
            
            // Add tailjob context if applicable
            if (IsTailjob(props))
            {
                Pawn tailOwner = HasTail(speaker) ? speaker : (target != null && HasTail(target) ? target : null);
                if (tailOwner != null)
                {
                    string tailDesc = GetTailDescription(tailOwner);
                    modifier += $"They are using {tailOwner.Name?.ToStringShort ?? "their"}'s {tailDesc} for intimate pleasure. ";
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
    }
}