using System.Text.Json.Serialization;

namespace EEGCore.Data
{
    public enum ComponentType
    {
        Unknown,
        EyeArtifact,
    }

    public enum SuppressType
    {
        None,
        ZeroLead,
        HiPass30,
    }

    public class ComponentLead : Lead
    {
        public ComponentType ComponentType { get; set; } = ComponentType.Unknown;

        public SuppressType Suppress { get; set; } = SuppressType.None;

        [JsonIgnore]
        public bool IsArtifact => ComponentType != ComponentType.Unknown;
    }

    public class ICARecord : Record
    {
        // Mixing matrix
        public double[][] A { get; set; } = new double[0][];

        // Demixing matrix
        public double[][] W { get; set; } = new double[0][];

        public Record? X { get; set; } = default(Record);

        public RecordRange? XRange { get; set; } = default(RecordRange);
    }
}
