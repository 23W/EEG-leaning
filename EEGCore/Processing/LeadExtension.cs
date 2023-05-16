using EEGCore.Data;
using System.Text.Json;

namespace EEGCore.Processing
{
    public static class LeadExtension
    {
        public static double GetMaximumAbsoluteValue(this IEnumerable<Lead> leads)
        {
            var range = leads.Select(l => l.GetMinimumMaximum())
                             .Aggregate(Tuple.Create(0.0, 0.0),
                                        (r1, r2) => Tuple.Create(Math.Min(r1.Item1, r2.Item1),
                                                                 Math.Max(r1.Item2, r2.Item2)));
            var maxSignalAmpl = Math.Max(Math.Abs(range.Item1),
                                         Math.Abs(range.Item2));
            return maxSignalAmpl;
        }

        public static double GetMaximum(this Lead lead) => GeneralUtilities.GetMaximum(lead.Samples);

        public static double GetMinimum(this Lead lead) => GeneralUtilities.GetMinimum(lead.Samples);

        public static Tuple<double, double> GetMinimumMaximum(this Lead lead) => GeneralUtilities.GetMinimumMaximum(lead.Samples);

        public static double GetMean(this Lead lead) => GeneralUtilities.GetMean(lead.Samples);

        public static void AddInplace(this Lead lead, double value) => GeneralUtilities.Add(lead.Samples, value, lead.Samples);

        public static T Clone<T>(this T lead) where T : Lead, new()
        {
            var json = ToJson(lead);
            var clone = JsonSerializer.Deserialize<T>(json);
            return clone ?? new T();
        }

        public static string ToJson<T>(this T lead) where T : Lead
        {
            var json = JsonSerializer.Serialize<T>(lead);
            return json;
        }
    }

    public static class ComponentLeadExtension
    {
        public static void RemoveArtifactInfo(this ComponentLead lead, ArtifactType artifactType)
        {
            lead.ArtifactInfo.RemoveAll(info => info.ArtifactType == artifactType);
        }

        public static void AddArtifactInfo(this ComponentLead lead, ArtifactType artifactType, bool removeOtherSameType = true)
        {
            AddArtifactInfo(lead, new ArtifactInfo() { ArtifactType = artifactType, Probaprobability = 1.0 }, removeOtherSameType);
        }

        public static void AddArtifactInfo(this ComponentLead lead, ArtifactInfo artifactInfo, bool removeOtherSameType = true)
        {
            bool add = true;

            if (removeOtherSameType)
            {
                add = !lead.ArtifactInfo.Any(a => a.ArtifactType == artifactInfo.ArtifactType &&
                                                  a.Probaprobability >= artifactInfo.Probaprobability);
                if (add)
                {
                    RemoveArtifactInfo(lead, artifactInfo.ArtifactType);
                }
            }

            if (add)
            {
                lead.ArtifactInfo.Add(artifactInfo);
                lead.ArtifactInfo.Sort((a1, a2) => (int)Math.Round((a1.Probaprobability - a2.Probaprobability) * 100));
            }
        }
    }
}
