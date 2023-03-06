using EEGCore.Data;
using EEGCore.Processing.ICA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEGCleaning.Model
{
    internal enum ModelViewMode
    {
        Record,
        ICA
    };

    internal class RecordViewModel
    {
        internal ModelViewMode ViewMode { get; set; } = ModelViewMode.Record;

        internal RecordFactoryOptions RecordOptions { get; set; } = RecordFactoryOptions.DefaultEEG;

        internal Record SourceRecord { get; set; } = new Record();

        internal Record ProcessedRecord { get; set; } = new Record();

        internal ICARecord IndependentComponents { get; set; } = new ICARecord();

        internal double ScaleX { get; set; } = 1;

        internal double ScaleY { get; set; } = 1;
    }
}
