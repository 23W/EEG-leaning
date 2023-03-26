using EEGCore.Data;
using EEGCore.Utilities;
using MathNet.Numerics.Statistics;
using System.Diagnostics;

namespace EEGCore.Processing.Analysis
{
    public class ComponentArtifactResult : AnalysisResult
    {
        public IEnumerable<ComponentLead> ArticatComponents { get; set; } = Enumerable.Empty<ComponentLead>();
    }

    public class ElectrodeArtifactDetector : AnalyzerBase<ComponentArtifactResult>
    {
        public override ComponentArtifactResult Analyze(ICARecord input)
        {
            var res = new ComponentArtifactResult();

            if (input.X != default(Record))
            {
                res = AnalyzeSingleElectrodeArtifact(input);
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

        IEnumerable<Tuple<ArtifactType, double[]>> BuildSingleElectrodeArtifactSignatures(Record record)
        {
            var res = Enumerable.Range(0, record.LeadsCount)
                                .Select(leadIndex => Tuple.Create(ArtifactType.SingleElectrodeArtifact,
                                                                  BuildSingleElectrodeArtifactSignature(record, leadIndex)))
                                .ToList();
            return res;
        }

        ComponentArtifactResult AnalyzeSingleElectrodeArtifact(ICARecord input)
        {
            Debug.Assert(input.X != default);

            var res = new ComponentArtifactResult();

            var signatures = BuildSingleElectrodeArtifactSignatures(input.X);
            var artefacts = new HashSet<ComponentLead>();

            foreach (var (lead, componentIndex) in input.Leads.Cast<ComponentLead>().WithIndex())
            {
                var componentWeights = input.GetMixingVector(componentIndex);

                foreach (var signature in signatures.Where(s => s.Item2.Length == componentWeights.Length))
                {
                    var correlation = Math.Abs(Correlation.Pearson(componentWeights, signature.Item2));
                    if (correlation >= 0.85)
                    {
                        lead.AddArtifactInfo(new ArtifactInfo() { ArtifactType = signature.Item1, Probaprobability = correlation }, false);
                        artefacts.Add(lead);
                    }
                }
            }

            res.Succeed = artefacts.Any();
            res.ArticatComponents = artefacts.ToList();

            return res;
        }

        #endregion
    }
}
