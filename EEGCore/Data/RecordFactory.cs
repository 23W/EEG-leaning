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

        public static RecordFactoryOptions DefaultEEG => new RecordFactoryOptions()
        {
            ZeroMean = true,
            SortLeads = true,
            CutOffLowFreq = 0.3,
            CutOffHighFreq = 45,
        };

        public static RecordFactoryOptions DefaultEEGNoFilter => new RecordFactoryOptions()
        {
            ZeroMean = true,
            SortLeads = true,
        };

        public static RecordFactoryOptions DefaultEmpty => new RecordFactoryOptions();
    }

    public class RecordFactory
    {
        public Record FromFile(string filename, RecordFactoryOptions? options = default)
        {
            var res = default(Record);

            var deserializer = default(IRecordDeserializer);

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

            for (var leadIndex = 0; leadIndex<res.LeadsCount; leadIndex++)
            {
                var eegType = DataUtilities.GetEEGLeadTypeByName(res.Leads[leadIndex].Name);
                if (eegType != LeadType.Unknown)
                {
                    var eegLead = new EEGLead()
                    {
                        Name = res.Leads[leadIndex].Name,
                        Samples = res.Leads[leadIndex].Samples,
                        LeadType = eegType,
                    };

                    res.Leads[leadIndex] = eegLead;
                }
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
                filter.ProcessInplace(res.Leads);
            }
            else if (cutOffLow.HasValue)
            {
                var filter = FilterFactory.BuildHighPassFilter(res.SampleRate, cutOffLow.Value);
                filter.ProcessInplace(res.Leads);
            }
            else if (cutOffHigh.HasValue)
            {
                var filter = FilterFactory.BuildLowPassFilter(res.SampleRate, cutOffHigh.Value);
                filter.ProcessInplace(res.Leads);
            }

            if (options?.SortLeads ?? false)
            {
                res.Leads.Sort((l1, l2) => DataUtilities.ComparetTo(l1, l2));
            }

            return res;
        }

        public void ToFile(string filename, Record record)
        {
            var serializer = default(IRecordSerializer);

            var ext = Path.GetExtension(filename);
            switch (ext)
            {
                case ArffSerializer.ArffExtension:
                    {
                        serializer = new ArffSerializer();
                        break;
                    }
            }

            if (serializer == default)
            {
                throw new Exception("Unsupported format");
            }

            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                serializer.Serialize(record, stream);
            }
        }
    }
}
