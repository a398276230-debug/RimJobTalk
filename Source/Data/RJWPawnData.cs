using rjw;
using Verse;

namespace RimJobTalk.Data
{
    /// <summary>
    /// Wrapper class for RJW pawn-related properties.
    /// Provides easy access to RJW data through Scriban templates.
    /// Usage in template: {{ pawn.rjw.orientation }}, {{ pawn.rjw.is_virgin }}
    /// </summary>
    public class RJWPawnData
    {
        private readonly Pawn _pawn;
        private readonly CompRJW _compRJW;

        public RJWPawnData(Pawn pawn)
        {
            _pawn = pawn;
            _compRJW = pawn?.GetCompRJW();
        }

        /// <summary>
        /// Check if this pawn has valid RJW data
        /// </summary>
        public bool IsValid => _pawn != null && _compRJW != null;

        // ===== 性取向相关 =====

        /// <summary>
        /// Sexual orientation (Heterosexual, Homosexual, Bisexual, etc.)
        /// </summary>
        public string Orientation => _compRJW?.orientation.ToString() ?? "Unknown";

        /// <summary>
        /// Is the pawn asexual
        /// </summary>
        public bool IsAsexual => _pawn != null && xxx.is_asexual(_pawn);

        /// <summary>
        /// Is the pawn bisexual
        /// </summary>
        public bool IsBisexual => _pawn != null && xxx.is_bisexual(_pawn);

        /// <summary>
        /// Is the pawn homosexual
        /// </summary>
        public bool IsHomosexual => _pawn != null && xxx.is_homosexual(_pawn);

        /// <summary>
        /// Is the pawn heterosexual
        /// </summary>
        public bool IsHeterosexual => _pawn != null && xxx.is_heterosexual(_pawn);

        /// <summary>
        /// Is the pawn pansexual
        /// </summary>
        public bool IsPansexual => _pawn != null && xxx.is_pansexual(_pawn);

        // ===== 性癖好/特质相关 =====

        /// <summary>
        /// Is the pawn a nymphomaniac (sex addict)
        /// </summary>
        public bool IsNympho => _pawn != null && xxx.is_nympho(_pawn);

        /// <summary>
        /// Is the pawn a rapist
        /// </summary>
        public bool IsRapist => _pawn != null && xxx.is_rapist(_pawn);

        /// <summary>
        /// Is the pawn a masochist
        /// </summary>
        public bool IsMasochist => _pawn != null && xxx.is_masochist(_pawn);

        /// <summary>
        /// Is the pawn a zoophile
        /// </summary>
        public bool IsZoophile => _pawn != null && xxx.is_zoophile(_pawn);

        /// <summary>
        /// Is the pawn a necrophiliac
        /// </summary>
        public bool IsNecrophiliac => _pawn != null && xxx.is_necrophiliac(_pawn);

        /// <summary>
        /// Is the pawn a virgin
        /// </summary>
        public bool IsVirgin => _pawn != null && xxx.is_Virgin(_pawn);

        // ===== 性需求相关 =====

        /// <summary>
        /// Current sex need status (Frustrated, Horny, Neutral, Satisfied)
        /// </summary>
        public string SexNeed => _pawn != null ? xxx.need_sex(_pawn).ToString() : "Unknown";

        /// <summary>
        /// Is the pawn frustrated (sexually)
        /// </summary>
        public bool IsFrustrated => _pawn != null && xxx.is_frustrated(_pawn);

        /// <summary>
        /// Is the pawn horny
        /// </summary>
        public bool IsHorny => _pawn != null && xxx.is_horny(_pawn);

        /// <summary>
        /// Is the pawn sexually satisfied
        /// </summary>
        public bool IsSatisfied => _pawn != null && xxx.is_satisfied(_pawn);

        /// <summary>
        /// Sex drive level (0-1+)
        /// </summary>
        public float SexDrive => _pawn != null ? xxx.get_sex_drive(_pawn) : 0f;

        /// <summary>
        /// Sex satisfaction level
        /// </summary>
        public float SexSatisfaction => _pawn != null ? xxx.get_sex_satisfaction(_pawn) : 0f;

        /// <summary>
        /// Vulnerability level (how likely to be raped)
        /// </summary>
        public float Vulnerability => _pawn != null ? xxx.get_vulnerability(_pawn) : 0f;

        // ===== 生殖器相关 =====

        /// <summary>
        /// Does the pawn have a penis (includes male ovipositor)
        /// </summary>
        public bool HasPenis => _pawn != null && Genital_Helper.has_male_bits(_pawn);

        /// <summary>
        /// Does the pawn have a vagina
        /// </summary>
        public bool HasVagina => _pawn != null && Genital_Helper.has_vagina(_pawn);

        /// <summary>
        /// Does the pawn have breasts
        /// </summary>
        public bool HasBreasts => _pawn != null && Genital_Helper.has_breasts(_pawn);

        /// <summary>
        /// Does the pawn have an anus
        /// </summary>
        public bool HasAnus => _pawn != null && Genital_Helper.has_anus(_pawn);

        /// <summary>
        /// Does the pawn have any genitals
        /// </summary>
        public bool HasGenitals => _pawn != null && Genital_Helper.has_genitals(_pawn);

        // ===== 性能力相关 =====

        /// <summary>
        /// Can the pawn penetrate (has functioning penis/ovipositor)
        /// </summary>
        public bool CanFuck => _pawn != null && xxx.can_fuck(_pawn);

        /// <summary>
        /// Can the pawn be penetrated (has functioning orifice)
        /// </summary>
        public bool CanBeFucked => _pawn != null && xxx.can_be_fucked(_pawn);

        /// <summary>
        /// Can the pawn masturbate
        /// </summary>
        public bool CanMasturbate => _pawn != null && xxx.can_masturbate(_pawn);

        /// <summary>
        /// Can the pawn rape others
        /// </summary>
        public bool CanRape => _pawn != null && xxx.can_rape(_pawn);

        /// <summary>
        /// Can the pawn be raped
        /// </summary>
        public bool CanGetRaped => _pawn != null && xxx.can_get_raped(_pawn);

        /// <summary>
        /// Can the pawn have loving sex
        /// </summary>
        public bool CanDoLoving => _pawn != null && xxx.can_do_loving(_pawn);

        // ===== 其他状态 =====

        /// <summary>
        /// Is the pawn a whore (sex worker)
        /// </summary>
        public bool IsWhore => _pawn != null && xxx.is_whore(_pawn);

        /// <summary>
        /// Is the pawn a slave
        /// </summary>
        public bool IsSlave => _pawn != null && xxx.is_slave(_pawn);

        /// <summary>
        /// Is the pawn a prude
        /// </summary>
        public bool IsPrude => _pawn != null && xxx.is_prude(_pawn);

        /// <summary>
        /// Is the pawn a lecher
        /// </summary>
        public bool IsLecher => _pawn != null && xxx.is_lecher(_pawn);

        /// <summary>
        /// Is the pawn being raped right now
        /// </summary>
        public bool IsBeingRaped => _pawn != null && xxx.is_gettin_rapedNow(_pawn);

        /// <summary>
        /// Is the pawn pregnant
        /// </summary>
        public bool IsPregnant => _pawn != null && _pawn.IsPregnant();

        /// <summary>
        /// Is the pawn visibly pregnant
        /// </summary>
        public bool IsVisiblyPregnant => _pawn != null && _pawn.IsVisiblyPregnant();

        /// <summary>
        /// Pawn's name for convenience
        /// </summary>
        public string Name => _pawn?.LabelShort ?? "Unknown";

        /// <summary>
        /// Pawn's full name
        /// </summary>
        public string FullName => _pawn?.Name?.ToStringFull ?? "Unknown";

        /// <summary>
        /// Pawn's gender
        /// </summary>
        public string Gender => _pawn?.gender.ToString() ?? "Unknown";

        /// <summary>
        /// Is the pawn female
        /// </summary>
        public bool IsFemale => _pawn != null && xxx.is_female(_pawn);

        /// <summary>
        /// Is the pawn male
        /// </summary>
        public bool IsMale => _pawn != null && xxx.is_male(_pawn);

        /// <summary>
        /// Get the underlying Pawn object
        /// </summary>
        public Pawn Pawn => _pawn;

        /// <summary>
        /// Static factory method to get RJW data for a pawn
        /// </summary>
        public static RJWPawnData GetFor(Pawn pawn)
        {
            return pawn != null ? new RJWPawnData(pawn) : null;
        }
    }
}