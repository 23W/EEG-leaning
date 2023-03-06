using Accord.Math;
using Accord.Statistics.Analysis;
using Accord.Statistics.Analysis.ContrastFunctions;
using MathNet.Numerics.LinearAlgebra;

namespace EEGCore.Processing.ICA
{
    internal class FastICA_Accord : FastICAEngine
    {
        internal enum NonGaussianityEstimation
        {
            // According to Hyvärinen, the Logcosh contrast function is a good general-purpose
            // contrast function.
            LogCosh,

            // According to Hyvärinen, the kurtosis contrast function is justified on statistical
            // grounds only for estimating sub-Gaussian independent components when there are
            // no outliers.
            Kurtosis,

            // According to Hyvärinen, the Exponential contrast function may be used when the
            // independent components are highly super-Gaussian or when robustness is very important.
            Exponential
        }

        internal NonGaussianityEstimation Estimation { get; set; } = NonGaussianityEstimation.LogCosh;

        internal override int MaxIterationCount { get; set; } = 1000;

        internal override double Tolerance { get; set; } = 0.0001;

        IndependentComponentAnalysis? Engine { get; set; }

        internal override ICAResult Decompose(double[][] mixture, int? numOfComponents = default)
        {
            // Accord.NET implements FastICA algorithm
            Engine = new IndependentComponentAnalysis()
            {
                Algorithm = IndependentComponentAlgorithm.Parallel,
                Method = AnalysisMethod.Center,
                NumberOfOutputs = numOfComponents ?? 0,
                Iterations = MaxIterationCount,
                Tolerance = Tolerance,
                Overwrite = false
            };

            switch (Estimation)
            {
                case NonGaussianityEstimation.LogCosh:
                    Engine.Contrast = new Logcosh();
                    break;

                case NonGaussianityEstimation.Kurtosis:
                    Engine.Contrast = new Kurtosis();
                    break;

                case NonGaussianityEstimation.Exponential:
                    Engine.Contrast = new Exponential();
                    break;
            }

            var inputT = mixture.Transpose();

            // Compute the analysis
            var demixer = Engine.Learn(inputT);

            // Separate the input signals
            var res = new ICAResult()
            {
                Sources = demixer.Transform(inputT).Transpose(),
                W = Engine.DemixingMatrix.Transpose(),
                A = Engine.DemixingMatrix.PseudoInverse().Transpose(), // don't use MixingMatrix in IndependentComponentAnalysis, it's corrupted
            };

            return res;
        }

        internal override double[][] Compose(double[][] a, double[][] sources)
        {
            var sourcesMatrix = Matrix<double>.Build.DenseOfRowArrays(sources);
            var mixingMatrix = Matrix<double>.Build.DenseOfRowArrays(a);

            var mixture = (mixingMatrix * sourcesMatrix).ToRowArrays();
            return mixture;
        }
    }
}
