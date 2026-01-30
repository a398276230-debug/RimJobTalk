using rjw;
using Verse;

namespace RimJobTalk.Data
{
    /// <summary>
    /// Wrapper class for RJW sex event properties.
    /// Provides easy access to SexProps data through Scriban templates.
    /// Usage in template: {{ sex.type }}, {{ sex.is_rape }}, {{ sex.initiator.Name }}
    /// </summary>
    public class RJWSexData
    {
        private readonly SexProps _sexProps;
        private RJWPawnData _initiatorData;
        private RJWPawnData _recipientData;

        public RJWSexData(SexProps sexProps)
        {
            _sexProps = sexProps;
        }

        /// <summary>
        /// Check if this sex data is valid
        /// </summary>
        public bool IsValid => _sexProps != null;

        // ===== 基本信息 =====

        /// <summary>
        /// Sex type (Vaginal, Anal, Oral, Masturbation, etc.)
        /// </summary>
        public string Type => _sexProps?.sexType.ToString() ?? "Unknown";

        /// <summary>
        /// The raw sex type enum value
        /// </summary>
        public xxx.rjwSextype SexType => _sexProps?.sexType ?? xxx.rjwSextype.None;

        /// <summary>
        /// Is this sex act a rape
        /// </summary>
        public bool IsRape => _sexProps?.isRape ?? false;

        /// <summary>
        /// Is the pawn a rapist in this act
        /// </summary>
        public bool IsRapist => _sexProps?.isRapist ?? false;

        /// <summary>
        /// Is this a whoring/prostitution act
        /// </summary>
        public bool IsWhoring => _sexProps?.isWhoring ?? false;

        /// <summary>
        /// Was a condom used
        /// </summary>
        public bool UsedCondom => _sexProps?.usedCondom ?? false;

        /// <summary>
        /// Is this vanilla loving (not RJW specific)
        /// </summary>
        public bool IsCoreLovin => _sexProps?.isCoreLovin ?? false;

        /// <summary>
        /// Is this a mech implant act
        /// </summary>
        public bool IsMechImplant => _sexProps?.isMechImplant ?? false;

        /// <summary>
        /// Number of orgasms so far
        /// </summary>
        public int Orgasms => _sexProps?.orgasms ?? 0;

        // ===== 参与者信息 =====

        /// <summary>
        /// The pawn initiating the sex act
        /// </summary>
        public Pawn Initiator => _sexProps?.initiator;

        /// <summary>
        /// The pawn receiving the sex act
        /// </summary>
        public Pawn Recipient => _sexProps?.recipient;

        /// <summary>
        /// The main pawn (from SexProps.pawn)
        /// </summary>
        public Pawn Pawn => _sexProps?.pawn;

        /// <summary>
        /// The partner pawn
        /// </summary>
        public Pawn Partner => _sexProps?.partner;

        /// <summary>
        /// RJW data for the initiator
        /// </summary>
        public RJWPawnData InitiatorRJW
        {
            get
            {
                if (_initiatorData == null && Initiator != null)
                    _initiatorData = new RJWPawnData(Initiator);
                return _initiatorData;
            }
        }

        /// <summary>
        /// RJW data for the recipient
        /// </summary>
        public RJWPawnData RecipientRJW
        {
            get
            {
                if (_recipientData == null && Recipient != null)
                    _recipientData = new RJWPawnData(Recipient);
                return _recipientData;
            }
        }

        /// <summary>
        /// Initiator's name for convenience
        /// </summary>
        public string InitiatorName => Initiator?.LabelShort ?? "Unknown";

        /// <summary>
        /// Recipient's name for convenience
        /// </summary>
        public string RecipientName => Recipient?.LabelShort ?? "Unknown";

        /// <summary>
        /// Does this act have a partner
        /// </summary>
        public bool HasPartner => _sexProps?.hasPartner() ?? false;

        // ===== 角色信息 =====

        /// <summary>
        /// Is the main pawn the receiver in this act
        /// </summary>
        public bool IsReceiver => _sexProps?.isReceiver ?? false;

        /// <summary>
        /// Is this a reversed role act
        /// </summary>
        public bool IsReverse => _sexProps?.isRevese ?? false;

        /// <summary>
        /// Is the main pawn the initiator
        /// </summary>
        public bool IsInitiator => _sexProps?.IsInitiator() ?? false;

        /// <summary>
        /// Is the main pawn submissive in this act
        /// </summary>
        public bool IsSubmissive => _sexProps?.IsSubmissive() ?? false;

        // ===== 性行为类型判断 =====

        /// <summary>
        /// Is this vaginal sex
        /// </summary>
        public bool IsVaginal => SexType == xxx.rjwSextype.Vaginal;

        /// <summary>
        /// Is this anal sex
        /// </summary>
        public bool IsAnal => SexType == xxx.rjwSextype.Anal;

        /// <summary>
        /// Is this oral sex
        /// </summary>
        public bool IsOral => SexType == xxx.rjwSextype.Oral;

        /// <summary>
        /// Is this masturbation
        /// </summary>
        public bool IsMasturbation => SexType == xxx.rjwSextype.Masturbation;

        /// <summary>
        /// Is this double penetration
        /// </summary>
        public bool IsDoublePenetration => SexType == xxx.rjwSextype.DoublePenetration;

        /// <summary>
        /// Is this a boobjob
        /// </summary>
        public bool IsBoobjob => SexType == xxx.rjwSextype.Boobjob;

        /// <summary>
        /// Is this a handjob
        /// </summary>
        public bool IsHandjob => SexType == xxx.rjwSextype.Handjob;

        /// <summary>
        /// Is this a footjob
        /// </summary>
        public bool IsFootjob => SexType == xxx.rjwSextype.Footjob;

        /// <summary>
        /// Is this fingering
        /// </summary>
        public bool IsFingering => SexType == xxx.rjwSextype.Fingering;

        /// <summary>
        /// Is this scissoring
        /// </summary>
        public bool IsScissoring => SexType == xxx.rjwSextype.Scissoring;

        /// <summary>
        /// Is this mutual masturbation
        /// </summary>
        public bool IsMutualMasturbation => SexType == xxx.rjwSextype.MutualMasturbation;

        /// <summary>
        /// Is this fisting
        /// </summary>
        public bool IsFisting => SexType == xxx.rjwSextype.Fisting;

        /// <summary>
        /// Is this rimming (analingus)
        /// </summary>
        public bool IsRimming => SexType == xxx.rjwSextype.Rimming;

        /// <summary>
        /// Is this fellatio (blowjob)
        /// </summary>
        public bool IsFellatio => SexType == xxx.rjwSextype.Fellatio;

        /// <summary>
        /// Is this cunnilingus
        /// </summary>
        public bool IsCunnilingus => SexType == xxx.rjwSextype.Cunnilingus;

        /// <summary>
        /// Is this 69 position
        /// </summary>
        public bool IsSixtynine => SexType == xxx.rjwSextype.Sixtynine;

        // ===== 描述性信息 =====

        /// <summary>
        /// Get a human-readable description of the sex type
        /// </summary>
        public string TypeDescription
        {
            get
            {
                return SexType switch
                {
                    xxx.rjwSextype.Vaginal => "vaginal intercourse",
                    xxx.rjwSextype.Anal => "anal intercourse",
                    xxx.rjwSextype.Oral => "oral sex",
                    xxx.rjwSextype.Masturbation => "masturbation",
                    xxx.rjwSextype.DoublePenetration => "double penetration",
                    xxx.rjwSextype.Boobjob => "boobjob",
                    xxx.rjwSextype.Handjob => "handjob",
                    xxx.rjwSextype.Footjob => "footjob",
                    xxx.rjwSextype.Fingering => "fingering",
                    xxx.rjwSextype.Scissoring => "scissoring",
                    xxx.rjwSextype.MutualMasturbation => "mutual masturbation",
                    xxx.rjwSextype.Fisting => "fisting",
                    xxx.rjwSextype.Rimming => "rimming",
                    xxx.rjwSextype.Fellatio => "fellatio",
                    xxx.rjwSextype.Cunnilingus => "cunnilingus",
                    xxx.rjwSextype.Sixtynine => "69 position",
                    _ => "sexual activity"
                };
            }
        }

        /// <summary>
        /// Get the underlying SexProps object
        /// </summary>
        public SexProps SexProps => _sexProps;

        /// <summary>
        /// Static factory method to create RJWSexData from SexProps
        /// </summary>
        public static RJWSexData FromSexProps(SexProps props)
        {
            return props != null ? new RJWSexData(props) : null;
        }
    }
}