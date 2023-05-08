using MathNet.Filtering.FIR;

namespace EEGCore.Processing.Filtering
{
    public static class FilterFactory
    {
        public static IFilter BuildLowPassFilter(double samplingRate, double cutOff, int order = 0)
        {
            order = order > 0 ? order : CalcOrder(samplingRate, cutOff);

            var window = FirCoefficients.LowPass(samplingRate, cutOff, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static IFilter BuildHighPassFilter(double samplingRate, double cutOff, int order = 0)
        {
            order = order > 0 ? order : CalcOrder(samplingRate, cutOff);

            var window = FirCoefficients.HighPass(samplingRate, cutOff, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static IFilter BuildBandPassFilter(double samplingRate, double cutOffLow, double cutOffHigh, int order = 0)
        {
            var lowOrder = order > 0 ? order : CalcOrder(samplingRate, cutOffLow);
            var highOrder = order > 0 ? order : CalcOrder(samplingRate, cutOffHigh);
            order = Math.Max(lowOrder, highOrder);

            var window = FirCoefficients.BandPass(samplingRate, cutOffLow, cutOffHigh, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        public static IFilter BuildBandStopFilter(double samplingRate, double cutOffLow, double cutOffHigh, int order = 0)
        {
            var lowOrder = order > 0 ? order : CalcOrder(samplingRate, cutOffLow);
            var highOrder = order > 0 ? order : CalcOrder(samplingRate, cutOffHigh);
            order = Math.Max(lowOrder, highOrder);

            var window = FirCoefficients.BandStop(samplingRate, cutOffLow, cutOffHigh, halforder: order >> 1);
            var filter = new FIRFilter(window);

            return filter;
        }

        #region Constatnts

        public const int c_minimalOrder = 64;

        #endregion

        #region Helper Methods

        static int CalcOrder(double samplingRate, double cutOff)
        {
            var reqOrder = (int)Math.Ceiling((samplingRate * 2) / cutOff);
            if ((reqOrder % 2) != 0)
            {
                reqOrder += 1;
            }

            var order = Math.Max(reqOrder, c_minimalOrder);
            return order;
        }

        #endregion
    }
}
