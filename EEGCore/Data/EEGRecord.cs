namespace EEGCore.Data
{
    public enum LeadType
    {
        Unknown,
        Frontal,
        Central,
        Temporal,
        Parietal,
        Occipital,
    }

    public class EEGLead : Lead
    {
        public LeadType LeadType { get; set; } = LeadType.Unknown;
    }
}
