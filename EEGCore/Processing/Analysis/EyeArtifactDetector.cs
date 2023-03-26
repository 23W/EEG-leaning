using EEGCore.Data;
using EEGCore.Utilities;
using System.Diagnostics;

namespace EEGCore.Processing.Analysis
{

    public class EyeArtifactDetector : AnalyzerBase<ComponentArtifactResult>
    {
        public override ComponentArtifactResult Analyze(ICARecord input)
        {
            Debug.Assert(input.LeadsCount > 0);

            var res = new ComponentArtifactResult();

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
