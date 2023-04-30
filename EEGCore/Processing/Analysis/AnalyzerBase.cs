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
}
