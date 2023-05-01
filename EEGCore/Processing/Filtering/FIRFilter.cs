using MathNet.Filtering.FIR;

namespace EEGCore.Processing.Filtering
{
    public class FIRFilter : IFilter
    {
        #region Properties

        public bool AutoUnshifting { get; set; } = true;

        public double[] Window { get; private init; }

        public int WindowShift => Window.Length / 2;

        #endregion

        #region Helper Properties

        OnlineFirFilter Filter { get; init; }

        #endregion

        #region Construction

        internal FIRFilter(double[] window)
        {
            Window = window;
            Filter = new OnlineFirFilter(Window);
        }

        #endregion

        #region IFilter implementation

        public double Process(double sample) => Filter.ProcessSample(sample);

        public double[] Process(double[] samples)
        {
            var destination = default(double[]);

            if (AutoUnshifting)
            {
                destination = new double[samples.Length];

                var srcIndex = 0;
                var dstIndex = 0;
                var saturationLength = Math.Min(WindowShift, samples.Length);

                // saturate the filter (initialize the filter's window by its half width)
                for (; srcIndex< saturationLength; srcIndex++)
                {
                    Filter.ProcessSample(samples[srcIndex]);
                }

                // filtering (put filtering result from begin of destination buffer)
                for (; srcIndex < samples.Length; srcIndex++, dstIndex++)
                {
                    destination[dstIndex] = Filter.ProcessSample(samples[srcIndex]);
                }

                // desaturate the filter (extract rest of filter's window information by filtering via last sample)
                for (; dstIndex < destination.Length; dstIndex++)
                {
                    destination[dstIndex] = Filter.ProcessSample(samples[srcIndex-1]);
                }
            }
            else
            {
                destination = Filter.ProcessSamples(samples);
            }

            return destination;
        }

        #endregion
    }
}
