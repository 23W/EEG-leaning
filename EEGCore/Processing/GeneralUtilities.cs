using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace EEGCore.Processing
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return Math.PI * val / 180;
        }

        public static double ToDegrees(this double val)
        {
            return val * 180 / Math.PI;
        }
    }

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

        public static double[] GetMatrixColumn(double[][] matrix, int column)
        {
            var m = Matrix<double>.Build.DenseOfRowArrays(matrix);
            var result = m.Column(column).ToArray();

            return result;
        }

        public static double[][] CloneMatrix(double[][] matrix)
        {
            var m = Matrix<double>.Build.DenseOfRowArrays(matrix);
            var result = m.ToRowArrays();

            return result;
        }
    }
}
