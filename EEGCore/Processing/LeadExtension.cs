namespace EEGCore.Processing
{
    public static class LeadExtension
    {
        public static double GetMaximum(this Data.Lead lead) => GeneralUtilities.GetMaximum(lead.Samples);

        public static double GetMinimum(this Data.Lead lead) => GeneralUtilities.GetMinimum(lead.Samples);

        public static Tuple<double, double> GetMinimumMaximum(this Data.Lead lead) => GeneralUtilities.GetMinimumMaximum(lead.Samples);

        public static double GetMean(this Data.Lead lead) => GeneralUtilities.GetMean(lead.Samples);

        public static void AddInplace(this Data.Lead lead, double value) => GeneralUtilities.Add(lead.Samples, value, lead.Samples);
    }
}
