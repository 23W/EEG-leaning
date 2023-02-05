using System.Text;

namespace EEGCore.Serialization
{
    internal interface ISerializer
    {
        internal void Serialize(Data.Record record, Stream stream, Encoding? encoding = default);
    }

    internal interface IDeserializer
    {
        internal Data.Record Deserialize(Stream stream, Encoding? encoding = default);
    }
}
