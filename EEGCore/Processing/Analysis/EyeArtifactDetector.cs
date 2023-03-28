using EEGCore.Data;
using EEGCore.Utilities;
using System.Diagnostics;

namespace EEGCore.Processing.Analysis
{
    public class EyeArtifactDetector : AnalyzerBase<ComponentArtifactResult>
    {
        public ICARecord Input { get; init; } = new ICARecord();

        public override ComponentArtifactResult Analyze()
        {
            Debug.Assert(Input.LeadsCount > 0);

            var res = new ComponentArtifactResult();

            if (Input.X == default ||
               !Input.X.Leads.Any(l=>l is EEGLead))
            {
                return res;
            }

            foreach(var (componentLead, index) in Input.Leads.WithIndex())
            {
                var componentWeights = Input.GetMixingVector(index);
            }

            return res;
        }
    }
}
