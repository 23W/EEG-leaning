using Accord.Math;
using EEGCore.Data;
using EEGCore.Processing.Analysis;
using EEGCore.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using System.Diagnostics;
using Vector = EEGCore.Data.Vector;

namespace EEGCore.Processing.Model
{
    public class DipoleResult
    {
        public ComponentLead Lead { get; set; } = new ComponentLead();

        public Dipole Dipole { get; set; } = new Dipole();

        public double[] ModelWeights { get; set; } = Array.Empty<double>();

        public double[] ComponentWeights { get; set; } = Array.Empty<double>();

        public Vector[] WeightLocations { get; set; } = Array.Empty<Vector>();

        public double Probaprobability { get; set; } = 0;

        public double Nonconformance { get; set; } = 0;
    }

    public class DipolesResult : AnalysisResult
    {
        public IEnumerable<DipoleResult> Dipoles { get; set; } = Enumerable.Empty<DipoleResult>();
    }

    public class InverseEEGTask : AnalyzerBase<DipolesResult>
    {
        public ICARecord Input { get; init; } = new ICARecord();

        public double Threshold { get; set; } = 0.7;

        public int AngleStep { get; set; } = 10;

        public int R100Step { get; set; } = 10;

        public override DipolesResult Analyze()
        {
            var res = new DipolesResult();

            if (Input.X != default(Record))
            {
                // parallel calculation
                var results = Enumerable.Range(0, Input.LeadsCount)
                                        .AsParallel()
                                        .Select(componentIndex => FindDipole(componentIndex))
                                        .ToList();

                res.Dipoles = results.Where(dipole => dipole != default)
                                     .Cast<DipoleResult>()
                                     .ToList();
                res.Succeed = res.Dipoles.Any();
            }
            return res;
        }

        public DipoleResult? FindDipole(int componentIndex)
        {
            Debug.Assert(Input.X != default);
            Debug.Assert(componentIndex < Input.LeadsCount);

            var res = default(DipoleResult);

            // collection of scalp location for known EEG leads 
            var knownLeadCoordinates = Input.X.Leads.Cast<EEGLead>()
                                                    .WithIndex()
                                                    .Where(l => l.item.LeadType != LeadType.Unknown)
                                                    .Select(l => new { item = ScalpSphere.GetLeadXYZ(l.item.Name), l.index })
                                                    .Where(l => l.item.HasValue)
                                                    .ToList();
            if (knownLeadCoordinates.Any())
            {
                var knownCoordinates = knownLeadCoordinates.Select(l => l.item)
                                                           .Cast<Vector>()
                                                           .WithIndex()
                                                           .ToList();

                // component weigths array
                var componentWeights = Input.GetMixingVector(componentIndex);

                // the same array but with weights of known leads
                var knowWeights = componentWeights.WithIndex()
                                                  .Where(c => knownLeadCoordinates.Any(l => l.index == c.index))
                                                  .Select(c=>c.item)
                                                  .ToArray();

                // array for weights calcualted from model
                var modelWeights = new double[knownLeadCoordinates.Count];

                var dipole = new Dipole();
                var first = true;
                var bestDipolesResult = new DipoleResult() 
                {
                    Lead = (ComponentLead)Input.Leads[componentIndex],
                    ComponentWeights = knowWeights,
                    WeightLocations = knownCoordinates.Select(c => c.item).ToArray(),
                };

                for (var alpha = -180; alpha < 180; alpha += AngleStep)
                {
                    for (var beta = -30; beta <= 90; beta += AngleStep)
                    {
                        for (var r100 = 0; r100 <= 100; r100 += R100Step)
                        {
                            var r = r100 / 100.0;

                            for (var momentAlpha = -180; momentAlpha < 180; momentAlpha += AngleStep)
                            {
                                for (var momentBeta = -90; momentBeta < 90; momentBeta += AngleStep)
                                {
                                    var momentR = 100;
                                    //for (var momentR = 100; momentR > 0; momentR -= (R100Step / 2))
                                    {
                                        dipole.Location = new PolarCoordinate(alpha, beta, r);
                                        dipole.Moment = new PolarCoordinate(momentAlpha, momentBeta, momentR / 100.0);

                                        foreach (var (coordinate, index) in knownCoordinates)
                                        {
                                            modelWeights[index] = dipole.CalcPotential(coordinate);
                                        }

                                        var nonconformance = Nonconformance(modelWeights, knowWeights);
                                        if (first ||
                                            IsNonconformanceBetter(nonconformance,bestDipolesResult.Nonconformance))
                                        {
                                            first = false;
                                            bestDipolesResult.Nonconformance = nonconformance;
                                            bestDipolesResult.Probaprobability = Correlation.Pearson(modelWeights, knowWeights);
                                            bestDipolesResult.Dipole = dipole.Clone();
                                            bestDipolesResult.ModelWeights = (double[])modelWeights.Clone();
                                        }
                                    }
                                }
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

        internal static double Nonconformance(double[] model, double[] samples)
        {
            var modelVector = new DenseVector(model).Normalize(2);
            var samplesVector = new DenseVector(samples).Normalize(2);

            var nonconformance = GoodnessOfFit.PopulationStandardError(modelVector, samplesVector);
            return nonconformance;
        }

        internal static bool IsNonconformanceBetter(double val1, double val2)
        {
            return val1 < val2;
        }
    }
}
