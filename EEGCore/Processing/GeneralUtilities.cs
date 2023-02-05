using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace EEGCore.Processing
{
    public static class GeneralUtilities
    {
        public static double GetMaximum(double[] samples)
        {
            var vector = new DenseVector(samples);
            return vector.Maximum();
        }

        public static double GetMinimum(double[] samples)
        {
            var vector = new DenseVector(samples);
            return vector.Minimum();
        }

        public static Tuple<double, double> GetMinimumMaximum(double[] samples)
        {
            var vector = new DenseVector(samples);
            return Tuple.Create(vector.Minimum(), vector.Maximum());
        }

        public static double GetMean(double[] samples)
        {
            var vector = new DenseVector(samples);
            return vector.Mean();
        }

        public static void Add(double[] samples, double value, double[] result)
        {
            var vectorSrc = new DenseVector(samples);
            var vectorDst = new DenseVector(samples);

            vectorSrc.Add(value, vectorDst);
        }
    }
}
