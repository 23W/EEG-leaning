using EEGCore.Processing;
using EEGCore.Processing.Filtering;
using EEGCore.Serialization;
using EEGCore.Utilities;

namespace EEGCore.Data
{

    public class RecordFactoryOptions
    {
        public enum AcceptableLeadTypes
        {
            AnyEEG,
            StrictEEG,
            Any
        }

        public bool ZeroMean { get; set; } = false;

        public bool SortLeads { get; set; } = false;

        public AcceptableLeadTypes LeadTypes { get; set; } = AcceptableLeadTypes.Any;

        public double? CutOffLowFreq { get; set; } = null;

        public double? CutOffHighFreq { get; set; } = null;

        public static RecordFactoryOptions DefaultEEG => new RecordFactoryOptions()
        {
            ZeroMean = true,
            SortLeads = true,
            LeadTypes = AcceptableLeadTypes.StrictEEG,
            CutOffLowFreq = 0.3,
            CutOffHighFreq = 45,
        };

        public static RecordFactoryOptions DefaultEEGNoFilter => new RecordFactoryOptions()
        {
            ZeroMean = true,
            SortLeads = true,
            LeadTypes = AcceptableLeadTypes.StrictEEG,
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
                case EDFSerializer.EDFExtension:
                    {
                        deserializer = new EDFSerializer();
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
                        Name = DataUtilities.CleanEEGLeadName(res.Leads[leadIndex].Name),
                        Samples = res.Leads[leadIndex].Samples,
                        LeadType = eegType,
                    };

                    res.Leads[leadIndex] = eegLead;
                }
            }

            switch (options?.LeadTypes)
            {
                case RecordFactoryOptions.AcceptableLeadTypes.AnyEEG:
                    res.Leads = res.Leads.Where(l => l is EEGLead).ToList();
                    break;

                case RecordFactoryOptions.AcceptableLeadTypes.StrictEEG:
                    res.Leads = res.Leads.Where(l => (l is EEGLead) && DataUtilities.GetEEGLeadCodeByName(l.Name) != default).ToList();
                    break;
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
                res.Leads.Sort(DataUtilities.ComparetTo);
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
