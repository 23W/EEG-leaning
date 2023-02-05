using EEGCore.Processing;
using EEGCore.Processing.Filtering;
using EEGCore.Serialization;
using EEGCore.Utilities;

namespace EEGCore.Data
{
    public class RecordFactoryOptions
    {
        public bool ZeroMean { get; set; } = false;

        public bool SortLeads { get; set; } = false;

        public double? CutOffLowFreq { get; set; } = null;

        public double? CutOffHighFreq { get; set; } = null;
    }

    public class RecordFactory
    {
        public Record FromFile(string filename, RecordFactoryOptions? options = default)
        {
            var res = default(Record);

            var deserializer = default(IDeserializer);

            var ext = Path.GetExtension(filename);
            switch (ext)
            {
                case ArffSerializer.ArffExtension:
                    {
                        deserializer = new ArffSerializer();
                        break;
                    }
            }

            if (deserializer == default)
            {
                throw new Exception("Unsupported format");
            }

            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                res = deserializer.Deserialize(stream);
            }

            foreach (var lead in res.Leads)
            {
                lead.LeadType = DataUtilities.GetLeadType(lead.Name);
            }

            if (options?.ZeroMean ?? false)
            {
                foreach (var lead in res.Leads)
                {
                    lead.AddInplace(-lead.GetMean());
                }
            }

            var cutOffLow = options?.CutOffLowFreq;
            var cutOffHigh = options?.CutOffHighFreq;
            if (cutOffLow.HasValue && cutOffHigh.HasValue)
            {
                var filter = FilterFactory.BuildBandPassFilter(res.SampleRate, cutOffLow.Value, cutOffHigh.Value);
                foreach (var lead in res.Leads)
                {
                    lead.Samples = filter.Process(lead.Samples);
                }
            }
            else if (cutOffLow.HasValue)
            {
                var filter = FilterFactory.BuildHighPassFilter(res.SampleRate, cutOffLow.Value);
                foreach (var lead in res.Leads)
                {
                    lead.Samples = filter.Process(lead.Samples);
                }
            }
            else if (cutOffHigh.HasValue)
            {
                var filter = FilterFactory.BuildLowPassFilter(res.SampleRate, cutOffHigh.Value);
                foreach (var lead in res.Leads)
                {
                    lead.Samples = filter.Process(lead.Samples);
                }
            }

            if (options?.SortLeads ?? false)
            {
                res.Leads.Sort((l1, l2) => DataUtilities.ComparetTo(l1, l2));
            }

            return res;
        }
    }
}
