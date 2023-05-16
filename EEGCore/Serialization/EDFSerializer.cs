using EDFCSharp;
using EEGCore.Data;
using EEGCore.Utilities;
using System.Text;

namespace EEGCore.Serialization
{
    internal class EDFSerializer : IRecordDeserializer
    {
        internal const string EDFExtension = ".edf";

        Data.Record IRecordDeserializer.Deserialize(Stream stream, Encoding? encoding)
        {
            var res = new Data.Record();

            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);

            using (var edf = new EDFFile(data))
            {
                res.Name = $"{edf.Header.PatientID.Value} - {edf.Header.RecordID.Value}";
                res.SampleRate = edf.Header.NumberOfSamplesPerRecord.Value[0] / edf.Header.RecordDurationInSeconds.Value;

                res.Leads = edf.Signals.Where(s => s.FrequencyInHZ == res.SampleRate)
                                       .Select(s => new Data.Lead()
                                       {
                                           Name = s.Label.Value,
                                           Samples = Enumerable.Range(0, s.Samples.Count).Select(index => s.ScaledSample(index)).ToArray(),
                                       })
                                       .ToList();
            }

            return res;
        }
    }
}
