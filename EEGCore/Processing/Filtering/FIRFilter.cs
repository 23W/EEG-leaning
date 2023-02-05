using MathNet.Filtering.FIR;

namespace EEGCore.Processing.Filtering
{
    internal class FIRFilter : Filter
    {
        internal FIRFilter(double[] window)
        {
            Filter = new OnlineFirFilter(window);
        }

        public override void Reset() => Filter.Reset();

        public override double Process(double sample) => Filter.ProcessSample(sample);

        public override double[] Process(double[] samples) => Filter.ProcessSamples(samples);

        OnlineFirFilter Filter { get; init; }
    }
}
