using EEGCore.Data;
using EEGCore.Processing.ICA;
using EEGCore.Utilities;
using System.Diagnostics;

namespace EEGCore.Processing.Analysis
{
    public class AutoArtifactCleanerResult : AnalysisResult
    {
        public class RangeResult
        {
            public RecordRange RecordRange { get; set; } = new RecordRange();

            public bool HasEyeArtifact { get; set; } = false;

            public bool HasSingleElectrodeArtifact { get; set; } = false;

            public bool HasReferenceElectrodeArtifact { get; set; } = false;
        }

        public Record? Output { get; set; } = new Record();

        public IEnumerable<RangeResult> Ranges { get; set; } = Enumerable.Empty<RangeResult>();
    }

    public class AutoArtifactCleaner : AnalyzerBase<AutoArtifactCleanerResult>
    {
        #region Properties

        public Record Input { get; init; } = new Record();

        public bool WholeRecord { get; set; } = false;

        public bool CleanSingleElectrodeArtifacts { get; set; } = true;

        public bool CleanReferenceElectrodeArtifacts { get; set; } = true;

        public bool CleanEyeArtifacts { get; set; } = true;

        #endregion

        public override AutoArtifactCleanerResult Analyze()
        {
            Debug.Assert(Input.LeadsCount > 0);
         
            var result = new AutoArtifactCleanerResult();

            var rangesResult = FindRanges();
            if (rangesResult.Succeed)
            {
                var composition = default(Record);
                var compositionRanges = new List<AutoArtifactCleanerResult.RangeResult>();

                foreach(var range in rangesResult.Ranges)
                {
                    var components = RunICADecompose(range);

                    if (CleanSingleElectrodeArtifacts || 
                        CleanReferenceElectrodeArtifacts)
                    {
                        var electrodeArtifactsResult = FindElectrodeArtifacts(components);
                    }

                    if (CleanEyeArtifacts)
                    {
                        var eyeArtifactsResult = FindEyeArtifacts(components);
                    }

                    var compositionRange = new AutoArtifactCleanerResult.RangeResult() { RecordRange = range };
                    bool makeComposition = false;

                    foreach (var (lead, componentIndex) in components.Leads.Cast<ComponentLead>().WithIndex())
                    {
                        if (lead.IsReferenceElectrodeArtifact && CleanReferenceElectrodeArtifacts)
                        {
                            lead.Suppress = SuppressType.ZeroLead;
                            compositionRange.HasReferenceElectrodeArtifact = true;
                            makeComposition = true;
                        }
                        else if (lead.IsSingleElectrodeArtifact && CleanSingleElectrodeArtifacts)
                        {
                            lead.Suppress = SuppressType.ZeroLead;
                            compositionRange.HasSingleElectrodeArtifact = true;
                            makeComposition = true;
                        }
                        else if (lead.IsEyeArtifact && CleanEyeArtifacts)
                        {
                            lead.Suppress = SuppressType.HiPass10;
                            compositionRange.HasEyeArtifact = true;
                            makeComposition = true;
                        }
                        else
                        {
                            lead.Suppress = SuppressType.None;
                        }

                        components.BuildLeadAlternativeSuppress(componentIndex);
                    }

                    if (makeComposition)
                    {
                        if (composition != default)
                        {
                            components.X = composition;
                        }

                        compositionRanges.Add(compositionRange);

                        composition = RunICACompose(components);
                    }
                }

                result.Output = composition;
                result.Ranges = compositionRanges;
                result.Succeed = composition != default &&
                                 compositionRanges.Any();
            }

            return result;
        }

        #region Helpter Methods

        RecordRangeResult FindRanges()
        {
            var result = default(RecordRangeResult);

            if (WholeRecord)
            {
                result = new RecordRangeResult()
                {
                    Ranges = new List<RecordRange>() { new RecordRange() { From = 0, Duration = Input.Duration } },
                    Succeed = true,
                };
            }
            else
            {
                var rangeDetector = new ArtifactCandidateRangeDetector() { Input = Input };
                result = rangeDetector.Analyze();
            }

            return result;
        }

        ICARecord RunICADecompose(RecordRange range)
        {
            var ica = new FastICA()
            {
                MaxIterationCount = 10000,
                Tolerance = 1E-06,
                NormalizePower = true,
            };

            var components = ica.Decompose(Input, range);
            return components;
        }

        Record RunICACompose(ICARecord components)
        {
            var ica = new FastICA();

            var composition = ica.Compose(components, SuppressComponents.MatrixAndComponents);
            return composition;
        }

        ComponentArtifactResult FindElectrodeArtifacts(ICARecord components)
        {
            var artifactsDetector = new ElectrodeArtifactDetector() { Input = components };
            var result = artifactsDetector.Analyze();
            return result;
        }

        ComponentArtifactResult FindEyeArtifacts(ICARecord components)
        {
            var artifactsDetector = new EyeArtifactDetector() { Input = components };
            var result = artifactsDetector.Analyze();
            return result;
        }

        #endregion
    }
}
