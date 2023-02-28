using Accord.Diagnostics;
using ArffTools;
using EEGCore.Data;
using System.Text;

namespace EEGCore.Serialization
{
    internal class ArffSerializer : IRecordSerializer, IRecordDeserializer
    {
        internal const string ArffExtension = ".arff";

        Data.Record IRecordDeserializer.Deserialize(Stream stream, Encoding? encoding)
        {
            var res = new Data.Record();

            using (var arffReader = encoding == default ? new ArffReader(stream) : new ArffReader(stream, encoding))
            {
                // initialize record
                var header = arffReader.ReadHeader();
                res.Name = header.RelationName;

                var leadIndices = new List<int>();
                var leadData = new List<List<double>>();

                var rangeIndices = new Dictionary<string, int>();
                var rangeData = new Dictionary<string, List<bool>>();

                // read all frames to data lists
                {
                    var attrubuteIndex = 0;
                    var leadCount = 0;
                    foreach (var attribute in header.Attributes)
                    {
                        if (attribute.Type is ArffNumericAttribute)
                        {
                            res.Leads.Add(new Data.Lead() { Name = attribute.Name });

                            leadIndices.Add(attrubuteIndex);
                            leadData.Add(new List<double>());

                            leadCount++;
                        }
                        else if (attribute.Type is ArffNominalAttribute nominalAttribute &&
                                 nominalAttribute.Values.Count() == 2)
                        {
                            rangeIndices.Add(attribute.Name, attrubuteIndex);
                            rangeData.Add(attribute.Name, new List<bool>());
                        }

                        attrubuteIndex++;
                    }

                    if (leadCount == 0)
                    {
                        throw new Exception("Leads data not found");
                    }

                    object[] frame;
                    while ((frame = arffReader.ReadInstance()) != default)
                    {
                        foreach (var index in leadIndices)
                        {
                            leadData[index].Add((double)frame[index]);
                        }

                        foreach (var (name, index) in rangeIndices)
                        {
                            var value = (int)frame[index];
                            rangeData[name].Add(value != 0);
                        }
                    }
                }

                // move data to record
                var leadIndex = 0;
                foreach (var data in leadData)
                {
                    res.Leads[leadIndex].Samples = data.ToArray();
                    data.Clear();

                    leadIndex++;
                }

                // create ranges
                foreach(var (rangeName, rangeMarks) in rangeData)
                {
                    var index = 0;
                    do
                    {
                        var gap = rangeMarks.Skip(index)
                                            .TakeWhile(m => !m).Count();
                        index += gap;

                        if (index < res.Duration)
                        {
                            var duration = rangeMarks.Skip(index)
                                                     .TakeWhile(m => m).Count();
                            Debug.Assert(duration > 0);

                            var newRange = new RecordRange()
                            {
                                Name = rangeName,
                                From = index,
                                Duration = duration
                            };
                            res.Ranges.Add(newRange);

                            index += duration;
                        }
                    }
                    while (index < res.Duration);
                }
            }

            return res;
        }

        void IRecordSerializer.Serialize(Data.Record record, Stream stream, Encoding? encoding)
        {
            using (var arffWriter = encoding == default ? new ArffWriter(stream) : new ArffWriter(stream, encoding))
            {
                arffWriter.WriteRelationName(record.Name);

                foreach(var lead in record.Leads)
                {
                    arffWriter.WriteAttribute(new ArffAttribute(lead.Name, ArffAttributeType.Numeric));
                }

                for (var frameIndex = 0; frameIndex<record.Duration; frameIndex++) 
                {
                    var frame = new object[record.LeadsCount];
                    for(var leadIndex = 0; leadIndex<record.LeadsCount; leadIndex++)
                    {
                        frame[leadIndex] = record.Leads[leadIndex].Samples[frameIndex];
                    }

                    arffWriter.WriteInstance(frame);
                }
            }
        }
    }
}
