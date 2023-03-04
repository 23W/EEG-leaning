using Accord.Math;

namespace EEGCore.Processing.ICA
{
    internal abstract class FastICAEngine
    {
        internal abstract int MaxIterationCount { get; set; }

        internal abstract double Tolerance { get; set; }

        internal abstract ICAResult Solve(double[][] input, int? numOfComponents = default);
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

        public ICAResult Solve(double[,] input, int? numOfComponents = default) => Solve(input.ToJagged(), numOfComponents);

        public ICAResult Solve(double[][] input, int? numOfComponents = default) => Engine.Solve(input, numOfComponents);
    }
}
