using Accord.Math;
using Accord.Statistics.Analysis;
using Accord.Statistics.Analysis.ContrastFunctions;

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

        internal override ICAResult Solve(double[][] input, int? numOfComponents = default)
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

            var inputT = input.Transpose();

            // Compute the analysis
            var demixer = Engine.Learn(inputT);

            // Separate the input signals
            var res = new ICAResult()
            {
                Sources = demixer.Transform(inputT).Transpose(),
                A = Engine.MixingMatrix.Transpose(),
                W = Engine.DemixingMatrix.Transpose(),
            };

            return res;
        }
    }
}
