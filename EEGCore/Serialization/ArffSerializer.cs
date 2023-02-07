using ArffTools;
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

                // read all frames to data lists
                {
                    var attrubuteIndex = 0;
                    foreach (var attribute in header.Attributes)
                    {
                        if (attribute.Type is ArffNumericAttribute)
                        {
                            res.Leads.Add(new Data.Lead() { Name = attribute.Name });

                            leadIndices.Add(attrubuteIndex);
                            leadData.Add(new List<double>());
                            attrubuteIndex++;
                        }
                    }

                    if (attrubuteIndex == 0)
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
