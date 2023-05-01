using System.Diagnostics;
using System.Text.Json.Serialization;

namespace EEGCore.Data
{
    [JsonDerivedType(typeof(ComponentLead), nameof(ComponentLead))]
    [JsonDerivedType(typeof(EEGLead), nameof(EEGLead))]
    public class Lead
    {
        public string Name { get; set; } = string.Empty;

        public double[] Samples { get; set; } = Array.Empty<double>();
    }

    public class RecordRange
    {
        public string Name { get; set; } = string.Empty;

        public int From { get; set; } = 0;

        public int Duration { get; set; } = 0;

        protected int To => From + Duration;

        public bool Contains(RecordRange recordRange)
        {
            var contains = From <= recordRange.From && 
                           To >= recordRange.To;
            return contains;
        }

        public bool HasIntersection(RecordRange recordRange)
        {
            var from = Math.Min(From, recordRange.From);
            var to = Math.Max(To, recordRange.To);

            var hasIntersection = (to - from) < (Duration + recordRange.Duration);
            return hasIntersection;
        }

        public RecordRange Intersection(RecordRange recordRange)
        {
            Debug.Assert(HasIntersection(recordRange));

            var from = Math.Max(From, recordRange.From);
            var to = Math.Min(To, recordRange.To);

            var union = new RecordRange() { Name = Name, From = from, Duration = to - from };
            return union;
        }

        public RecordRange Union(RecordRange recordRange)
        {
            var from = Math.Min(From, recordRange.From);
            var to = Math.Max(To, recordRange.To);

            var union = new RecordRange() { Name = Name, From = from, Duration = to - from };
            return union;
        }
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