namespace EEGCore.Processing.Analysis
{
    public class AnalysisResult
    {
        // Is "true" when analysis has been successful and result was achived.
        public bool Succeed { get; set; } = false;
    }


    public abstract class AnalyzerBase<T> where T : AnalysisResult
    {
        public abstract T Analyze();
    }


    public static class AnalyzerUtils
    {
        public delegate void SaturationDelegate(int srcIndex);
        public delegate void ProcessDelegate(int srcIndex, int dstIndex);
        public delegate void DesaturationDelegate(int dstIndex);

        public static void UnshiftAnalysis(int totalCount, int shiftLength, SaturationDelegate saturation, ProcessDelegate process, DesaturationDelegate desaturation)
        {
            var srcIndex = 0;
            var dstIndex = 0;
            var saturationLength = Math.Min(shiftLength, totalCount);

            // saturate the process (initialize the delay timeline by shift length)
            for (; srcIndex < saturationLength; srcIndex++)
            {
                saturation(srcIndex);
            }

            // payload (process and put result from begin of destination)
            for (; srcIndex < totalCount; srcIndex++, dstIndex++)
            {
                process(srcIndex, dstIndex);
            }

            // desaturate the process (extract rest of delay timeline)
            for (; dstIndex < totalCount; dstIndex++)
            {
                desaturation(dstIndex);
            }
        }
    }
}
