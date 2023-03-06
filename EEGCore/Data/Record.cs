using System.Text.Json.Serialization;

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

    public enum ComponentType
    {
        Unknown,
        EyeArtifact,
    }

    [JsonDerivedType(typeof(ComponentLead), nameof(ComponentLead))]
    [JsonDerivedType(typeof(EEGLead), nameof(EEGLead))]
    public class Lead
    {
        public string Name { get; set; } = string.Empty;

        public double[] Samples { get; set; } = new double[0];
    }

    public class EEGLead : Lead
    {
        public LeadType LeadType { get; set; } = LeadType.Unknown;
    }

    public class ComponentLead : Lead
    {
        public ComponentType ComponentType { get; set; } = ComponentType.Unknown;

        public bool Suppress { get; set; } = false;

        [JsonIgnore]
        public bool IsArtifact => ComponentType != ComponentType.Unknown;
    }

    public class RecordRange
    {
        public string Name { get; set; } = string.Empty;

        public int From { get; set; } = 0;

        public int Duration { get; set; } = 0;
    }

    public class Record
    {
        public string Name { get; set; } = string.Empty;

        public List<Lead> Leads { get; set; } = new List<Lead>();

        public double SampleRate { get; set; } = 128;

        [JsonIgnore]
        public int LeadsCount => Leads.Count();

        [JsonIgnore]
        public int Duration => Leads.FirstOrDefault()?.Samples.Length ?? 0;

        public List<RecordRange> Ranges { get; set; } = new List<RecordRange>();
    }

    public class ICARecord : Record
    {
        // Mixing matrix
        public double[][] A { get; set; } = new double[0][];

        // Demixing matrix
        public double[][] W { get; set; } = new double[0][];

        public Record? X { get; set; } = default(Record);
    }
}