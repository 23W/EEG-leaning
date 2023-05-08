using EEGCore.Data;
using EEGCore.Processing.Filtering;
using EEGCore.Processing.Model;
using MathNet.Numerics.Statistics;
using System.Diagnostics;

namespace EEGCore.Processing.Analysis
{
    public class RecordRangeResult : AnalysisResult
    {
        public IEnumerable<RecordRange> Ranges { get; set; } = Enumerable.Empty<RecordRange>();
    }

    public class ArtifactCandidateRangeDetector : AnalyzerBase<RecordRangeResult>
    {
        public enum ApplyRangesToInput
        {
            None,
            Replace,
            ReplaceSameName,
        };

        #region Properties

        public Record Input { get; init; } = new Record();

        public double WindowWidthSeconds { get; set; } = 1;

        public double DeviationThreshould { get; set; } = 3;

        public double RangeMinDurationSecounds { get; set; } = 0.1;

        public double RangeMarginSecounds { get; set; } = 2;

        public double RangePaddingSecounds { get; set; } = 1.5;
        
        public ApplyRangesToInput ApplyToInput { get; set; } = ApplyRangesToInput.Replace;

        #endregion

        public override RecordRangeResult Analyze()
        {
            Debug.Assert(Input.LeadsCount > 0);

            var input = PrepeareInput();
            var leadAnalysis = input.Leads.AsParallel()
                                          .Select(AnalyzeLead).ToList();
            /* for debug:
            foreach (var (samples, index) in leadAnalysis.Select(la => Tuple.Create(la.Item2, la.Item3)))
            {
                Input.Leads[index].Samples = samples;
            }
            */

            var ranges = leadAnalysis.Select(la => la.Item1)
                                     .Aggregate(new List<RecordRange>(), (allRanges, leadRanges) => { allRanges.AddRange(leadRanges); return allRanges; });

            // build joint result
            var resultRanges = new List<RecordRange>();
            foreach (var leadRange in ranges)
            {
                if (!resultRanges.Any(r => r.Contains(leadRange)))
                {
                    var intersectionIndex = resultRanges.FindIndex(r => r.HasIntersection(leadRange));
                    if (intersectionIndex >= 0)
                    {
                        resultRanges[intersectionIndex] = leadRange.Union(resultRanges[intersectionIndex]);
                    }
                    else
                    {
                        resultRanges.Add(leadRange);
                    }
                }
            }

            // sort by left boundary
            resultRanges.Sort((r1, r2) => r1.From - r2.From);

            // apply marging (join ranges with gap less than margin)
            {
                var rangeMarging = (int)Math.Round(RangeMarginSecounds * Input.SampleRate);

                bool again;
                do
                {
                    again = false;

                    for (var index = 0; (index < resultRanges.Count) && !again; index++)
                    {
                        var range1 = resultRanges[index];
                        var boundary = range1.From +
                                       range1.Duration;

                        for (var nextIndex = index + 1; (nextIndex < resultRanges.Count) && !again; nextIndex++)
                        {
                            var range2 = resultRanges[nextIndex];
                            if ((range2.From - boundary) <= rangeMarging)
                            {
                                resultRanges[index] = range1.Union(range2);
                                resultRanges.RemoveAt(nextIndex);
                                again = true;
                            }
                        }
                    }
                }
                while (again);
            }

            // set up range names
            foreach (var range in resultRanges)
            {
                range.Name = "Artifact candidate";
            }

            var res = new RecordRangeResult() 
            { 
                Ranges = resultRanges,
                Succeed = resultRanges.Any()
            };

            if (res.Succeed)
            {
                switch (ApplyToInput)
                {
                    case ApplyRangesToInput.Replace:
                        Input.Ranges.Clear();
                        Input.Ranges.AddRange(res.Ranges);
                        break;

                    case ApplyRangesToInput.ReplaceSameName:
                        Input.Ranges.RemoveAll(src => res.Ranges.Any(dst => dst.Name.Equals(src.Name, StringComparison.OrdinalIgnoreCase)));
                        Input.Ranges.AddRange(res.Ranges);
                        Input.Ranges.Sort((r1, r2) => r1.From - r2.From);
                        break;
                }
            }

            return res;
        }

        Record PrepeareInput()
        {
            var prepearedInput = Input.Clone();

            var filter = FilterFactory.BuildBandPassFilter(Input.SampleRate, 0.5, 8.0);
            filter.ProcessInplace(prepearedInput.Leads);

            return prepearedInput;
        }

        Tuple<IEnumerable<RecordRange>, double[], int> AnalyzeLead(Lead lead, int leadIndex)
        {
            var ranges = new List<RecordRange>();

            var samples = lead.Samples;
            var length = samples.Length;
            var windowWidth = (int)Math.Round(WindowWidthSeconds * Input.SampleRate);
            var rangeMinDuration = (int)Math.Round(RangeMinDurationSecounds * Input.SampleRate);
            var rangePadding = (int)Math.Round(RangePaddingSecounds * Input.SampleRate);
            var shift = windowWidth / 2;

            // build deviation function with window
            var devSamples = new double[length];
            var window = new WindowFunction() { Width = (int)Math.Round(WindowWidthSeconds * Input.SampleRate) };
            window.Fill(samples[0]);
            AnalyzerUtils.UnshiftAnalysis(length, shift,
                                          (srcIndex) => window.Add(samples[srcIndex]),
                                          (srcIndex, dstIndex) => { devSamples[dstIndex] = Statistics.StandardDeviation(window.Samples); window.Add(samples[srcIndex]); },
                                          (dstIndex) => { devSamples[dstIndex] = Statistics.StandardDeviation(window.Samples); window.Add(samples[length-1]); });


            // find all ranges with deviation paroxysm
            var filter = FilterFactory.BuildHighPassFilter(Input.SampleRate, 1.5);
            var baseSamples = filter.Process(lead.Samples);
            var (mean, var) = Statistics.MeanStandardDeviation(baseSamples);
            var genDev = (/*mean +*/ var) * DeviationThreshould;

            for (var startIndex = 0; startIndex< length; startIndex++)
            {
                var isStart = devSamples[startIndex] > genDev;
                if (isStart)
                {
                    var endIndex = startIndex;
                    while ((endIndex < length) && (devSamples[endIndex] > genDev))
                    {
                        endIndex++;
                    }

                    var duration = endIndex - startIndex + 1;
                    if (duration >= rangeMinDuration)
                    {
                        var from = Math.Max(0, startIndex - rangePadding);
                        var to = Math.Min(length - 1, startIndex + duration + rangePadding);

                        ranges.Add(new RecordRange() { From = from, Duration = (to - from + 1) });
                    }

                    startIndex = endIndex;
                }
            }

            // returns found ranges, deviation function, lead index
            return Tuple.Create(ranges.AsEnumerable(), devSamples, leadIndex);
        }
    }
}
