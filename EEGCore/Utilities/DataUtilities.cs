using EEGCore.Data;

namespace EEGCore.Utilities
{
    public static class DataUtilities
    {
        public static string CleanEEGLeadName(string leadName)
        {
            leadName = leadName.Replace("EEG", "")
                               .Replace(".", "")
                               .Replace(",", "")
                               .Replace("-", "")
                               .Replace(" ", "");
            return leadName;
        }

        public static LeadType GetEEGLeadTypeByName(string leadName)
        {
            var leadType = LeadType.Unknown;

            leadName = CleanEEGLeadName(leadName).ToLower();

            if (leadName.StartsWith("AF", StringComparison.OrdinalIgnoreCase) ||
                leadName.StartsWith("F", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Frontal;
            }
            else if (leadName.StartsWith("T", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Temporal;
            }
            else if (leadName.StartsWith("C", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Central;
            }
            else if (leadName.StartsWith("P", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Parietal;
            }
            else if (leadName.StartsWith("O", StringComparison.OrdinalIgnoreCase))
            {
                leadType = LeadType.Occipital;
            }

            return leadType;
        }

        public static LeadCode? GetEEGLeadCodeByName(string leadName)
        {
            leadName = CleanEEGLeadName(leadName).ToLower();

            var leadCode = Enum.GetValues(typeof(LeadCode))
                               .Cast<LeadCode>()
                               .Select(l => Tuple.Create(l.ToString().ToLower(), l))
                               .FirstOrDefault(l => l.Item1.Equals(leadName));

            return leadCode?.Item2;
        }

        internal static int ComparetTo(Lead l1, Lead l2)
        {
            var res = 0;

            var eegLead1 = l1 as EEGLead;
            var eegLead2 = l2 as EEGLead;

            if (eegLead1 != default && eegLead2 != default)
            {
                res = (eegLead1.LeadType - eegLead2.LeadType);
            }
            else if (eegLead1 != default && eegLead2 == default)
            {
                res = -1;
            }
            else if (eegLead1 == default && eegLead2 != default)
            {
                res = 1;
            }

            if (res == 0)
            {
                res = l1.Name.CompareTo(l2.Name);
            }

            return res;
        }
    }
}
