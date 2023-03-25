using EEGCore.Data;

namespace EEGCleaning.Model
{
    internal enum ModelViewMode
    {
        Record,
        ICA
    };

    internal class TimePositionItem
    {
        internal double Value { get; set; } = 0;

        public override string ToString() => Value >= 0 ? $"{TimeSpan.FromSeconds(Value)}" : "-";

        static internal TimePositionItem Default => new TimePositionItem();
    }


    internal class SpeedItem
    {
        internal double Value { get; set; } = -1;

        public override string ToString() => Value > 0 ? $"{Value} mm/sec" : "Auto";

        static internal SpeedItem Default => new SpeedItem();
    }

    internal class AmplItem
    {
        internal double Value { get; set; } = -1;

        public override string ToString() => Value > 0 ? $"{Value} mkV/сm" : "Auto";

        static internal AmplItem Default => new AmplItem();
    }

    internal class RecordViewModel
    {
        internal ModelViewMode ViewMode { get; set; } = ModelViewMode.Record;

        internal RecordFactoryOptions RecordOptions { get; set; } = RecordFactoryOptions.DefaultEEGNoFilter;

        internal Record SourceRecord { get; set; } = new Record();

        internal Record ProcessedRecord { get; set; } = new Record();

        internal ICARecord IndependentComponents { get; set; } = new ICARecord();

        internal Record VisibleRecord => ViewMode== ModelViewMode.Record ? ProcessedRecord : IndependentComponents;

        internal TimePositionItem Position { get; set; } = TimePositionItem.Default;

        internal SpeedItem Speed { get; set; } = SpeedItem.Default;

        internal AmplItem Amplitude { get; set; } = AmplItem.Default;
    }
}
