using Accord.Diagnostics;
using EEGCore.Data;

namespace EEGCore.Processing.ICA
{
    public static class ICAExtension
    {
        public static ICARecord Solve(this FastICA ica, Record input, RecordRange? range = default(RecordRange), int? numOfComponents = default)
        {
            Debug.Assert(range == default || range.Duration > 0);

            int leadsCount = input.LeadsCount;

            double[][] data = new double[leadsCount][];
            for (var leadIndex = 0; leadIndex < leadsCount; leadIndex++)
            {
                data[leadIndex] = input.Leads[leadIndex].Samples
                                                        .Skip(range?.From ?? 0)
                                                        .Take(range?.Duration ?? input.Duration)
                                                        .ToArray();
            }

            var icaResult = ica.Solve(data, numOfComponents);

            var res = new ICARecord()
            {
                Name = input.Name,
                SampleRate = input.SampleRate,
                A = icaResult.A,
                W = icaResult.W,
                XTypes = input.Leads.Select(l => l.LeadType).ToArray(),
                XNames = input.Leads.Select(l => l.Name).ToArray(),
                Leads = icaResult.Sources.Select((sourceSamples, sourceIndex) => new Lead()
                {
                    Name = $"IC{sourceIndex + 1}",
                    LeadType = LeadType.IndependenctComponent,
                    Samples = sourceSamples
                }).ToList()
            };

            return res;
        }
    }
}
