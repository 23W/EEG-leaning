using Accord.Diagnostics;
using Accord.Statistics.Distributions.Univariate;
using EEGCore.Data;

namespace EEGCore.Processing.ICA
{
    public static class ICAExtension
    {
        public static ICARecord Decompose(this FastICA ica, Record mixture, RecordRange? range = default(RecordRange), int? numOfComponents = default)
        {
            Debug.Assert(mixture.LeadsCount > 1);
            Debug.Assert(range == default || range.Duration > 0);

            int leadsCount = mixture.LeadsCount;

            double[][] data = new double[leadsCount][];
            for (var leadIndex = 0; leadIndex < leadsCount; leadIndex++)
            {
                data[leadIndex] = mixture.Leads[leadIndex].Samples
                                                        .Skip(range?.From ?? 0)
                                                        .Take(range?.Duration ?? mixture.Duration)
                                                        .ToArray();
            }

            var icaResult = ica.Decompose(data, numOfComponents);

            var res = new ICARecord()
            {
                Name = mixture.Name,
                SampleRate = mixture.SampleRate,
                A = icaResult.A,
                W = icaResult.W,
                X = mixture,
                Leads = icaResult.Sources.Select((sourceSamples, sourceIndex) => new ComponentLead()
                {
                    Name = $"IC{sourceIndex + 1}",
                    ComponentType = ComponentType.Unknown,
                    Suppress = false,
                    Samples = sourceSamples
                }).Cast<Lead>().ToList()
            };

            return res;
        }

        public static Record Compose(this FastICA ica, ICARecord sources)
        {
            Debug.Assert(sources.LeadsCount > 1);

            var mixture = ica.Compose(sources.A, sources.GetLeadMatrix());
            var res = new Record()
            {
                Name = sources.Name,
                SampleRate = sources.SampleRate,
                Leads = mixture.Select((leadSamples, leadIndex) => new Lead()
                {
                    Name = $"X{leadIndex + 1}",
                    Samples = leadSamples,
                }).ToList()
            };

            if ((sources.X != default) &&
                (sources.X!.LeadsCount == res.LeadsCount))
            {
                for (var leadIndex = 0; leadIndex < res.LeadsCount; leadIndex++)
                {
                    var copy = sources.X.Leads[leadIndex].Clone();
                    copy.Samples = res.Leads[leadIndex].Samples;
                    res.Leads[leadIndex] = copy;
                }
            }

            return res;
        }
    }
}
