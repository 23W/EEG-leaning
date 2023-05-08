using System.Text.Json.Serialization;

namespace EEGCore.Data
{
    public enum ArtifactType
    {
        EyeArtifact,
        ReferenceElectrodeArtifact,
        SingleElectrodeArtifact,
    }

    public enum SuppressType
    {
        None,
        ZeroLead,
        HiPass10,
        HiPass20,
        HiPass30,
    }

    public class ArtifactInfo
    {
        public ArtifactType ArtifactType { get; set; } = ArtifactType.EyeArtifact;

        public double Probaprobability { get; set; } = 0;
    }

    public class ComponentLead : Lead
    {
        public List<ArtifactInfo> ArtifactInfo { get; set; } = new List<ArtifactInfo>();

        public SuppressType Suppress { get; set; } = SuppressType.None;

        [JsonIgnore]
        public bool IsArtifact => ArtifactInfo.Any();

        [JsonIgnore]
        public bool IsEyeArtifact => ArtifactInfo.Any(info => info.ArtifactType == ArtifactType.EyeArtifact);

        [JsonIgnore]
        public bool IsReferenceElectrodeArtifact => ArtifactInfo.Any(info => info.ArtifactType == ArtifactType.ReferenceElectrodeArtifact);

        [JsonIgnore]
        public bool IsSingleElectrodeArtifact => ArtifactInfo.Any(info => info.ArtifactType == ArtifactType.SingleElectrodeArtifact);

        public double[] Alternative { get; set; } = Array.Empty<double>();
    }

    public class ICARecord : Record
    {
        // Mixing matrix
        public double[][] A { get; set; } = new double[0][];

        // Demixing matrix
        public double[][] W { get; set; } = new double[0][];

        // Mixture (EEG record)
        public Record? X { get; set; } = default(Record);

        public RecordRange? XRange { get; set; } = default(RecordRange);
    }
}
