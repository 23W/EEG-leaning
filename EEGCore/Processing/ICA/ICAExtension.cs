using EEGCore.Data;

namespace EEGCore.Processing.ICA
{
    public static class ICAExtension
    {
        public static ICARecord Solve(this FastICA ica, Record input, int? numOfComponents = default)
        {
            int leadsCount = input.LeadsCount;

            double[][] data = new double[leadsCount][];
            for (var leadIndex = 0; leadIndex < leadsCount; leadIndex++)
            {
                data[leadIndex] = input.Leads[leadIndex].Samples;
            }

            var icaResult = ica.Solve(data, numOfComponents);

            var res = new ICARecord()
            {
                Name = input.Name,
                SampleRate = input.SampleRate,
            };

            res.Leads = icaResult.Sources.Select((sourceSamples, sourceIndex) => new Lead()
            {
                Name = $"IC{sourceIndex + 1}",
                LeadType = LeadType.IndependenctComponent,
                Samples = sourceSamples
            }).ToList();

            return res;
        }
    }
}
