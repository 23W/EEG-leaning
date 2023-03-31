using EEGCore.Data;
using EEGCore.Processing.Model;
using System.Diagnostics;
using System.Text.Json;

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

            var inverseEEGTask = new InverseEEGTask() { Input = Input };
            var dipolesResult = inverseEEGTask.Analyze();
            if (dipolesResult.Succeed)
            {
#if DEBUG
                var currentPath = Directory.GetCurrentDirectory();
                foreach (var dipole in dipolesResult.Dipoles)
                {
                    var name = $"Dipole-{dipole.Lead!.Name}.json";
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    dipole.Lead = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                    File.WriteAllText(Path.Combine(currentPath, name), JsonSerializer.Serialize(dipole));
                }
#endif
            }

            return res;
        }
    }
}
