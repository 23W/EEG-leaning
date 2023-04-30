namespace EEGCore.Processing.Filtering
{
    public abstract class Filter
    {
        public abstract void Reset();

        public abstract double Process(double sample);

        public abstract double[] Process(double[] sample);
    }
}
