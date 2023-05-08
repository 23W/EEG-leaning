namespace EEGCore.Processing.Filtering
{
    public interface IFilter
    {
        public double Process(double sample);

        public double[] Process(double[] sample);
    }
}
