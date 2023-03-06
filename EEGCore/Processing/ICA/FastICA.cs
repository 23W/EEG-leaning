using Accord.Math;

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

        FastICAEngine Engine { get; init; }

        public FastICA()
        {
            Engine = new FastICA_Accord();
        }

        public ICAResult Decompose(double[,] mixture, int? numOfComponents = default) => Decompose(mixture.ToJagged(), numOfComponents);

        public ICAResult Decompose(double[][] mixture, int? numOfComponents = default) => Engine.Decompose(mixture, numOfComponents);

        public double[][] Compose(double[][] a, double[][] sources) => Engine.Compose(a, sources);
    }
}
