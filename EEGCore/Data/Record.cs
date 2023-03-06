using System.Text.Json.Serialization;

namespace EEGCore.Data
{
    [JsonDerivedType(typeof(ComponentLead), nameof(ComponentLead))]
    [JsonDerivedType(typeof(EEGLead), nameof(EEGLead))]
    public class Lead
    {
        public string Name { get; set; } = string.Empty;

        public double[] Samples { get; set; } = new double[0];
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
}