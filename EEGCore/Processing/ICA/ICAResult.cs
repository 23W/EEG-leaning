namespace EEGCore.Processing.ICA
{
    public class ICAResult
    {
        public double[][] Sources { get; set; } = new double[0][];

        // Mixing matrix
        public double[][] A { get; set; } = new double[0][];

        // Demixing matrix
        public double[][] W { get; set; } = new double[0][];
    }
}
