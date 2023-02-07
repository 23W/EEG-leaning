using System.Text;

namespace EEGCore.Serialization
{
    internal interface IRecordSerializer
    {
        internal void Serialize(Data.Record record, Stream stream, Encoding? encoding = default);
    }

    internal interface IRecordDeserializer
    {
        internal Data.Record Deserialize(Stream stream, Encoding? encoding = default);
    }
}
