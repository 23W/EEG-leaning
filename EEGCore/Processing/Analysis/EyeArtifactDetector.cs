using EEGCore.Data;
using EEGCore.Processing.Model;
using EEGCore.Utilities;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using System.Diagnostics;
using System.Text.Json;
using Vector = EEGCore.Data.Vector;

namespace EEGCore.Processing.Analysis
{
    public class EyeArtifactDetector : AnalyzerBase<ComponentArtifactResult>
    {
        internal class IndexedScalpLead
        {
            internal EEGLead Lead { get; init; } = new EEGLead();
            internal Vector Location { get; init; } = new Vector();
            internal int Index { get; init; } = 0;
        }

        public ICARecord Input { get; init; } = new ICARecord();

        public double Threshold { get; set; } = 0.8;

        internal IEnumerable<IndexedScalpLead> KnownLeads => (Input.X == default) ? Enumerable.Empty<IndexedScalpLead>() :
                                                                                    Input.X.Leads.Cast<EEGLead>()
                                                                                                 .WithIndex()
                                                                                                 .Where(l => ScalpSphere.IsKnownLead(l.item.Name))
                                                                                                 .Select(l => new IndexedScalpLead()
                                                                                                 {
                                                                                                     Lead = l.item,
                                                                                                     Location = ScalpSphere.GetLeadXYZ(l.item.Name)!.Value,
                                                                                                     Index = l.index
                                                                                                 })
                                                                                                 .ToList();
        internal int AlphaStart { get; set; } = -30;
        internal int AlphaEnd { get; set; } = 30;
        internal int BetaStart { get; set; } = -20;
        internal int BetaEnd { get; set; } = 20;
        internal int AngleStep { get; set; } = 2;
        internal int R100Start { get; set; } = 50;
        internal int R100End { get; set; } = 100;
        internal int R100Step { get; set; } = 5;


        public override ComponentArtifactResult Analyze()
        {
            Debug.Assert(Input.LeadsCount > 0);

            var res = new ComponentArtifactResult();

            if (Input.X == default ||
               !Input.X.Leads.Any(l=>l is EEGLead))
            {
                return res;
            }

            // parallel calculation
            var results = Enumerable.Range(0, Input.LeadsCount)
                                    .AsParallel()
                                    .Select(FindEyeWeightsModel)
                                    .Where(dipole => dipole != default)
                                    .Cast<DipoleResult>()
                                    .Where(dipole => Math.Abs(dipole.Correlation) >= Threshold)
                                    .ToList();

            res.Succeed = results.Any();
            res.ArtifactComponents = results.Select(dipole => 
                                                  {
                                                      var artifactInfo = new ArtifactInfo()
                                                      {
                                                          ArtifactType = ArtifactType.EyeArtifact,
                                                          Probaprobability = Math.Abs(dipole.Correlation),
                                                      };
                                                      dipole.Lead.AddArtifactInfo(artifactInfo);
                                                      return dipole.Lead;
                                                  })
                                           .ToList();

#if DEBUG
            if (results.Any())
            {
                var currentPath = Directory.GetCurrentDirectory();
                foreach (var dipole in results)
                {
                    var name = $"EyeDipole-{dipole.Lead!.Name}.json";
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    dipole.Lead = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                    File.WriteAllText(Path.Combine(currentPath, name), JsonSerializer.Serialize(dipole));
                }
            }
#endif

            return res;
        }

        public DipoleResult? FindEyeWeightsModel(int componentIndex)
        {
            Debug.Assert(Input.X != default);
            Debug.Assert(componentIndex < Input.LeadsCount);

            var res = default(DipoleResult);

            // collection of scalp location for known EEG leads 
            var knownLeads = KnownLeads;
            if (knownLeads.Any())
            {
                var knownCoordinates = knownLeads.Select(l => l.Location)
                                                           .WithIndex()
                                                           .ToList();

                // component weigths array
                var componentWeights = Input.GetMixingVector(componentIndex);

                // the same array but with weights of known leads only
                var knownWeights = componentWeights.WithIndex()
                                                   .Where(c => knownLeads.Any(l => l.Index == c.index))
                                                   .Select(c => c.item)
                                                   .ToArray();
                // inversed version of known weights
                var inversedKnownWeights = new DenseVector(knownWeights).Multiply(-1).ToArray();

                // array for weights calcualted from model
                var modelWeights = new double[knownWeights.Length];

                var dipole = new Dipole();
                var first = true;
                var bestDipolesResult = new DipoleResult()
                {
                    Lead = (ComponentLead)Input.Leads[componentIndex],
                    ComponentWeights = knownWeights,
                    WeightLocations = knownCoordinates.Select(c => c.item).ToArray(),
                };

                foreach (var alpha in EnumerableExtensions.Range(AlphaStart, AlphaEnd, AngleStep))
                {
                    foreach (var beta in EnumerableExtensions.Range(BetaStart, BetaEnd, AngleStep))
                    {
                        foreach (var r100 in EnumerableExtensions.Range(R100Start, R100End, R100Step))
                        {
                            var r = r100 / 100.0;

                            var momentAlpha = alpha;
                            var momentBeta = beta;
                            var momentR = 100;

                            dipole.Location = new PolarCoordinate(alpha, beta, r);
                            dipole.Moment = new PolarCoordinate(momentAlpha, momentBeta, momentR / 100.0);

                            foreach (var (coordinate, index) in knownCoordinates)
                            {
                                modelWeights[index] = dipole.CalcPotential(coordinate);
                            }

                            var nonconformance = Nonconformance(modelWeights, knownWeights, inversedKnownWeights);
                            if (first ||
                                IsNonconformanceBetter(nonconformance, bestDipolesResult.Nonconformance))
                            {
                                first = false;
                                bestDipolesResult.Nonconformance = nonconformance;
                                bestDipolesResult.Correlation = Correlation.Pearson(modelWeights, knownWeights);
                                bestDipolesResult.Dipole = dipole.Clone();
                                bestDipolesResult.ModelWeights = (double[])modelWeights.Clone();
                            }
                        }
                    }
                }

                if (!first)
                {
                    res = bestDipolesResult;
                }
            }

            return res;
        }

        internal static double Nonconformance(double[] model, double[] samples1, double[] samples2)
        {
            var nc1 = InverseEEGTask.Nonconformance(model, samples1);
            var nc2 = InverseEEGTask.Nonconformance(model, samples2);

            var nonconformance = IsNonconformanceBetter(nc1, nc2) ? nc1 : nc2;
            return nonconformance;
        }

        internal static bool IsNonconformanceBetter(double val1, double val2)
        {
            return val1 < val2;
        }
    }
}
