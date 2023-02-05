using MathNet.Filtering.FIR;

namespace EEGCore.Processing.Filtering
{
    public static class FilterFactory
    {
        public static Filter BuildLowPassFilter(double samplingRate, double cutOff, int order = 32)
        {
            var window = FirCoefficients.LowPass(samplingRate, cutOff, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static Filter BuildHighPassFilter(double samplingRate, double cutOff, int order = 32)
        {
            var window = FirCoefficients.HighPass(samplingRate, cutOff, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static Filter BuildBandPassFilter(double samplingRate, double cutOffLow, double cutOffHigh, int order = 32)
        {
            var window = FirCoefficients.BandPass(samplingRate, cutOffLow, cutOffHigh, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static Filter BuildBandStopFilter(double samplingRate, double cutOffLow, double cutOffHigh, int order = 32)
        {
            var window = FirCoefficients.BandStop(samplingRate, cutOffLow, cutOffHigh, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }
    }
}
