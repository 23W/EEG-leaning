using EEGCore.Data;
using EEGCore.Processing;
using EEGCore.Processing.Filtering;
using System.Diagnostics;

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

    internal class FrequencyItem
    {
        internal double Value { get; set; } = -1;

        internal bool HasValue => Value > 0;

        public override string ToString() => HasValue ? $"{Value} Hz" : "None";

        static internal FrequencyItem Default => new FrequencyItem();
    }

    internal class SpeedItem
    {
        internal double Value { get; set; } = -1;

        internal bool HasValue => Value > 0;

        public override string ToString() => HasValue ? $"{Value} mm/sec" : "Auto";

        static internal SpeedItem Default => new SpeedItem();
    }

    internal class AmplItem
    {
        internal double Value { get; set; } = -1;

        internal bool HasValue => Value > 0;
        
        public override string ToString() => HasValue ? $"{Value} mkV/сm" : "Auto";

        static internal AmplItem Default => new AmplItem();
    }

    internal class RecordViewModel
    {
        internal ModelViewMode ViewMode { get; set; } = ModelViewMode.Record;

        internal RecordFactoryOptions RecordOptions { get; set; } = RecordFactoryOptions.DefaultEEGNoFilter;

        internal Record SourceRecord { get => m_sourceRecord; set { m_sourceRecord = value; ResetVisibleRecord(); } }

        internal Record ProcessedRecord { get => m_processedRecord; set { m_processedRecord = value; ResetVisibleRecord(); } }

        internal ICARecord IndependentComponents { get => m_independentComponents; set { m_independentComponents = value; ResetVisibleRecord(); } }

        internal Record VisibleRecord => GetVisibleRecord();

        internal TimePositionItem Position { get; set; } = TimePositionItem.Default;

        internal SpeedItem Speed { get; set; } = SpeedItem.Default;

        internal AmplItem Amplitude { get; set; } = AmplItem.Default;

        internal FrequencyItem CutOffLowFrequency { get => m_cutOffLowFreq; set { m_cutOffLowFreq = value; ResetVisibleRecord(); } }

        internal FrequencyItem CutOffHighFrequency { get => m_cutOffHighFreq; set { m_cutOffHighFreq = value; ResetVisibleRecord(); } }

        #region Helper Methods

        void ResetVisibleRecord()
        {
            m_visibleRecord = default;
        }

        Record GetVisibleRecord()
        {
            if (m_visibleRecord == default)
            {
                var unfilteredRecord = (ViewMode == ModelViewMode.Record) ? ProcessedRecord : IndependentComponents;
                m_visibleRecord = unfilteredRecord;

                if (m_cutOffLowFreq.HasValue ||
                    m_cutOffHighFreq.HasValue)
                {
                    m_visibleRecord = (ViewMode == ModelViewMode.Record) ? unfilteredRecord.Clone() : ((ICARecord)unfilteredRecord).Clone();

                    if (m_cutOffLowFreq.HasValue &&
                        m_cutOffHighFreq.HasValue)
                    {
                        var filter = FilterFactory.BuildBandPassFilter(m_visibleRecord.SampleRate, m_cutOffLowFreq.Value, m_cutOffHighFreq.Value);
                        filter.ProcessInplace(m_visibleRecord.Leads);
                    }
                    else if (m_cutOffLowFreq.HasValue)
                    {
                        var filter = FilterFactory.BuildHighPassFilter(m_visibleRecord.SampleRate, m_cutOffLowFreq.Value);
                        filter.ProcessInplace(m_visibleRecord.Leads);
                    }
                    else
                    {
                        Debug.Assert(m_cutOffHighFreq.HasValue);

                        var filter = FilterFactory.BuildLowPassFilter(m_visibleRecord.SampleRate, m_cutOffHighFreq.Value);
                        filter.ProcessInplace(m_visibleRecord.Leads);
                    }
                }
            }

            return m_visibleRecord;
        }

        #endregion

        #region Members

        Record m_sourceRecord = new Record();
        Record m_processedRecord = new Record();
        ICARecord m_independentComponents = new ICARecord();

        FrequencyItem m_cutOffLowFreq = FrequencyItem.Default;
        FrequencyItem m_cutOffHighFreq = FrequencyItem.Default;

        Record? m_visibleRecord = default;

        #endregion
    }
}
