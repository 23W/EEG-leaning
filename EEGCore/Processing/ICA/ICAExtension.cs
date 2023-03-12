using EEGCore.Data;
using EEGCore.Utilities;
using System.Diagnostics;

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
                XRange = range,
                Leads = icaResult.Sources.Select((sourceSamples, sourceIndex) => new ComponentLead()
                {
                    Name = $"IC{sourceIndex + 1}",
                    ComponentType = ComponentType.Unknown,
                    Suppress = SuppressType.None,
                    Samples = sourceSamples
                }).Cast<Lead>().ToList()
            };

            return res;
        }

        public static Record Compose(this FastICA ica, ICARecord sources, bool suppressComponents = false)
        {
            Debug.Assert(sources.LeadsCount > 1);

            var sourcesMatrix = sources.GetLeadMatrix();
            if (suppressComponents)
            {
                foreach (var (lead,index) in sources.Leads.WithIndex())
                {
                    if (lead is ComponentLead)
                    {
                        var suppressedSamples = sources.BuildLeadSuppress(index);
                        sourcesMatrix[index] = suppressedSamples;
                    }
                }
            }

            var res = new Record();
            var mixture = ica.Compose(sources.A, sourcesMatrix);

            if ((sources.X != default) &&
                (sources.XRange != default))
            {
                res = sources.X.Clone();
                var range = sources.XRange;

                foreach(var (mixedLead, index) in mixture.WithIndex())
                {
                    var lead = res.Leads[index];
                    Array.Copy(mixedLead, 0, lead.Samples, range.From, Math.Min(mixedLead.Length, res.Duration));
                }
            }
            else
            {
                res = new Record()
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
            }

            return res;
        }
    }
}
