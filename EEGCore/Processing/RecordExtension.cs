using EEGCore.Data;

namespace EEGCore.Processing
{
    public static class RecordExtension
    {
        public static double GetMaximumAbsoluteValue(this Record record)
        {
            var range = record.Leads.Select(l => l.GetMinimumMaximum())
                                    .Aggregate(Tuple.Create(0.0, 0.0),
                                               (r1, r2) => Tuple.Create(Math.Min(r1.Item1, r2.Item1),
                                                                        Math.Max(r1.Item2, r2.Item2)));
            var maxSignalAmpl = Math.Max(Math.Abs(range.Item1),
                                         Math.Abs(range.Item2));
            return maxSignalAmpl;
        }
    }
}
