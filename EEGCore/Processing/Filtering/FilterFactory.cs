using MathNet.Filtering.FIR;

namespace EEGCore.Processing.Filtering
{
    public static class FilterFactory
    {
        public const int c_defaultOrder = 64;

        public static IFilter BuildLowPassFilter(double samplingRate, double cutOff, int order = c_defaultOrder)
        {
            var window = FirCoefficients.LowPass(samplingRate, cutOff, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static IFilter BuildHighPassFilter(double samplingRate, double cutOff, int order = c_defaultOrder)
        {
            var window = FirCoefficients.HighPass(samplingRate, cutOff, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static IFilter BuildBandPassFilter(double samplingRate, double cutOffLow, double cutOffHigh, int order = c_defaultOrder)
        {
            var window = FirCoefficients.BandPass(samplingRate, cutOffLow, cutOffHigh, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static IFilter BuildBandStopFilter(double samplingRate, double cutOffLow, double cutOffHigh, int order = c_defaultOrder)
        {
            var window = FirCoefficients.BandStop(samplingRate, cutOffLow, cutOffHigh, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }
    }
}
