using EEGCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEGCleaning.Model
{
    internal class RecordViewModel
    {
        internal RecordFactoryOptions RecordOptions { get; set; } = new RecordFactoryOptions()
        {
            ZeroMean = true,
            SortLeads = true,
            CutOffLowFreq = 0.3,
            CutOffHighFreq = 45,
        };

        internal Record Record { get; set; } = new Record();

        internal double ScaleX { get; set; } = 1;

        internal double ScaleY { get; set; } = 1;
    }
}
