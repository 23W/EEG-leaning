using EEGCore.Data;
using EEGCore.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;
using System.Diagnostics;

namespace EEGCore.Processing.Analysis
{
    public class ComponentArtifactResult : AnalysisResult
    {
        public IEnumerable<ComponentLead> ArtifactComponents { get; set; } = Enumerable.Empty<ComponentLead>();
    }

    public class ElectrodeArtifactDetector : AnalyzerBase<ComponentArtifactResult>
    {
        public ICARecord Input { get; init; } = new ICARecord();

        public double SingleElectrodeThreshold { get; set; } = 0.85;

        public double ReferenceElectrodeSlope { get; set; } = Math.Tan(1 * Math.PI / 180);
        public double ReferenceElectrodeMaxDistance { get; set; } = 0.2;

        public override ComponentArtifactResult Analyze()
        {
            var res = new ComponentArtifactResult();

            if (Input.X != default(Record))
            {
                var res1 = AnalyzeSingleElectrodeArtifact();
                var res2 = AnalyzeReferenceElectrodeArtifact();

                res.Succeed = res1.Succeed || res2.Succeed;
                res.ArtifactComponents = res1.ArtifactComponents.Union(res2.ArtifactComponents).ToList();
            }

            return res;
        }

        #region Helper Methods

        double[] BuildSingleElectrodeArtifactSignature(Record record, int leadIndex)
        {
            Debug.Assert(leadIndex < record.LeadsCount);

            var res = new double[record.LeadsCount];
            Array.Fill(res, 0.0);
            res[leadIndex] = 1.0;

            return res;
        }

        IEnumerable<double[]> BuildSingleElectrodeArtifactSignatures(Record record)
        {
            var res = Enumerable.Range(0, record.LeadsCount)
                                .Select(leadIndex => BuildSingleElectrodeArtifactSignature(record, leadIndex))
                                .ToList();
            return res;
        }

        ComponentArtifactResult AnalyzeSingleElectrodeArtifact()
        {
            Debug.Assert(Input.X != default);

            var res = new ComponentArtifactResult();

            var signatures = BuildSingleElectrodeArtifactSignatures(Input.X);
            var artefacts = new HashSet<ComponentLead>();

            foreach (var (lead, componentIndex) in Input.Leads.Cast<ComponentLead>().WithIndex())
            {
                var componentWeights = Input.GetMixingVector(componentIndex);

                foreach (var signature in signatures.Where(s => s.Length == componentWeights.Length))
                {
                    var correlation = Math.Abs(Correlation.Pearson(componentWeights, signature));
                    if (correlation >= SingleElectrodeThreshold)
                    {
                        var artifactInfo = new ArtifactInfo()
                        {
                            ArtifactType = ArtifactType.SingleElectrodeArtifact,
                            Probaprobability = correlation
                        };

                        lead.AddArtifactInfo(artifactInfo);
                        artefacts.Add(lead);
                    }
                }
            }

            res.Succeed = artefacts.Any();
            res.ArtifactComponents = artefacts.ToList();

            return res;
        }

        ComponentArtifactResult AnalyzeReferenceElectrodeArtifact()
        {
            Debug.Assert(Input.X != default);

            var res = new ComponentArtifactResult();

            var artefacts = new HashSet<ComponentLead>();

            foreach (var (lead, componentIndex) in Input.Leads.Cast<ComponentLead>().WithIndex())
            {
                var weights = Input.GetMixingVector(componentIndex);

                // all weights have same sign
                var firstWeight = weights.First(w => w != 0);
                var differentSign = weights.Any(w => (w * firstWeight) < 0);
                if (!differentSign)
                {
                    var weightSamples = weights.Select((w, i) => Tuple.Create((double)(i + 1), w))
                                               .ToArray();

                    var lineRegression = SimpleRegression.Fit(weightSamples);

                    var modelWeights = weightSamples.Select(s => lineRegression.A + lineRegression.B * s.Item1)
                                                    .ToArray();

                    //var r2 = GoodnessOfFit.R(modelWeights, weights);
                    var l2 = Distance.Euclidean(modelWeights, weights);

                    // l2 is good enough and slope B is less than 1 degree
                    if ((l2 <= ReferenceElectrodeMaxDistance) &&
                        (Math.Abs(lineRegression.B) <= ReferenceElectrodeSlope))
                    {
                        var artifactInfo = new ArtifactInfo()
                        {
                            ArtifactType = ArtifactType.ReferenceElectrodeArtifact,
                            Probaprobability = 1 // TODO: implement!
                        };

                        lead.AddArtifactInfo(artifactInfo);
                        artefacts.Add(lead);
                    }
                }
            }

            res.Succeed = artefacts.Any();
            res.ArtifactComponents = artefacts.ToList();

            return res;
        }

        #endregion
    }
}
