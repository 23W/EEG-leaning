namespace EEGCore.Processing.Generators
{
    public interface ISampleGenerator
    {
        public void Generate(double[] samples, bool addGenerated = false);
    }
}
