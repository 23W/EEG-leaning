using EEGCore.Data;
using System.Text.Json;

namespace EEGCore.Processing
{
    public static class LeadExtension
    {
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
}
