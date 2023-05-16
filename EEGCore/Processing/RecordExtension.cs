using EEGCore.Data;
using EEGCore.Processing.Filtering;
using EEGCore.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace EEGCore.Processing
{
    public static class RecordExtension
    {
        public static T Clone<T>(this T record, RecordRange? range = default) where T : Record, new()
        {
            var clone = new T();

            if (range != default)
            {
                Debug.Assert(range.To < record.Duration);
                clone = record.Clone();
                clone.Ranges.Clear();

                foreach (var lead in clone.Leads)
                {
                    lead.Samples = lead.Samples.Skip(range.From)
                                               .Take(range.Duration)
                                               .ToArray();
                }
            }
            else
            {
                var json = ToJson(record);
                clone = JsonSerializer.Deserialize<T>(json);
            }

            return clone ?? new T();
        }

        public static string ToJson<T>(this T record) where T : Record
        {
            var json = JsonSerializer.Serialize<T>(record);
            return json;
        }

        public static double[][] GetLeadMatrix(this Record record)
        {
            var res = record.Leads.Select(lead => lead.Samples).ToArray();
            return res;
        }

        public static void SetLeadMatrix(this Record record, double[][] leadsData)
        {
            if (record.LeadsCount!= leadsData.Length)
            {
                throw new ArgumentException($"{nameof(record.LeadsCount)} and {nameof(leadsData)} must be the same size");
            }

            foreach(var (data, index) in leadsData.WithIndex())
            {
                record.Leads[index].Samples = data;
            }
        }
    }

    public static class ICARecordExtension
    {
        public static double[] GetMixingVector(this ICARecord record, int componentIndex) => GeneralUtilities.GetMatrixColumn(record.A, componentIndex);

        public static double[] GetDemixingVector(this ICARecord record, int componentIndex) => GeneralUtilities.GetMatrixColumn(record.W, componentIndex);

        public static double[] BuildLeadSuppress(this ICARecord record, int componentIndex)
        {
            var res = Array.Empty<double>();

            var component = record.Leads[componentIndex] as ComponentLead;
            if (component != default)
            {
                switch (component.Suppress)
                {
                    case SuppressType.None:
                        res = (double[])component.Samples.Clone();
                        break;

                    case SuppressType.ZeroLead:
                        res = new double[component.Samples.Length];
                        break;

                    case SuppressType.HiPass10:
                        {
                            var highPassFilter = FilterFactory.BuildHighPassFilter(record.SampleRate, 10.0);
                            res = highPassFilter.Process(component.Samples);
                        }
                        break;

                    case SuppressType.HiPass20:
                        {
                            var highPassFilter = FilterFactory.BuildHighPassFilter(record.SampleRate, 20.0);
                            res = highPassFilter.Process(component.Samples);
                        }
                        break;

                    case SuppressType.HiPass30:
                        {
                            var highPassFilter = FilterFactory.BuildHighPassFilter(record.SampleRate, 30.0);
                            res = highPassFilter.Process(component.Samples);
                        }
                        break;
                }
            }

            return res;
        }

        public static void BuildLeadAlternativeSuppress(this ICARecord record, int componentIndex)
        {
            var component = record.Leads[componentIndex] as ComponentLead;
            if (component != default)
            {
                if (component.Suppress == SuppressType.None)
                {
                    component.Alternative = Array.Empty<double>();
                }
                else
                {
                    var suppressedSamples = BuildLeadSuppress(record, componentIndex);
                    component.Alternative = suppressedSamples;
                }
            }
        }
    }
}
