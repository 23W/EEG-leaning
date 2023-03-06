using EEGCore.Data;
using EEGCore.Utilities;
using System.Diagnostics;

namespace EEGCore.Processing.Analysis
{
    public class EyeArtifactResult : AnalysisResult
    {

    }

    public class EyeArtifactDetector
    {
        public EyeArtifactResult Analyze(ICARecord input)
        {
            Debug.Assert(input.LeadsCount > 0);

            var res = new EyeArtifactResult();

            if (input.X == default ||
               !input.X.Leads.Any(l=>l is EEGLead))
            {
                return res;
            }

            foreach(var (componentLead, index) in input.Leads.WithIndex())
            {
                var componentWeights = input.GetMixingVector(index);
            }

            return res;
        }
    }
}
