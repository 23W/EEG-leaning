using EEGCore.Data;
using EEGCore.Processing.Analysis;
using EEGCore.Utilities;
using System.Diagnostics;

namespace EEGCore.Processing.Model
{
    public class DipoleResult
    {
        public ComponentLead Lead { get; set; } = new ComponentLead();

        public Dipole Dipole { get; set; } = new Dipole();

        public double Probaprobability { get; set; } = 0;
    }

    public class DipolesResult : AnalysisResult
    {
        public IEnumerable<DipoleResult> Dipoles { get; set; } = Enumerable.Empty<DipoleResult>();
    }

    public class InverseEEGTask : AnalyzerBase<DipolesResult>
    {
        public ICARecord Input { get; init; } = new ICARecord();

        public override DipolesResult Analyze()
        {
            var res = new DipolesResult();

            if (Input.X != default(Record))
            {
                res.Dipoles = Enumerable.Range(0, Input.LeadsCount)
                                        .Select(componentIndex => FindDipole(componentIndex))
                                        .Where(dipole => dipole != default)
                                        .Cast<DipoleResult>()
                                        .ToList();

                res.Succeed = res.Dipoles.Any();
            }
            return res;
        }

        DipoleResult? FindDipole(int componentIndex)
        {
            Debug.Assert(Input.X != default);
            Debug.Assert(componentIndex < Input.LeadsCount);

            var res = default(DipoleResult);

            var componentWeights = Input.GetMixingVector(componentIndex);

            return res;
        }
    }
}
