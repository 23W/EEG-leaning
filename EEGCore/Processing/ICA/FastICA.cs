using Accord.Math;
using EEGCore.Utilities;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace EEGCore.Processing.ICA
{
    internal abstract class FastICAEngine
    {
        internal abstract int MaxIterationCount { get; set; }

        internal abstract double Tolerance { get; set; }

        internal abstract ICAResult Decompose(double[][] mixture, int? numOfComponents = default);

        internal abstract double[][] Compose(double[][] a, double[][] sources);
    }

    public class FastICA
    {
        public int MaxIterationCount
        {
            get => Engine.MaxIterationCount;
            set => Engine.MaxIterationCount = value;
        }

        public double Tolerance 
        { 
            get => Engine.Tolerance;
            set => Engine.Tolerance = value;
        }

        public bool NormalizePower { get; set; } = false;

        FastICAEngine Engine { get; init; }

        public FastICA()
        {
            Engine = new FastICA_Accord();
        }

        public ICAResult Decompose(double[,] mixture, int? numOfComponents = default) => Decompose(mixture.ToJagged(), numOfComponents);

        public ICAResult Decompose(double[][] mixture, int? numOfComponents = default)
        {
            var res = Engine.Decompose(mixture, numOfComponents);

            if (NormalizePower)
            {
                var a = Matrix<double>.Build.DenseOfRowArrays(res.A);
                var norms = a.ColumnNorms(2.0);

                var it = a.EnumerateColumns().Select((column, index) => column / norms[index]);
                var aNorm = Matrix<double>.Build.DenseOfColumnVectors(it);
                var wNorm = aNorm.PseudoInverse();

                res.A = aNorm.ToRowArrays();
                res.W = aNorm.ToRowArrays();

                foreach (var (component, index) in res.Sources.WithIndex())
                {
                    var c = new DenseVector(component);
                    c.MapInplace(v => v * norms[index]);
                }
            }

            return res;
        }

        public double[][] Compose(double[][] a, double[][] sources) => Engine.Compose(a, sources);
    }
}
